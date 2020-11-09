using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MUE_Unity
{
  public static Canvas canvas;
  public static GameObject buttonPrefab;
  public static GameObject CreateButton(Vector2 pos, Vector2 scale,string Btxt,string imgPath="")
  {
    var button = Object.Instantiate(buttonPrefab,new Vector3(pos.x,pos.y,0), Quaternion.identity);
    var rectTransform = button.GetComponent<RectTransform>();
    rectTransform.SetParent(canvas.transform);
    rectTransform.localScale=new Vector3(scale.x,scale.y,1);


    // rectTransform.anchorMax = cornerTopRight;
    // rectTransform.anchorMin = cornerBottomLeft;
    // rectTransform.offsetMax = Vector2.zero;
    // rectTransform.offsetMin = Vector2.zero;
    //Debug.Log(button.transform.GetChild(0).GetComponent<Text>().text);
    button.transform.GetChild(0).GetComponent<Text>().text=Btxt;
    if(imgPath.Length>0)
      button.GetComponent<Button>().GetComponent<Image>().sprite=Resources.Load<Sprite>(imgPath);
    return button;
  }

  public static GameObject CreateObj(Vector2 pos, Vector2 scale,string prefabNAME)
  {
    // Debug.Log(Resources.Load<GameObject>(prefabNAME));
    var obj = Object.Instantiate(Resources.Load<GameObject>(prefabNAME),new Vector3(pos.x,pos.y,0), Quaternion.identity);
    var rectTransform = obj.GetComponent<RectTransform>();
    rectTransform.SetParent(canvas.transform);
    rectTransform.localScale=new Vector3(scale.x,scale.y,1);

    return obj;
  }
}
public class UIs : MonoBehaviour
{
  public GameObject buttonPrefab;
  public Canvas canvas;

  public void sync()
  {
    MUE_Unity.canvas=canvas;
    MUE_Unity.buttonPrefab=buttonPrefab;
  }
  void Start()
  {
    sync();
  }
  void Update()
  {

  }
  public class MAIN
  {
    public static GameObject B_mouse;
    public static GameObject B_keyboard;
    public static GameObject B_setings;
    public static GameObject B_exit;
    public static void Load()
    {
      // Button B;
      B_mouse=MUE_Unity.CreateButton(new Vector2(0,200),new Vector2(4,4),"","Img/Mouse");
      B_mouse.GetComponent<Button>().onClick.AddListener(Unload);
      B_mouse.GetComponent<Button>().onClick.AddListener(UIs.MOUSE.Load);

      B_keyboard=MUE_Unity.CreateButton(new Vector2(0,0),new Vector2(4,4),"","Img/Keyboard");
      B_keyboard.GetComponent<Button>().onClick.AddListener(() =>{Debug.Log("B");});

      B_setings=MUE_Unity.CreateButton(new Vector2(0,-200),new Vector2(4,4),"Setings");
      B_setings.GetComponent<Button>().onClick.AddListener(() =>{Debug.Log("C");});

      B_exit=MUE_Unity.CreateButton(new Vector2(570,290),new Vector2(3,3),"X","Img/Square");
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
      Destroy(B_setings);
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


      B_back=MUE_Unity.CreateButton(new Vector2(580,300),new Vector2(2,2),"<=","Img/Square");
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
}
