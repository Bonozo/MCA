using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Button Base")]
public class ButtonBase : MonoBehaviour {
	
	public AudioSource audioPressed;
	public Texture standartTexture;
	public Texture pressedTexture;
	public bool canPressed = true;
	public GUITexture workingGUI;
	
	private bool down = false;
	private bool up = false;
	
	public bool PressedDown { get {if(down&&!up) {down=up=false; if(audioPressed!=null) audioPressed.Play(); return true;} else return false;} }
	public bool PressedUp { get {if(up) {down=up=false; if(audioPressed!=null) audioPressed.Play(); return true;} else return false;} }
	
	public void Ignore() { down=up=false; }
	
	virtual protected void Start()
	{
		if(workingGUI==null) workingGUI=this.GetComponent<GUITexture>();
	}
	
	virtual protected void Update()
	{
		if(dsbl) return;
		
		foreach(Touch touch in Input.touches)
		{
			if( workingGUI.HitTest(touch.position ) )
			{
				if(!aspressed) GetComponent<GUITexture>().texture = (touch.phase!=TouchPhase.Ended && canPressed)?pressedTexture:standartTexture;
				if( touch.phase == TouchPhase.Began ) down = true;
				if( touch.phase == TouchPhase.Ended && down) up = true;
				return;
			}
		}
		
		if( workingGUI.HitTest(Input.mousePosition) )
		{
			if( Input.GetMouseButtonDown(0) ) down=true;
			if( Input.GetMouseButtonUp(0) && down) up=true;
			if(!aspressed) GetComponent<GUITexture>().texture = (Input.GetMouseButton(0) && canPressed)?pressedTexture:standartTexture;
			return;
		}
		
		down = up = false;
		if(!aspressed) GetComponent<GUITexture>().texture = standartTexture;
	}
	
	private bool aspressed = false;
	public void SetAsPressed()
	{
		aspressed = true;
		GetComponent<GUITexture>().texture = pressedTexture;
	}
	
	private bool dsbl = false;
	public void DisableButtonForUse()
	{
		dsbl = true;
		GetComponent<GUITexture>().texture = standartTexture;
	}
	
	public void EnableButtonForUse()
	{
		dsbl = false;
	}
}
