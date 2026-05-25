using UnityEngine;
using System.Net;
using System.Threading;
using System.Text;
using System.Net.Sockets;

class UDPReceiver : MonoBehaviour
{
    private Thread receiveThread;
    private UdpClient client = null;


    private int port = 4210;


    public void initialize(UdpClient client)
    {
        this.client = client;
        receiveThread = new Thread(new ThreadStart(ReceiveData))
        {
            IsBackground = true
        };
        receiveThread.Start();
    }


    void ReceiveData()
    {
        client = new UdpClient(port);

        while (true)
        {
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

            byte[] data = client.Receive(ref anyIP);

            string text = Encoding.UTF8.GetString(data);

            Debug.Log(text);
        }
    }
    void OnApplicationQuit()
    {
        receiveThread?.Abort();
        client?.Close();
    }
}
