using UnityEngine;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Sends binary UDP packets to the mask.
/// Each packet is at least 4 bytes: [type, A, B, C] + optional payload.
/// </summary>
public class UDPSender : MonoBehaviour
{
    private UdpClient  _udp;
    public UdpClient Socket => _udp;
    private IPEndPoint _endpoint;

    public void Initialize(string ip, int port)
    {
        _udp      = new UdpClient(port);
        _endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }

    // -------------------------------------------------------
    // Mask commands
    // -------------------------------------------------------

    public void SetMode(MaskMode mode)
        => SendPacket(MaskCommandType.SetMode, 0, 0, (byte)mode);

    public void SetExpression(byte expressionIndex)
        => SendPacket(MaskCommandType.SetExpression, 0, 0, expressionIndex);

    public void SetBrightness(byte brightness)
        => SendPacket(MaskCommandType.SetBrightness, 0, 0, brightness);

    public void QuickSwitch()
        => SendPacket(MaskCommandType.FunctionCall, 0x01, 0, 0);

    /// <param name="seconds">How many seconds before the mask auto-switches expression.</param>
    public void ScheduleQuickSwitch(byte seconds)
        => SendPacket(MaskCommandType.FunctionCall, 0x02, seconds, 0);

    /// <summary>
    /// Push a full 32-pixel frame. Automatically switches mask to Manual mode.
    /// </summary>
    /// <param name="left">16 pixels for the left eye, row-major (index = row*4 + col).</param>
    /// <param name="right">16 pixels for the right eye, row-major.</param>
    public void SetFrame(Color32[] left, Color32[] right)
    {
        if (left.Length != 16 || right.Length != 16)
        {
            Debug.LogError("[UDPSender] SetFrame requires exactly 16 pixels per eye.");
            return;
        }

        byte[] payload = new byte[4 + 96];
        payload[0] = (byte)MaskCommandType.SetFrame;
        payload[1] = 0;
        payload[2] = 0;
        payload[3] = 0;

        for (int i = 0; i < 16; i++)
        {
            payload[4  + i * 3]     = left[i].r;
            payload[4  + i * 3 + 1] = left[i].g;
            payload[4  + i * 3 + 2] = left[i].b;
            payload[52 + i * 3]     = right[i].r;
            payload[52 + i * 3 + 1] = right[i].g;
            payload[52 + i * 3 + 2] = right[i].b;
        }

        Send(payload);
    }

    /// <summary>Set a single pixel on the mask.</summary>
    /// <param name="target">0 = both eyes, 1 = left only, 2 = right only.</param>
    public void SetColor(byte target, byte row, byte col, Color32 color)
    {
        Send(new byte[] {
            (byte)MaskCommandType.SetColor, target, row, col,
            color.r, color.g, color.b
        });
    }

    // -------------------------------------------------------
    // Internal
    // -------------------------------------------------------

    private void SendPacket(MaskCommandType type, byte a, byte b, byte c)
        => Send(new byte[] { (byte)type, a, b, c });

    private void Send(byte[] data)
    {
        if (_udp == null)
        {
            Debug.LogWarning("[UDPSender] Not initialized.");
            return;
        }
        try
        {
            _udp.Send(data, data.Length, _endpoint);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[UDPSender] Send failed: " + e.Message);
        }
    }

    void OnDestroy() => _udp?.Close();
}