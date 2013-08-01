using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	// Environments
	public float MaxSpaceY = 3f;
	
	[System.NonSerializedAttribute]
	public bool tutorialMode = false;
	
	// Player
	public float PlayerSpeed = 40f;
	public float PlayerUpDownSpeed = 0.2f;
	public float PlayerUpDownIgnore = 0.0f; // in meters (0-3)
	
	public float PlayerRotateSpeed = 0.2f;
	public float PlayerRotationMaxAngle = 50f;
	public float PlayerRotateIgnore = 0.0f; // in degrees (0-90)
	
	public float PlayerWaitForRise = 4f;
	public float SureShotDistance = 150f;
	
	// Aliens
	public float AlienShipAppearTime = 1f;
	
	public void UpdateOptions()
	{
		LevelInfo.Environments.FPS.SetActive(Options.Instance.showFPS);
	}
	
	public void InitShipParams()
	{
		// engine 0,1,2,3,4,5
		PlayerSpeed = 35f+5*Store.Instance.powerupShipEngine.level;
	}
	
	public void UpdatePurchasedItems()
	{
		LevelInfo.Environments.fuelOverheat.UpTime = 0.5f-0.08f*Store.Instance.powerupBoostFuel.level;
		LevelInfo.Environments.fireOverheat.UpTime = 0.3f-0.04f*Store.Instance.powerupFireHeating.level;
		//LevelInfo.Environments.score.Lives = Store.Instance.powerupToughGuy.level+1;
	}
	
	void Awake()
	{
		InitShipParams();
		
		UpdateOptions();
		UpdatePurchasedItems();
	}
}
