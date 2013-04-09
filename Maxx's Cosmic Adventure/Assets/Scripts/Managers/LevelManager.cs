using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	// Environments
	public float MaxSpaceY = 3f;
	
	// Player
	public float PlayerSpeed = 40f;
	public float PlayerUpDownSpeed = 8f;
	public float PlayerWaitForRise = 4f;
	public float SureShotDistance = 150f;
	
	// Aliens
	public float AlienShipAppearTime = 1f;
	
	public void UpdateOptions()
	{
		LevelInfo.Environments.FPS.SetActive(Options.Instance.showFPS);
	}
	
	public void UpdatePurchasedItems()
	{
		LevelInfo.Environments.fuelOverheat.UpTime = 0.5f-0.1f*Store.Instance.powerupBoostFuel.level;
		LevelInfo.Environments.score.Lives = Store.Instance.powerupToughGuy.level+1;
	}
	
	void Awake()
	{
		UpdateOptions();
		UpdatePurchasedItems();
	}
}
