using UnityEngine;
using System.Collections;

public class PlayerCollisionDetection : MonoBehaviour {
	
	#region Collision Detection
	
	void OnTriggerEnter(Collider col)
	{
		switch(col.gameObject.tag)
		{
		case "Gem":
			if(LevelInfo.Environments.playerShip.Intergalactic && col.gameObject.GetComponent<Gem>().gemType != Gems.Unlikelium)
				break;
			TakeGem(col.gameObject);
			break;
		case "Asteroid":
		case "Enemy":
		case "AlienBullet":
			if(col.gameObject.tag == "AlienBullet")
				Destroy(col.gameObject);
			else
				col.gameObject.SendMessage("Explode");
			if(LevelInfo.Environments.playerShip.Invincibility)
				break;
			#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
			if( Options.Instance.vibration )
				Handheld.Vibrate();
			#endif	
			LevelInfo.Environments.score.LostLive();
			if(LevelInfo.Environments.score.Lives > 0 )
				LevelInfo.Audio.PlayVoiceOverShipCrash();
			break;
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		Debug.Log("Collision " + col.gameObject.name);
	}
	
	#endregion
	
	#region Powerups
	
	private void TakeGem(GameObject gem)
	{
		Gems gemtype = gem.GetComponent<Gem>().gemType;
		LevelInfo.Environments.tutorials.TakenGem(gemtype);
		
		switch( gemtype )
		{
		case Gems.Unlikelium:
			int val = gem.GetComponent<Gem>().unlikeliumValue;
			LevelInfo.Environments.playerShip.unlikeliums += val;
			
			// reportings
			LevelInfo.Environments.score.unlikeliumsCollected += val;
			Stats.Instance.ReportCollectedUnlikelium(val);
			
			if(val==5) LevelInfo.Environments.infoMessage.ShowMessage("Bronze");
			if(val==10) LevelInfo.Environments.infoMessage.ShowMessage("Silver");
			if(val==25) LevelInfo.Environments.infoMessage.ShowMessage("Gold");
			break;
		case Gems.SureShot:
			LevelInfo.Environments.playerShip.StartSureShot();
			LevelInfo.Environments.infoMessage.ShowMessage("Sure Shot");
			break;
		case Gems.Magnet:
			LevelInfo.Environments.playerShip.StartMagned();
			LevelInfo.Environments.infoMessage.ShowMessage("Lay It On Me");
			break;
		case Gems.In3s:
			LevelInfo.Environments.mButton.currentPowerup = Gems.In3s;
			LevelInfo.Environments.infoMessage.ShowMessage("Triple Trouble");
			break;
		case Gems.FreezeWorld:
			LevelInfo.Environments.playerShip.StartFreezeWorld();
			LevelInfo.Audio.PlayVoiceGetHoldItNowHitIt();
			LevelInfo.Environments.infoMessage.ShowMessage("Hold It Now, Hit It");
			break;
		case Gems.Pow:
			LevelInfo.Environments.mButton.currentPowerup = Gems.Pow;
			LevelInfo.Environments.infoMessage.ShowMessage("Pow");
			break;
		case Gems.FireBall:
			LevelInfo.Environments.mButton.currentPowerup = Gems.FireBall;
			LevelInfo.Environments.infoMessage.ShowMessage("Lighten Up");
			break;
		case Gems.LoveUnlikelium:
			LevelInfo.Environments.playerShip.StartLoveUnlikelium();
			LevelInfo.Environments.infoMessage.ShowMessage("Shazam!");
			break;
		case Gems.Intergalactic:
			LevelInfo.Environments.playerShip.StartIntergalactic();
			LevelInfo.Environments.infoMessage.ShowMessage("Intergalactic");
			break;
		case Gems.ToughGuy:
			LevelInfo.Environments.score.AddLive();
			LevelInfo.Environments.infoMessage.ShowMessage("Tough Guy");
			break;
		case Gems.BeastieBoost:
			LevelInfo.Environments.playerShip.StartBeastieBoost();
			LevelInfo.Environments.infoMessage.ShowMessage("Beastie Boost!");
			break;
		case Gems.Lazer:
			LevelInfo.Environments.mButton.currentPowerup = Gems.Lazer;
			LevelInfo.Environments.infoMessage.ShowMessage("Lazer");
			break;
		default:
			Debug.Log("TakeGem: " + gemtype.ToString() + " is not implemented");
			break;
		}
		
		if( gemtype != Gems.Unlikelium)
		{
			// reoprtings
			LevelInfo.Environments.score.powerupsCollected++;
			Stats.Instance.ReportCollectedPowerup(gemtype);
		}
		
		LevelInfo.Audio.PlayAudioGemPickUp(gemtype);
		Destroy(gem);
	}
	
	#endregion
}
