using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public readonly int MaxLives = 5;
	public UISprite[] guiShield;
	
	private int currentLives = 3;
	
	[System.NonSerializedAttribute]
	public int score = 0;
	
	void Start ()
	{
		Lives = Store.Instance.powerupToughGuy.level+1;
		
		UpdateActiveShields();
		UpdateCrashedShield();
	}
	
	void UpdateActiveShields()
	{
		for(int i=0;i<currentLives;i++)
		{
			guiShield[i].gameObject.SetActive(true);
			guiShield[i].spriteName = "shield";
		}
	}
	
	void UpdateCrashedShield()
	{
		for(int i=currentLives;i<MaxLives;i++)
			guiShield[i].gameObject.SetActive(false);		
	}
	
	void Update()
	{
		if(LevelInfo.State.state != GameState.Play) return;
		UpdateActiveShields();
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
	
	public void AddLive()
	{
		currentLives = Mathf.Min(currentLives+1,MaxLives); 
	}
	public void LostLive() 
	{
		currentLives = Mathf.Max(currentLives-1,0);
		
		if( currentLives == 0 )
		{
			UpdateCrashedShield();
			LevelInfo.State.state = GameState.Lose;
		}
		else
		{
			StartCoroutine(LostLiveThread(currentLives));
		}
	}
	
	private IEnumerator LostLiveThread(int t)
	{
		guiShield[t].spriteName = "shieldcrash";
		yield return new WaitForSeconds(0.5f);
		if(t>=currentLives) 
			guiShield[t].gameObject.SetActive(false);
	}
	
	#endregion
}
