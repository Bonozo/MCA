using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {
	
	public GUITexture MainMenuGUI,ResumeGUI;
	
	private Store store;
	
	void Start()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
	}
	
	/*void OnMouseUp()
	{
		Time.timeScale = 1-Time.timeScale;
	}*/
	
	void Update()
	{
		foreach(Touch touch in Input.touches)
			if( touch.phase == TouchPhase.Ended && guiTexture.HitTest(touch.position) )
				Time.timeScale = 1-Time.timeScale;
		
		store.Active = Time.timeScale == 0.0f;
		
		/*if( paused)
		{
			foreach(Touch touch in Input.touches)
			{
				if( touch.phase == TouchPhase.Ended && MainMenuGUI.HitTest(touch.position) || MainMenuGUI.HitTest(Input.mousePosition) )
					Application.LoadLevel("start");
				if( touch.phase == TouchPhase.Ended && ResumeGUI.HitTest(touch.position) || ResumeGUI.HitTest(Input.mousePosition) )
					paused = false;
			}
			
			if( Input.GetMouseButton(0) )
			{
				Vector2 mouse = Input.mousePosition;
				mouse.x /= Screen.width; mouse.y /= Screen.height;
				if( MainMenuGUI.HitTest(Input.mousePosition) )
					Application.LoadLevel("mainmenu");
				if( ResumeGUI.HitTest(Input.mousePosition) )
						paused = false;
			}
		}
		
		Time.timeScale = paused?0f:1f;
		MainMenuGUI.enabled = ResumeGUI.enabled = paused;*/
	}
}
