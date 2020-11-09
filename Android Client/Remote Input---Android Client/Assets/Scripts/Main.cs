using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;

public class Main : MonoBehaviour
{
  // Start is called before the first frame update
  public UIs uis;
  private TcpClient socketConnection;
  private string ip="192.168.0.200";
  private int PORT=999;
  void connect()
  {
    if(socketConnection==null)
    {
      try
      {
        Debug.Log("Trying to connect");
        socketConnection = new TcpClient(ip, PORT);
      }
      catch (SocketException socketException)
      {
        Debug.Log("Socket exception: " + socketException);
      }
      Debug.Log("connected");
    }
  }
  void Start()
  {
    Application.targetFrameRate = 60;
    QualitySettings.vSyncCount = 1;
    EventQueue=new List<string>();
    uis.sync();
    {
    UIs.MAIN.Load();
    // UIs.MAIN.Unload();

    }
    // Debug.Log("SEND:AAA".Remove(0,5));
    connect();
    connect();
  }
  public static List<string> EventQueue;
  // Update is called once per frame
  private void Send(byte[] bytes)
  {
    //Encoding.ASCII.GetBytes(clientMessage);
    NetworkStream stream = socketConnection.GetStream();
		stream.Write(bytes, 0, bytes.Length);
  }
  public static void Quit()
  {
    Debug.Log("EXIT");
    Application.Quit();
  }
  void Update()
  {
    // connect();
    if(EventQueue.Count>0)
    {
      if(EventQueue[0].Contains("SEND:"))
      {
        Send(Encoding.ASCII.GetBytes(EventQueue[0].Remove(0,5)+"|||"));
        EventQueue.RemoveAt(0);
      }
    }
  }
}
