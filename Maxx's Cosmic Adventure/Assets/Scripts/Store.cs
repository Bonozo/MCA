using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	
	#region Enums
	
	private enum State { Nothing,Offence,Defence};
	
	#endregion
	
	#region Public Field
	
	public GameObject storeMaxxShip;
	public Texture2D textureBackground;
	public Texture2D textureScreenShot;
	
	#endregion
	
	#region Private Field
	
	private bool _showStore = false;
	public bool ShowStore{
		get{
			return _showStore;
		}
		set{
			_showStore = value;
			storeMaxxShip.SetActive(_showStore);
		}
	}
	
	private State state = State.Nothing;
	
	private Vector2 scrollposition = Vector2.zero;
	private int wooi = -1;
	
	#endregion
	
	#region Properties
	
	public bool IsMainMenu { get { return Application.loadedLevelName=="mainmenu"; }}
	public bool IsPlayGame { get { return Application.loadedLevelName=="playgame"; }}
	
	#endregion
	
	#region Awake, Start, Update
	
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	
	#endregion
	
	#region Draw Store

	public void DrawStore()
	{	
		float udh = 0.1f*Screen.height;
		Vector2 screenpart = new Vector2(Screen.width*0.5f,Screen.height*0.5f);
		Vector2 screen = new Vector2(Screen.width,Screen.height);
		
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
		if(IsPlayGame && GUI.Button(new Rect(screenpart.x,screen.y-udh,screen.x*0.25f,udh),"Return to Game") )
		{
			ShowStore = false;
			wooi = -1;
			LevelInfo.State.state = GameState.Paused;
		}
		
		// Main menu
		if( GUI.Button(new Rect(screen.x*0.75f,screen.y-udh,screen.x*0.25f,udh),"Main Menu") )
		{
			Time.timeScale = 1f;
			ShowStore = false;
			wooi = -1;
			if(IsMainMenu)
				MainMenu.Instance.State = MainMenu.MenuState.Title;
			if(IsPlayGame)
				Application.LoadLevel("mainmenu");
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

	void OnGUI()
	{
		if( ShowStore ) 
			DrawStore();
	}
	
	#endregion
	
	#region  Safe Store
	
	//Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile Store _staticInstance;	
    public static Store Instance {
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(Store)) as Store;
                    if (_staticInstance == null) {
                       Debug.LogError("The Store instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion
}
