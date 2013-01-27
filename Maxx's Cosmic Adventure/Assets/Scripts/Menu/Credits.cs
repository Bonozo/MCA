using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Credits")]
public class Credits : MonoBehaviour {
	
	[System.Serializable]
	public class CreditStruct
	{
		public string title;
		public string[] name;
	}
	
	public CreditStruct[] members;
	
	public float Speed = 0.05f;
	
	public GUIStyle guiStyleTitle;
	public GUIStyle guiStyleName;
	private float currentHeight = -1f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.touchCount == 0 )
			currentHeight += Time.deltaTime*Speed;
	}
	
	void OnEnable()
	{
		currentHeight = -1;
	}
	
	private Rect RectScreen(float a,float b,float w,float h)
	{
		return new Rect(a*Screen.width,b*Screen.height,w*Screen.width,h*Screen.height);
	}
	
	void OnGUI()
	{
		foreach(Touch touch in Input.touches)
				currentHeight += 0.25f*touch.deltaPosition.y/Screen.height;
		if( currentHeight < -1 ) currentHeight = -1;
		
		float index = -currentHeight;
		
		foreach(var mb in members )
		{
			GUI.Label(RectScreen(0.25f,index,0.5f,0.1f),mb.title,guiStyleTitle);
			index += 0.1f;
			foreach(var nm in mb.name)
			{
				GUI.Label(RectScreen(0.2f,index,0.6f,0.1f),nm,guiStyleName);
				index += 0.1f;
			}
			index += 0.1f;
		}
		if(index<0f)
		{
			currentHeight = -1;
			MainMenu.Instance.State = MainMenu.MenuState.Title;
		}
	}
}
