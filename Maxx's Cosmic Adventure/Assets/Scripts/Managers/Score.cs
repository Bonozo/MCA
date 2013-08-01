using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public readonly int MaxLives = 5;
	public UISprite[] guiShield;
	
	private int currentLives = 3;
	private Color colRed = new Color(1f,0.2f,0.2f,1f),colSt = new Color(1f,1f,1f,1f);
	
	void Start ()
	{
		Lives = 3;
		if( Store.Instance.powerupToughGuy.level>0 )
		{
			Store.Instance.powerupToughGuy.level--;
			Lives = 5;
		}
		
		UpdateActiveShields();
		UpdateCrashedShield();
	}
	
	void UpdateActiveShields()
	{
		for(int i=0;i<currentLives;i++)
		{
			guiShield[i].gameObject.SetActive(true);
			guiShield[i].color = colSt;
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
		
		LevelInfo.Environments.playerShip.LostLifeSmoke(currentLives);
		StartCoroutine(LostLiveThread(currentLives));
		if( currentLives == 0 )
		{
			UpdateCrashedShield();
			LevelInfo.State.StartDying();
		}
	}
	
	private IEnumerator LostLiveThread(int t)
	{
		guiShield[t].color = colRed;
		yield return new WaitForSeconds(0.5f);
		if(t>=currentLives) 
			guiShield[t].gameObject.SetActive(false);
	}
	
	#endregion
	
	#region Score
	
	[System.NonSerializedAttribute]
	public int asteroidsDestoyed=0;
	[System.NonSerializedAttribute]
	public int jeebiesDestoyed=0;
	[System.NonSerializedAttribute]
	public int unlikeliumsCollected=0;
	[System.NonSerializedAttribute]
	public int powerupsCollected=0;
	
	public int totalScore{
		get{
			return Mathf.FloorToInt( (10*asteroidsDestoyed+10*jeebiesDestoyed+
				unlikeliumsCollected+1)*LevelInfo.Environments.playerShip.DistanceTravelled );
		}
	}
	
	#endregion
}
