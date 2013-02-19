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
			LevelInfo.Environments.infoMessage.ShowMessage("Sure Shot");
			break;
		case Gems.Shield:
			LevelInfo.Environments.score.AddLive();
			break;
		case Gems.Magnet:
			EnableAllUnlikeliumsMagnet();
			LevelInfo.Environments.infoMessage.ShowMessage("Magnet");
			break;
		case Gems.In3s:
			LevelInfo.Environments.missles.currentPowerup = Gems.In3s;
			LevelInfo.Environments.infoMessage.ShowMessage("In 3's");
			break;
		case Gems.FreezeWorld:
			LevelInfo.Environments.playerShip.StartFreezeWorld();
			LevelInfo.Environments.infoMessage.ShowMessage("Freeze Galaxy");
			break;
		case Gems.Pow:
			LevelInfo.Environments.missles.currentPowerup = Gems.Pow;
			LevelInfo.Environments.infoMessage.ShowMessage("Pow");
			break;
		case Gems.FireBall:
			LevelInfo.Environments.missles.currentPowerup = Gems.FireBall;
			LevelInfo.Environments.infoMessage.ShowMessage("FireBall");
			break;
		case Gems.LoveUnlikelium:
			LevelInfo.Environments.playerShip.StartLoveUnlikelium();
			LevelInfo.Environments.infoMessage.ShowMessage("Unlikelium List");
			break;
		}
		LevelInfo.Audio.PlayAudioGemPickUp(gemtype);
		Destroy(gem);
	}
	
	private void EnableAllUnlikeliumsMagnet()
	{
		GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
		foreach(GameObject g in gems )
		{
			if(g!=null && g.GetComponent<Gem>() != null)
				g.GetComponent<Gem>().ActivateMagnet();
		}
	}
	
	
	
	#endregion
}
