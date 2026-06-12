using UnityEngine;

/// <summary>
/// Coordinates the UDP sender and receiver for the cosplay mask.
///
/// This is the only script other MonoBehaviours should talk to —
/// use the public Send* methods to command the mask, and subscribe
/// to Receiver.OnStateReceived (or read Receiver.LatestState) for feedback.
///
/// Requires UDPSender and UDPReceiver on the same GameObject.
/// </summary>
[RequireComponent(typeof(UDPSender))]
[RequireComponent(typeof(UDPReceiver))]
public class UDPHandler : MonoBehaviour
{
    [SerializeField] private string maskIP = "192.168.4.1";
    [SerializeField] private int maskPort = 4210;

    public UDPSender Sender { get; private set; }
    public UDPReceiver Receiver { get; private set; }

    void Awake()
    {
        Sender = GetComponent<UDPSender>();
        Receiver = GetComponent<UDPReceiver>();
        Sender.Initialize(maskIP, maskPort);
        Receiver.Initialize(Sender.Socket);
    }
    void OnApplicationQuit()
    {
        Sender?.GetComponent<UDPSender>()?.OnDestroy();
        Receiver?.GetComponent<UDPReceiver>()?.OnDestroy();
    }

    // -------------------------------------------------------
    // Convenience pass-throughs so callers don't need a Sender reference.
    // -------------------------------------------------------

    public void SendSetMode(MaskMode mode) => Sender.SetMode(mode);
    public void SendSetExpression(byte index) => Sender.SetExpression(index);
    public void SendSetBrightness(byte brightness) => Sender.SetBrightness(brightness);
    public void SendQuickSwitch() => Sender.QuickSwitch();
    public void SendSetLookDirection(byte direction, byte durationSteps) => Sender.SetLookDirection(direction, durationSteps);
    public void SendReleaseLookDirection() => Sender.ReleaseLookDirection();
    public void SendScheduleQuickSwitch(byte seconds) => Sender.ScheduleQuickSwitch(seconds);
    public void SendSetFrame(Color32[] left, Color32[] right) => Sender.SetFrame(left, right);
    public void SendSetColor(byte target, byte row, byte col, Color32 color) => Sender.SetColor(target, row, col, color);

    // -------------------------------------------------------
    // Quick test callable from the Inspector context menu.
    // -------------------------------------------------------
    [ContextMenu("Test — Set expression 2")]
    private void Test() => SendSetExpression(2);
    [ContextMenu("Test — Look right for 1 second")]
    private void Test2() => SendSetLookDirection(0, 10);
    [ContextMenu("Test — Draw frame")]
    private void Test3()
    {
        Color32[] left = new Color32[16];
        Color32[] right = new Color32[16];

        for (int i = 0; i < 16; i++)
        {
            left[i] = UnityEngine.Random.ColorHSV();
            right[i] = UnityEngine.Random.ColorHSV();
        }   
        SendSetFrame(left, right);
    }
}