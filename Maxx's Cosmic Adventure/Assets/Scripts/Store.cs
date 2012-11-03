using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	
	private enum State { Nothing,Offence,Defence};
	
	public bool Active = false;
	
	private State state = State.Nothing;
	
	public Texture2D textureBackground;
	public Texture2D textureScreenShot;
	
	private float udh = 0.1f*Screen.height;
	private Vector2 screenpart = new Vector2(Screen.width*0.5f,Screen.height*0.5f);
	private Vector2 screen = new Vector2(Screen.width,Screen.height);
	
	private Vector2 scrollposition = Vector2.zero;
	private int wooi = -1;
	
	public void DrawStore()
	{	
		// Background
		GUI.DrawTexture(new Rect(0f,0f,Screen.width,udh),textureBackground);
		GUI.DrawTexture(new Rect(0f,0f,screenpart.x,Screen.height),textureBackground);
		GUI.DrawTexture(new Rect(0f,screen.y-udh,Screen.width,udh),textureBackground);
		
		// Screen Shot
		if(wooi==-1)
		{
			//GUI.DrawTexture(new Rect(screenpart.x,udh,screenpart.x,screen.y-2*udh),textureScreenShot);
		}
		
		// Unlikelium amount
		GUI.Box(new Rect(0,0,screenpart.x,udh),"Unlikelium : " + GameEnvironment.Unlikelium);
		
		// Currently Stored Powerups
		GUI.Box(new Rect(0,screen.y-udh,screen.x*0.25f,udh),"Currently Stored Powerups");
		
		// Graphics icon Stored Powerups
		GUI.Box(new Rect(screen.x*0.25f,screen.y-udh,screen.x*0.25f,udh),"Graphics icon Stored Powerups");
		
		// Return to Game
		if( GUI.Button(new Rect(screenpart.x,screen.y-udh,screen.x*0.25f,udh),"Return to Game") )
		{
			Time.timeScale = 1f;
			Active = false;
			wooi = -1;
		}
		
		// Main menu
		if( GUI.Button(new Rect(screen.x*0.75f,screen.y-udh,screen.x*0.25f,udh),"Main Menu") )
		{
			Time.timeScale = 1f;
			Application.LoadLevel("mainmenu");
			Active = false;
			wooi = -1;
		}	
		
		switch(state)
		{
		case State.Nothing:
			if( GUI.Button(new Rect(0,udh,screenpart.x,screenpart.y-udh),"Offence") )
				state = State.Offence;
			if( GUI.Button(new Rect(0,screenpart.y,screenpart.x,screenpart.y-udh),"Defence") )
				state = State.Defence;
			break;
		case State.Offence:
			if( GUI.Button(new Rect(screenpart.x,0,screenpart.x,udh),"Offence/Defence") )
			{
				state = State.Nothing;
				wooi = -1;
			}
			
			scrollposition = GUI.BeginScrollView(new Rect(0,udh,screenpart.x,screen.y-2*udh),
				scrollposition,new Rect(0,0,screen.x*0.48f,GameEnvironment.Offence.Length*udh),false,true);
	
			for(int i=0;i<GameEnvironment.Offence.Length;i++)
				if( GUI.Button(new Rect(0f,i*udh,screen.x*0.48f,udh),GameEnvironment.Offence[i] ) )
					wooi = i;
			GUI.EndScrollView();
			
			if( wooi != -1 )
			{
				GUI.Box(new Rect(screenpart.x,udh,screenpart.x,udh),"Description");
				GUI.Box(new Rect(screenpart.x,2*udh,screenpart.x,screen.y-5*udh),"Picture");
				if( GUI.Button( new Rect(screenpart.x,screen.y-2*udh,screenpart.x,udh),"Buy it now") ) {}
				GUI.Box( new Rect(screenpart.x,screen.y-3*udh,screenpart.x,udh),"Cost");
			}
			
			break;
		case State.Defence:
			if( GUI.Button(new Rect(screenpart.x,0,screenpart.x,udh),"Offence/Defence") )
			{
				state = State.Nothing;
				wooi = -1;
			}
			
			scrollposition = GUI.BeginScrollView(new Rect(0,udh,screenpart.x,screen.y-2*udh),
				scrollposition,new Rect(0,0,screen.x*0.48f,GameEnvironment.Defence.Length*udh),false,true);
	
			for(int i=0;i<GameEnvironment.Defence.Length;i++)
				if( GUI.Button(new Rect(0f,i*udh,screen.x*0.48f,udh),GameEnvironment.Defence[i] ) )
					wooi = i;
			GUI.EndScrollView();
			
			if( wooi != -1 )
			{
				GUI.Box(new Rect(screenpart.x,udh,screenpart.x,udh),"Description");
				GUI.Box(new Rect(screenpart.x,2*udh,screenpart.x,screen.y-5*udh),"Picture");
				if( GUI.Button( new Rect(screenpart.x,screen.y-2*udh,screenpart.x,udh),"Buy it now") ) {}
				GUI.Box( new Rect(screenpart.x,screen.y-3*udh,screenpart.x,udh),"Cost");
			}
			
			break;
		}
		
	}
	
	private bool maxxloaded = false;
	void OnGUI()
	{
		if( Active ) 
		{
			if( ! maxxloaded )
			{
				Application.LoadLevelAdditive("maxxrotate");
				maxxloaded = true;
			}
			DrawStore();
		}
		else
		{
			if( maxxloaded )
			{
				Destroy(GameObject.Find("maxxrotate"));
				maxxloaded = false;
			}	
		}
	}
}
