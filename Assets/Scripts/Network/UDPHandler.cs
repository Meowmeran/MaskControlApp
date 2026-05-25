using UnityEngine;
using System.Net;
using System.Threading;
using System.Text;
using System.Net.Sockets;

[RequireComponent(typeof(UDPReceiver))]
[RequireComponent(typeof(UDPSender))]
public class UDPHandler : MonoBehaviour
{
    [SerializeField] private string ip = "127.0.0.1";
    [SerializeField] private int port = 4210;
    private UdpClient client;
        
    private UDPReceiver udpReceiver;
    private UDPSender udpSender;
    void Start()
    {
        udpReceiver = GetComponent<UDPReceiver>();
        udpSender = GetComponent<UDPSender>();

        udpReceiver.initialize(client);
        udpSender.initialize(client);
    }

    public void Send(string text)
    {
        udpSender.SendData(text);
        Debug.Log("Send: " + text);
    }

    public void Send(byte type, byte a, byte b, byte c)
    {
        int data = (type << 24) | (a << 16) | (b << 8) | c;
        udpSender.SendData(data);
    }

    public void Send(int data)
    {
        udpSender.SendData(data);
    }

    [ContextMenu("Test")]
    public void Test()
    {
        udpSender.SendData("test");
    }
}
