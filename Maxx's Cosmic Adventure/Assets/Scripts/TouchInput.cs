using UnityEngine;
using System.Collections;

public class TouchInput : MonoBehaviour {
	
	public GUITexture LeftGUI,RightGUI,FireLeftGUI,FireRightGUI;
	public GUITexture MGUI,BGUI;
	public GUITexture PowerUpAutoFire;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	private bool Clicked(GUITexture gui)
	{
		foreach(Touch touch in Input.touches)
			if( gui.HitTest(touch.position) )
				return true;
		/*Vector2 mousepos = Input.mousePosition;
		mousepos.x /= Screen.width; mousepos.y /= Screen.height;
		if( gui.HitTest( mousepos ) )
			return true;*/
		return false;
	}
	
	public bool Left { get { return Clicked(LeftGUI); }}
	public bool Right { get { return Clicked(RightGUI); }}
	public bool FireLeft { get { return Clicked(FireLeftGUI); }}
	public bool FireRight { get { return Clicked(FireRightGUI); }}
	
	public bool B { get { return Clicked(BGUI) || Input.GetKey(KeyCode.B); }}
	public bool M { get { return Clicked(MGUI) || Input.GetKey(KeyCode.M); }}
	
	public float AxisHorizontal { get {
			if( Left && Right ) return 0.0f;
			if( Left ) return -1.0f;
			if( Right ) return 1.0f;
			return 0.0f;
		}}
	
	void OnGUI()
	{		
		//GUI.Label(new Rect(0,110,200,200), "bool : " + Left );
	}
	
	// update
	
			
	public bool FireLeftWithPhase(TouchPhase phase)
	{
		foreach(Touch touch in Input.touches)
			if( touch.phase == phase && FireLeftGUI.HitTest(touch.position) )
				return true;
		return false;
	}
		
	public bool LeftWithPhase(TouchPhase phase)
	{
		foreach(Touch touch in Input.touches)
			if( touch.phase == phase && LeftGUI.HitTest(touch.position) )
				return true;
		return false;
	}
	
	public bool RightWithPhase(TouchPhase phase)
	{
		foreach(Touch touch in Input.touches)
			if( touch.phase == phase && RightGUI.HitTest(touch.position) )
				return true;
		return false;
	}
		
	public bool MWithPhase(TouchPhase phase)
	{
		foreach(Touch touch in Input.touches)
			if( touch.phase == phase && MGUI.HitTest(touch.position) )
				return true;
		return false;
	}
		
	public bool BightWithPhase(TouchPhase phase)
	{
		foreach(Touch touch in Input.touches)
			if( touch.phase == phase && BGUI.HitTest(touch.position) )
				return true;
		return false;
	}
	
	public bool PowerUpAutoWithPhase(TouchPhase phase)
	{
		foreach(Touch touch in Input.touches)
			if( touch.phase == phase && PowerUpAutoFire.HitTest(touch.position) )
				return true;
		return false;
	}
}
