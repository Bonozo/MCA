using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public UIFilledSprite bar;
	public UIFilledSprite bar2;
	public UISprite background2;
	
	private float shield = 1f;
	private float shieldshow = 1f;
	private float statusupdatespeed = 1f;
	private bool lost = false;
	
	public float Shield{ get{ return shieldshow; }}
	
	void Awake()
	{
		if( Store.Instance.powerupToughGuy.level>0 )
		{
			Store.Instance.powerupToughGuy.level--;
			shield = 2f; // double shield
		}
		
		shieldshow = shield;
		UpdateUI();
	}
	
	void Update()
	{	
		#if UNITY_EDITOR
		if( Input.GetKeyUp(KeyCode.KeypadPlus) )
			LevelInfo.Environments.score.AddShield(0.2f);
		if( Input.GetKeyUp(KeyCode.KeypadMinus) )
			LevelInfo.Environments.score.LoseShield(0.2f);
		#endif
		
		if(LevelInfo.State.state != GameState.Play) return;
		
		if( shieldshow < shield ) shieldshow = Mathf.Min(shieldshow+Time.deltaTime*statusupdatespeed,shield);
		if( shieldshow > shield ) shieldshow = Mathf.Max(shieldshow-Time.deltaTime*statusupdatespeed,shield);
		UpdateUI();
		
		/*if(shieldshow>=0.5f) bar.color = Color.green;
		else if( shieldshow>0.25f) bar.color = new Color(0.257f,0.659f,1f,1f);
		else bar.color = Color.red;*/
		bar.color = hullColorByLevel[Store.Instance.powerupShipHull.level];
		
		if(!lost&&Lose)
		{
			LevelInfo.State.StartDying();
			lost = true;
		}
	}
	
	private Color[] hullColorByLevel = new Color[6]
	{
		new Color( 133f/255f,211f/255f,255f/255f,1f), // Light Blue
		new Color(  0f/255f,146f/255f,255f/255f,1f), // Dark Blue
		new Color(255f/255f,255f/255f,255f/255f,1f), // White
		new Color(255f/255f,  0f/255f,  0f/255f,1f), // Red
		new Color(211f/255f,232f/255f,250f/255f,1f), // Silver
		new Color(120f/255f,120f/255f,120f/255f,1f)  // Black
	};
	
	void UpdateUI()
	{
		bar.fillAmount = Mathf.Clamp01(shieldshow);
		bar2.fillAmount = Mathf.Clamp01(shieldshow-1);
		NGUITools.SetActive(background2.gameObject,shieldshow>1f);
	}
	
	#region Properties
	
	public bool Lose { get { return Shield<=0.001f; }}
	
	#endregion
	
	#region Messages
	
	public void AddShield(float amount)
	{
		shield = Mathf.Clamp(shield + amount,0f,2f);
		lost = false;
	}
	
	public void LoseShield(float amount)
	{
		float hullfactor = 1f-0.1f*Store.Instance.powerupShipHull.level;
		amount *= hullfactor;
		shield = Mathf.Clamp(shield - amount,0f,2f);
		LevelInfo.Environments.playerShip.LostLifeSmoke(Mathf.CeilToInt(shield*5));			
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
