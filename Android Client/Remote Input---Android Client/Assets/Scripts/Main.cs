using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;

public class Main : MonoBehaviour
{
	// Start is called before the first frame update
	public UIs uis;
	private TcpClient socketConnection;

	// settings
	public static string ip {get; private set;}
	public static int port {get; private set;}

	private string settingsPath;
	private int LinesInSettings=10;
	void connect()
	{
		if(port==-1)
			return;
		if(socketConnection!=null)
		{
			socketConnection.Close();
			socketConnection=null;
		}
		try
		{
			Debug.Log("Trying to connect");
			socketConnection = new TcpClient(ip, port);
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
			return;
		}
		Debug.Log("connected");
	}
	void InitSettings()
	{
		File.Create(settingsPath).Dispose();

		string[] settings=new string[LinesInSettings];
		// IP
		settings[0]="127.0.0.1";
		// port
		settings[1]="-1";
		File.WriteAllLines(settingsPath,settings);
	}
	void WriteSettings()
	{
		string[] settings=new string[LinesInSettings];
		// IP
		settings[0]=ip;

		// port
		settings[1]=port.ToString();
		File.WriteAllLines(settingsPath,settings);
	}
	void ReadSettings()
	{
		string[] settings=new string[LinesInSettings];
		settings=File.ReadAllLines(settingsPath);
		// IP
		ip=settings[0];

		// port
		port=Int32.Parse(settings[1]);
	}
	void Start()
	{
		settingsPath = Application.persistentDataPath + "/settings.txt";
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 1;
		EventQueue=new List<string>();

		if(!File.Exists(settingsPath))
			InitSettings();
		ReadSettings();

		uis.sync();

		UIs.MAIN.Load();
		// UIs.MAIN.Unload();

		connect();
	}
	public static void Show(string str)
	{
		Main.EventQueue.Add("SEND:SHOW_"+str);
	}
	public static List<string> EventQueue;
	private void Send(byte[] bytes)
	{
		if(socketConnection==null)
		{
			connect();
			if(socketConnection==null)
				{
					Debug.Log("DISCARDED");
					return;
				}
		}
		//Encoding.ASCII.GetBytes(clientMessage);
		NetworkStream stream = socketConnection.GetStream();
		stream.Write(bytes, 0, bytes.Length);
	}
	public static void Quit()
	{
		Debug.Log("EXIT");
		// socketConnection.Close();
		Application.Quit();
	}
	void Update()
	{
		// connect();
		if(EventQueue.Count>0)
		{
			if(EventQueue[0].Substring(0,5)=="SEND:")
			{
				Send(Encoding.ASCII.GetBytes(EventQueue[0].Remove(0,5)+"|||"));
				EventQueue.RemoveAt(0);
			}
			else if(EventQueue[0].Substring(0,7)=="UPDATE:")
			{
				if(EventQueue[0].Substring(0,7+5)=="UPDATE:PORT:")
				{
					port=Int32.Parse(EventQueue[0].Substring(7+5));
				}
				else if(EventQueue[0].Substring(0,7+3)=="UPDATE:IP:")
				{
					ip=EventQueue[0].Substring(7+3);
				}
				else
				{
					Debug.Log("UNKNOWN EVENT"+EventQueue[0]);
					EventQueue.RemoveAt(0);
					return;
				}
				connect();
				WriteSettings();

			}
			else
			{
				Debug.Log("UNKNOWN EVENT"+EventQueue[0]);
			}
			EventQueue.RemoveAt(0);


		}
	}
}
