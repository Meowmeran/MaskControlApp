using UnityEngine;
using System.Text;
using System.Net.Sockets;

class UDPSender : MonoBehaviour
{
    private UdpClient client = null;
    private int port = 4210;

    public bool Initialize(UdpClient client, int port)
    {
        this.port = port;
        this.client = client;
        return true;
    }


    public void SendData(string text)
    {
        client.Send(Encoding.UTF8.GetBytes(text), text.Length);
    }

    public void SendData(int text)
    {
        client.Send(Encoding.UTF8.GetBytes(text.ToString()), text.ToString().Length);
    }
    
}
