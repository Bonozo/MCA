using UnityEngine;
using System.Collections;

public class PlayerCollisionDetection : MonoBehaviour {
	
	#region Collision Detection
	
	void OnTriggerEnter(Collider col)
	{
		switch(col.gameObject.tag)
		{
		case "Gem":
			TakeGem(col.gameObject);
			break;
		case "Asteroid":
			
			break;
		}
	}
	
	/*void OnCollisionEnter(Collision col)
	{
		
	}*/
	
	#endregion
	
	#region Powerups
	
	private void TakeGem(GameObject gem)
	{
		Gems gemtype = gem.GetComponent<Gem>().gemType;
		switch( gemtype )
		{
		case Gems.Unlikelium:
			LevelInfo.Environments.playerShip.numberUnlikelium++;
			break;
		case Gems.SureShot:
			LevelInfo.Environments.playerShip.StartSureShot();
			break;
		case Gems.Shield:
			LevelInfo.Environments.score.AddLive();
			break;
		case Gems.Magnet:
			EnableAllUnlikeliumsMagnet();
			break;
		case Gems.Missle:
			LevelInfo.Environments.missles.missleCount++;
			break;
		}
		
		if( gemtype != Gems.Unlikelium )
			LevelInfo.Environments.infoMessage.ShowMessage(gemtype.ToString());
		LevelInfo.Audio.PlayAudioGemPickUp(gemtype);
		Destroy(gem);
	}
	
	private void EnableAllUnlikeliumsMagnet()
	{
		GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
		foreach(var g in gems )
		{
			g.SendMessage("ActivateMagnet");
		}
	}
	
	#endregion
}
