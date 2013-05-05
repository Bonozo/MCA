using UnityEngine;
using System.Collections;

public class PlayerCollisionDetection : MonoBehaviour {
	
	#region Collision Detection
	
	void OnTriggerEnter(Collider col)
	{
		switch(col.gameObject.tag)
		{
		case "Gem":
			if(LevelInfo.Environments.playerShip.Invincibility && col.gameObject.GetComponent<Gem>().gemType != Gems.Unlikelium)
				break;
			TakeGem(col.gameObject);
			break;
		case "Asteroid":
		case "Enemy":
		case "AlienBullet":
			Destroy(col.gameObject);
			if(LevelInfo.Environments.playerShip.Invincibility)
				break;
			#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
			if( Options.Instance.vibration )
				Handheld.Vibrate();
			#endif	
			LevelInfo.Environments.score.LostLive();
			if(LevelInfo.Environments.score.Lives > 0 )
				LevelInfo.Audio.MaxxLostLife();
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
		
		if(gemtype != Gems.Unlikelium)
		{
			LevelInfo.Audio.MaxxGetsPowerup();
		}
		
		switch( gemtype )
		{
		case Gems.Unlikelium:
			int val = gem.GetComponent<Gem>().unlikeliumValue;
			LevelInfo.Environments.playerShip.unlikeliums += val;
			if(val==5) LevelInfo.Environments.infoMessage.ShowMessage("Bronze");
			if(val==10) LevelInfo.Environments.infoMessage.ShowMessage("Silver");
			if(val==25) LevelInfo.Environments.infoMessage.ShowMessage("Gold");
			break;
		case Gems.SureShot:
			LevelInfo.Environments.playerShip.StartSureShot();
			LevelInfo.Environments.infoMessage.ShowMessage("Sure Shot");
			break;
		case Gems.Shield:
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
		}
		
		LevelInfo.Environments.score.score += LevelInfo.Settings.scorePowerup;
		
		LevelInfo.Audio.PlayAudioGemPickUp(gemtype);
		Destroy(gem);
	}
	
	#endregion
}
