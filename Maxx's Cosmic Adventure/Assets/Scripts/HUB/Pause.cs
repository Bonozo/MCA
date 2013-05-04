using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
	
	public Transform left1,left2,right1,right2;
	public UIButton[] buttons;
	public GameObject popupQuitGame;
	
	bool ppause = false;
	float maxx = 1500;
	float speed = 4000;
	
	void Awake()
	{
		left1.localPosition  = new Vector3(-maxx,left1.localPosition.y,left1.localPosition.z);
		left2.localPosition  = new Vector3(-maxx,left2.localPosition.y,left2.localPosition.z);
		right1.localPosition = new Vector3( maxx,right1.localPosition.y,right1.localPosition.z);
		right2.localPosition = new Vector3( maxx,right2.localPosition.y,right2.localPosition.z);
	}
	
	void DoPauseAction()
	{
		if(LevelInfo.Environments.playerShip.Ready && !wantToExitGame &&
			(LevelInfo.State.state == GameState.Play || LevelInfo.State.state == GameState.Paused) )
		{
			if(ppause) UnPauseGame();
			else PauseGame();
		}	
	}
	
	void OnPress(bool isDown)
	{
		if(isDown)
			DoPauseAction();
	}
	
	public void PauseGame()
	{
		ppause = true;
		LevelInfo.State.state = GameState.Paused;
	}
	
	public void UnPauseGame()
	{
		ppause = false;
	}
	
	void Update()
	{
		if(ppause && left1.localPosition.x<-1)
		{
			float delta = Mathf.Min(speed*0.0167f,-left1.localPosition.x);
			SetX(-left1.localPosition.x-delta);
			/*left1.Translate(delta,0f,0f);
			left2.Translate(delta,0f,0f);
			right1.Translate(-delta,0f,0f);
			right2.Translate(-delta,0f,0f);*/
		}
		if(!ppause && left1.localPosition.x>-maxx)
		{
			float delta = speed*0.0167f;
			SetX(-left1.localPosition.x+delta);
			/*left1.Translate(-delta,0f,0f);
			left2.Translate(-delta,0f,0f);
			right1.Translate(delta,0f,0f);
			right2.Translate(delta,0f,0f);	*/	
			if(left1.localPosition.x<=-maxx)
			{
				LevelInfo.State.state = GameState.Play;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Escape))
			DoPauseAction();
	}
	
	void SetX(float maxx)
	{
		left1.localPosition  = new Vector3(-maxx,left1.localPosition.y,left1.localPosition.z);
		left2.localPosition  = new Vector3(-maxx,left2.localPosition.y,left2.localPosition.z);
		right1.localPosition = new Vector3( maxx,right1.localPosition.y,right1.localPosition.z);
		right2.localPosition = new Vector3( maxx,right2.localPosition.y,right2.localPosition.z);		
	}
	
	public void SetPauseButtonsActive(bool active)
	{
		foreach(var button in buttons)
			button.gameObject.SetActive(active);
	}
	
	private bool wantToExitGame = false;
	public bool WantToExitGame{
		get{
			return wantToExitGame;
		}
		set{
			wantToExitGame = value;
			SetPauseButtonsActive(!value);
			popupQuitGame.SetActive(value);
		}
	}
	
}
