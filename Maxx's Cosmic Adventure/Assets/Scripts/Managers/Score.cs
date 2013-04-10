using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public Texture2D TextureLive,TextureNoLive;
	public int MaxLives = 5;
	
	private int currentLives = 3;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if(Time.deltaTime == 0f ) return;
		for(int i=1;i<=MaxLives;i++)
		{
			Texture2D tex = i<=currentLives ? TextureLive:TextureNoLive;
			GUI.DrawTexture(new Rect(25*(i-1),0,25,25),tex);
		}
	}
	
	#region Properties
	
	public int Lives {
		get {
			return currentLives; 
		}
		set{
			currentLives = value;
		}
	}
	public bool Lose { get { return currentLives==0; }}
	
	#endregion
	
	#region Messages
	
	public void AddLive() { currentLives = Mathf.Min(currentLives+1,MaxLives); }
	public void LostLive() 
	{
		currentLives = Mathf.Max(currentLives-1,0);
		if( currentLives == 0 )
			LevelInfo.State.state = GameState.Lose;
	}
	
	#endregion
}