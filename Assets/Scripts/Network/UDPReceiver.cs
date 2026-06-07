using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// Listens for state broadcasts sent from the mask and exposes them as Unity data.
///
/// Incoming packet layout (100 bytes):
///   [0]      0x07  — STATE_BROADCAST type tag
///   [1]      mode  — current MaskMode value
///   [2]      expression index
///   [3]      brightness (0–255)
///   [4..51]  left eye  — 16 × RGB, row-major (index = row*4 + col)
///   [52..99] right eye — 16 × RGB, row-major
///
/// Thread safety: ReceiveLoop runs on a background thread. All Unity-side
/// access (LatestState, OnStateReceived) is delivered through Update() on
/// the main thread, so it is safe to read from MonoBehaviours.
/// </summary>
public class UDPReceiver : MonoBehaviour
{
    private const byte STATE_BROADCAST_TAG = 0x07;
    private const int  PACKET_SIZE         = 100;

    public int listenPort = 4210;

    /// <summary>The most recently received mask state. Never null after first packet.</summary>
    public MaskState LatestState { get; private set; } = new MaskState();

    /// <summary>Raised on the main thread whenever a new state packet arrives.</summary>
    public event Action<MaskState> OnStateReceived;

    private UdpClient        _udp;
    private Thread           _thread;
    private volatile bool    _running;
    private MaskState        _pending;
    private readonly object  _lock      = new object();
    private bool             _hasPending;

    public void Initialize(UdpClient sharedSocket)
{
    _udp     = sharedSocket;
    _running = true;
    _thread  = new Thread(ReceiveLoop) { IsBackground = true, Name = "MaskUDPReceiver" };
    _thread.Start();
}

    // Swaps the latest received state onto the main thread once per frame.
    void Update()
    {
        MaskState arrived = null;
        lock (_lock)
        {
            if (_hasPending)
            {
                arrived     = _pending;
                _pending    = null;
                _hasPending = false;
            }
        }

        if (arrived != null)
        {
            LatestState = arrived;
            OnStateReceived?.Invoke(arrived);
        }
    }

    // Runs on background thread — never touches Unity objects directly.
    private void ReceiveLoop()
    {
        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);

        while (_running)
        {
            try
            {
                byte[] data = _udp.Receive(ref remote);

                if (data.Length < PACKET_SIZE || data[0] != STATE_BROADCAST_TAG)
                    continue;

                MaskState state = ParseState(data);

                lock (_lock)
                {
                    _pending    = state;
                    _hasPending = true;
                }
            }
            catch (SocketException)
            {
                break; // Socket was closed during shutdown — exit cleanly.
            }
            catch (Exception e)
            {
                Debug.LogWarning("[UDPReceiver] " + e.Message);
            }
        }
    }

    private static MaskState ParseState(byte[] data)
    {
        var state = new MaskState
        {
            Mode        = (MaskMode)data[1],
            Expression  = data[2],
            Brightness  = data[3],
            LeftPixels  = new Color32[16],
            RightPixels = new Color32[16],
        };

        for (int i = 0; i < 16; i++)
        {
            state.LeftPixels[i]  = new Color32(data[4  + i*3], data[5  + i*3], data[6  + i*3], 255);
            state.RightPixels[i] = new Color32(data[52 + i*3], data[53 + i*3], data[54 + i*3], 255);
        }

        return state;
    }

    void OnDisable()
    {
        _running = false;
        _udp?.Close();
        _thread?.Join(500);
    }
}

// -------------------------------------------------------

/// <summary>A snapshot of the mask's full state at a point in time.</summary>
public class MaskState
{
    public MaskMode Mode;
    public byte     Expression;
    public byte     Brightness;

    /// <summary>Left eye pixels, Color32[16], row-major (index = row*4 + col).</summary>
    public Color32[] LeftPixels  = new Color32[16];

    /// <summary>Right eye pixels, Color32[16], row-major (index = row*4 + col).</summary>
    public Color32[] RightPixels = new Color32[16];

    public Color32 GetPixel(bool leftEye, int row, int col)
        => (leftEye ? LeftPixels : RightPixels)[row * 4 + col];
}