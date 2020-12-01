using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPad : MonoBehaviour
{
	public Touch touch;

	public Vector2 lastPos;
	public Vector2 direction;

	public int update_interval=2;
	public int update_index=0;
	void show(string s)
	{
		this.gameObject.transform.GetChild(0).GetComponent<Text>().text=s;
	}
	string V2tS(Vector2 v)
	{
		return "["+v.x.ToString()+","+v.y.ToString()+"]";
	}
	void gen_event()
	{
		direction=touch.position-lastPos;
		lastPos=touch.position;
		string e="SEND:INPUT_MOUSE_MOVE_"+V2tS(direction);
		Main.EventQueue.Add(e);
		// show(direction.magnitude.ToString());
		// show(e);

	}
	void Update()
	{
		show("NONE");
		if (Input.touchCount > 0)
		{
			touch = Input.GetTouch(0);
			Vector2 pos= this.gameObject.transform.position;
			pos=Camera.main.WorldToScreenPoint(pos);
			pos-=touch.position;
			// Main.EventQueue.Add("SEND:SHOW_"+V2tS(pos));
			// Main.EventQueue.Add("SEND:SHOW_"+V2tS(new Vector2(pos3.x,pos3.y)-touch.position-this.gameObject.GetComponent<RectTransform>().rect.size));
			// Main.EventQueue.Add("SEND:SHOW_"+V2tS(this.gameObject.GetComponent<RectTransform>().rect.size));
			// Main.EventQueue.Add("SEND:SHOW_"+V2tS(this.gameObject.transform.localScale));
			Vector2 limits=Vector2.Scale(Vector2.Scale(this.gameObject.transform.localScale,this.gameObject.GetComponent<RectTransform>().rect.size),new Vector2(0.5f,0.5f));
			// Main.EventQueue.Add("SEND:SHOW_"+V2tS(limits));
			bool inside= Math.Abs(pos.x)<limits.x && Math.Abs(pos.y)<limits.y;
			// Main.EventQueue.Add("SEND:SHOW_"+inside.ToString());
			// Main.EventQueue.Add("SEND:SHOW_"+this.gameObject.GetComponent<RectTransform>().rect.width.ToString());
			if(!inside) return;
			if(touch.phase==TouchPhase.Began)
			{
				lastPos = touch.position;
				// direction_last=new Vector2(0,0);
			}
			if(touch.phase==TouchPhase.Moved)
			{
				if(update_interval==update_index)
				{
					gen_event();
					update_index=0;
				}
				else
					update_index++;
			}
			if(touch.phase==TouchPhase.Ended)
			{
				gen_event();
				update_index=0;

			}
		}
	}
	void OnMouseDrag()
	{
			if (Input.mousePosition.y < Screen.height / 2) {
					// float rotX = Input.GetAxis ("Mouse X") * rotSpeed * Mathf.Deg2Rad;
					// transform.RotateAround (Vector2.up, -rotX);
					Debug.Log(Input.GetAxis ("Mouse X"));
			}
	}
}
