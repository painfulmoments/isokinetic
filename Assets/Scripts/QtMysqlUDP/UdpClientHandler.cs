using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpClientHandler
{
    Socket socket; 
    EndPoint serverEnd; 
    IPEndPoint ipEnd; 
    string recvStr; 
    string sendStr; 
    byte[] recvData = new byte[1024]; 
    byte[] sendData = new byte[1024]; 
    int recvLen; 
    Thread connectThread;
    //udp 
    private string unityIP;
    private int unityPort;
    private string QTIP;
    private int QTPort;

    #region singleton
    private static UdpClientHandler instance;
    public static UdpClientHandler getInstance()
    {
        if (instance == null)
        {
            instance = new UdpClientHandler();
        }
        return instance; ;
    }
    #endregion
    public UdpClientHandler()
    {
        Debug.Log("UdpClientHandler()--unity udp socket Initialize instance");
    }
    public void InitSocket()
    {
        QTIP = ReadConfig.instance.QTIP;
        QTPort = ReadConfig.instance.QTPort;
        unityIP = ReadConfig.instance.unityIP;
        unityPort = ReadConfig.instance.unityPort;
        Debug.Log("QTIP:" + QTIP + ",QTPort:" + QTPort + ",unityIP:" + unityIP + ",unityPort:" + unityPort);
        //receiver ip and port
        ipEnd = new IPEndPoint(IPAddress.Parse(QTIP), QTPort);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //send ip and port
        IPEndPoint sender = new IPEndPoint(IPAddress.Parse(unityIP), unityPort);
        serverEnd = (EndPoint)sender;
        SocketSend("unity to QT");
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    public void SocketSend(string sendStr)
    {
        sendData = new byte[1024];
        sendData = Encoding.ASCII.GetBytes(sendStr);
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
        Debug.Log("SocketSend()--udp:unity send to QT,str:"+sendStr);
    }

    void SocketReceive()
    {
        while (true)
        {
            recvData = new byte[1024];
            recvLen = socket.ReceiveFrom(recvData, ref serverEnd);
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
        }
    }

    public string GetRecvStr()
    {
        string returnStr;       
        lock (this)
        {
            returnStr = recvStr;
        }
        return returnStr;
    }

    public void SocketQuit()
    {
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        if (socket != null)
        {
            socket.Close();           
        }
        Debug.Log("SocketQuit()--unity udp socket closed");
    }
}
