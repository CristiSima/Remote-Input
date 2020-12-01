using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MUE_Unity
{
	public static Canvas canvas;
	public static GameObject CreateButton(Vector2 pos, Vector2 scale,string Btxt,string imgPath="",int font=14)
	{
		var button = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Button"),new Vector3(pos.x,pos.y,0), Quaternion.identity);
		var rectTransform = button.GetComponent<RectTransform>();
		rectTransform.SetParent(canvas.transform);
		rectTransform.localScale=new Vector3(scale.x,scale.y,1);


		if(scale.x>scale.y)
		{
			button.transform.GetChild(0).GetComponent<RectTransform>().localScale=new Vector3(scale.y/scale.x,1,1);
			button.transform.GetChild(0).GetComponent<RectTransform>().localScale=new Vector3(1,scale.x/scale.y,1);
		}

		// rectTransform.anchorMax = cornerTopRight;
		// rectTransform.anchorMin = cornerBottomLeft;
		// rectTransform.offsetMax = Vector2.zero;
		// rectTransform.offsetMin = Vector2.zero;
		//Debug.Log(button.transform.GetChild(0).GetComponent<Text>().text);

		button.transform.GetChild(0).GetComponent<Text>().fontSize=font;
		button.transform.GetChild(0).GetComponent<Text>().text=Btxt;
		if(imgPath.Length>0)
			button.GetComponent<Button>().GetComponent<Image>().sprite=Resources.Load<Sprite>(imgPath);
		return button;
	}
	public static GameObject CreateObj(Vector2 pos, Vector2 scale,string prefabNAME)
	{
		// Debug.Log(Resources.Load<GameObject>(prefabNAME));
		var obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(prefabNAME),new Vector3(pos.x,pos.y,0), Quaternion.identity);
		var rectTransform = obj.GetComponent<RectTransform>();
		rectTransform.SetParent(canvas.transform);
		rectTransform.localScale=new Vector3(scale.x,scale.y,1);

		return obj;
	}
	public static GameObject CreateInput(Vector2 pos, Vector2 scale,string initial_value="", string place_holder="")
	{
		// Debug.Log(Resources.Load<GameObject>("Prefabs/InputField"));
		var obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/InputField"),new Vector3(pos.x,pos.y,0), Quaternion.identity);
		var rectTransform = obj.GetComponent<RectTransform>();
		rectTransform.SetParent(canvas.transform);

		rectTransform.localScale=new Vector3(scale.x,scale.y,1);

		obj.GetComponent<InputField>().text=initial_value;

		// Debug.Log(obj.GetComponent<InputField>());
		// Debug.Log(obj.GetComponent<InputField>().placeholder.GetComponent<Text>());
		// Debug.Log(obj.GetComponent<InputField>().placeholder.GetComponent<Text>().text);
		obj.GetComponent<InputField>().placeholder.GetComponent<Text>().text=place_holder;

		return obj;
	}
}
public class UIs : MonoBehaviour
{
	public Canvas canvas;

	public void sync()
	{
		MUE_Unity.canvas=canvas;
	}
	void Start()
	{
		sync();
	}
	public class MAIN
	{
		public static GameObject B_mouse;
		public static GameObject B_keyboard;
		public static GameObject B_settings;
		public static GameObject B_exit;
		public static void Load()
		{
			// Button B;
			B_mouse=MUE_Unity.CreateButton(new Vector2(0,200),new Vector2(4,4),"","Img/Mouse");
			B_mouse.GetComponent<Button>().onClick.AddListener(Unload);
			B_mouse.GetComponent<Button>().onClick.AddListener(UIs.MOUSE.Load);

			B_keyboard=MUE_Unity.CreateButton(new Vector2(0,0),new Vector2(4,4),"","Img/Keyboard");
			B_keyboard.GetComponent<Button>().onClick.AddListener(() =>{Debug.Log("B");});

			B_settings=MUE_Unity.CreateButton(new Vector2(0,-200),new Vector2(4,4),"settings",font:12);
			B_settings.GetComponent<Button>().onClick.AddListener(Unload);
			B_settings.GetComponent<Button>().onClick.AddListener(UIs.SETTINGS.Load);

			B_exit=MUE_Unity.CreateButton(new Vector2(570,290),new Vector2(3,3),"X","Img/Square",font:30);
			B_exit.GetComponent<Button>().onClick.AddListener(() =>{Debug.Log("D");});
			B_exit.GetComponent<Button>().onClick.AddListener(Main.Quit);

			// B=B_mouse.GetComponent<Button>();
			//B.onClick.AddListener(Unload);
			// Debug.Log("Constructed");
		}
		public static void Unload()
		{
			Destroy(B_mouse);
			Destroy(B_keyboard);
			Destroy(B_settings);
			Destroy(B_exit);
			 // Debug.Log("Destroyed?");
		}
	}
	public class MOUSE
	{
		public static GameObject B_Rclick;
		public static GameObject B_Lclick;
		public static GameObject B_scrol;
		public static GameObject B_pad;
		public static GameObject B_back;
		public static void Load()
		{
			// Debug.Log(Resources.Load<GameObject>("Prefabs/Pad"));
			B_pad=MUE_Unity.CreateObj(new Vector2(50,0),new Vector2(950,720),"Prefabs/Pad");

			B_Rclick=MUE_Unity.CreateButton(new Vector2(-532,-180),new Vector2(4,7),"R","Img/Square");
			B_Rclick.GetComponent<Button>().onClick.AddListener(() =>{Main.EventQueue.Add("SEND:INPUT_MOUSE_RCLICK");});


			B_Lclick=MUE_Unity.CreateButton(new Vector2(-532,180),new Vector2(4,7),"L","Img/Square");
			B_Lclick.GetComponent<Button>().onClick.AddListener(() =>{Main.EventQueue.Add("SEND:INPUT_MOUSE_LCLICK");});


			B_back=MUE_Unity.CreateButton(new Vector2(580,300),new Vector2(2,2),"<=","Img/Square",font:20);
			B_back.GetComponent<Button>().onClick.AddListener(Unload);
			B_back.GetComponent<Button>().onClick.AddListener(UIs.MAIN.Load);


		}
		public static void Unload()
		{
			Destroy(B_pad);
			Destroy(B_Rclick);
			Destroy(B_Lclick);
			Destroy(B_back);
			 // Debug.Log("Destroyed?");
		}
	}
	public class SETTINGS
	{
		public static GameObject IF_ip;
		public static GameObject IF_port;
		public static GameObject B_back;
		public static GameObject B_confirm;
		static bool validate()
		{
			// checks if data in fields is valid
			// TODO add checks
			if(Int32.Parse(IF_port.GetComponent<InputField>().text)>65000)
				IF_port.GetComponent<InputField>().text=Main.port.ToString();
			return true;
		}
		static void UpdateSettings()
		{
			if(IF_ip.GetComponent<InputField>().text!=Main.ip)
			{
				// update ip
				Main.EventQueue.Add("UPDATE:IP:"+IF_ip.GetComponent<InputField>().text);
				// Debug.Log(Main.ip+"->"+IF_ip.GetComponent<InputField>().text);
			}

			if(IF_port.GetComponent<InputField>().text!=Main.port.ToString())
			{
				// update port
				Main.EventQueue.Add("UPDATE:PORT:"+IF_port.GetComponent<InputField>().text);
				// Debug.Log(Main.port.ToString()+"->"+IF_port.GetComponent<InputField>().text);
			}
		}
		public static void Load()
		{
			IF_ip=MUE_Unity.CreateInput(new Vector2(0,200),new Vector2(2,2),Main.ip);
			IF_port=MUE_Unity.CreateInput(new Vector2(0,0),new Vector2(2,2),Main.port.ToString());

			B_back=MUE_Unity.CreateButton(new Vector2(580,300),new Vector2(2,2),"<=","Img/Square",font:20);
			B_back.GetComponent<Button>().onClick.AddListener(Unload);
			B_back.GetComponent<Button>().onClick.AddListener(UIs.MAIN.Load);

			B_confirm=MUE_Unity.CreateButton(new Vector2(-530,-280),new Vector2(4,2),"Confirm","Img/Square",font:13);
			B_confirm.GetComponent<Button>().onClick.AddListener(()=>
			{
				if(validate())
					UpdateSettings();
				else
				{
					// TODO ADD some better indication
					// some indication
				}
			});
		}
		public static void Unload()
		{
			Destroy(IF_ip);
			Destroy(IF_port);

			Destroy(B_back);
			Destroy(B_confirm);

		}
	}
}
