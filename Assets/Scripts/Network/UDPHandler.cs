using UnityEngine;
using System.Net;
using System.Threading;
using System.Text;
using System.Net.Sockets;

[RequireComponent(typeof(UDPReceiver))]
[RequireComponent(typeof(UDPSender))]
public class UDPHandler : MonoBehaviour
{
    [SerializeField] private string ip = "192.168.4.1";
    [SerializeField] private int port = 4210;
    private UdpClient client;
        
    private UDPReceiver udpReceiver;
    private UDPSender udpSender;
    void Start()
    {
        client = new UdpClient(ip, port);
        udpReceiver = GetComponent<UDPReceiver>();
        udpSender = GetComponent<UDPSender>();

        udpReceiver.Initialize(client, port);
        udpSender.Initialize(client, port);
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
        Debug.Log("Send: " + data);
    }

    public void Send(int data)
    {
        udpSender.SendData(data);
        Debug.Log("Send: " + data);
    }

    [ContextMenu("Test")]
    public void Test()
    {
        CommandType type = CommandType.SET_EXPRESSION;
        Send((byte)type, 0, 0, 2);
    }


}


  enum CommandType
  {
    SET_MODE = 0x01,
    SET_EXPRESSION = 0x02,
    SET_BRIGHTNESS = 0x03,
    FUNCTION_CALL = 0x04
  };
