using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	#region Main
	
	public AudioSource audioSourceBackground;
	public AudioSource audioSourcePlayerShip;
	public AudioSource audioSourcePlayerShipEngine;
	public AudioSource audioSourceJeebles;
	public AudioSource audioSourceUnlikeliums;
	public AudioSource Maxx;
	
	public void StopMusic()
	{
		audioSourceBackground.Stop();
	}
	
	public void StopEffects()
	{
		audioSourcePlayerShip.Stop();
		audioSourcePlayerShipEngine.Stop();
		audioSourceJeebles.Stop();
		audioSourceUnlikeliums.Stop();
		Maxx.Stop();
	}
	
	public void StopAll()
	{
		StopMusic();
		StopEffects();
	}
	
	public void PauseMusic()
	{
		audioSourceBackground.Pause();
		Maxx.Pause();
	}
	
	public void ResumeMusic()
	{
		audioSourceBackground.Play();
		Maxx.Play();
	}
	
	private float pitchAudioUnlikeliumDistance = 0.0f;
	
	void VolumeSetup()
	{
		audioSourceBackground.volume = Options.Instance.volumeMusic;
		audioSourcePlayerShip.volume = Options.Instance.volumeSFX;
		audioSourcePlayerShipEngine.volume = Options.Instance.volumeSFX;
		audioSourceJeebles.volume = Options.Instance.volumeSFX;
		audioSourceUnlikeliums.volume = Options.Instance.volumeSFX;
		Maxx.volume = Options.Instance.volumeSFX;
	}
	
	void Start()
	{
		PlayRandomPossibleGamePlay();
		VolumeSetup();
	}
	
	void Update()
	{
		VolumeSetup();
		
		if(LevelInfo.State.state != GameState.Play) return;
		
		UpdateVoiceOvers();
		
		// Unlikelium audio pitch
		if( LevelInfo.Environments.playerShip.DistanceTravelled >= pitchAudioUnlikeliumDistance )
			audioSourceUnlikeliums.pitch = 1f;
	}
	
	#endregion
	
	#region Game
	
	public AudioClip[] gameMusic;
	public void PlayRandomPossibleGamePlay()
	{
		// local protocol xprefabs
		int allow = 0;
		for(int i=1;i<=6;i++)
		{
			if( PlayerPrefs.GetInt("options_music"+i)==1 )
				allow++;
		}
		if(allow==0)
		{
			Debug.LogError("MCA local: 0 enabled game play musics.");
			return;
		}
		int selected = Random.Range(0,allow);
		for(int i=1;i<=6;i++)
			if(PlayerPrefs.GetInt("options_music"+i)==1 && selected--==0)
			{
				audioSourceBackground.clip = gameMusic[i-1];
				break;
			}
		if(audioSourceBackground.clip == null)
			Debug.LogError("MCA local: No clip choosen for game play music.");
		audioSourceBackground.Play();
	}
	
	public AudioClip clipGameOver;
	
	#endregion
	
	
	#region Player
	public AudioClip clipPlayerCollision;
	public AudioClip clipMission;
	public AudioClip clipCompleteMission;
	
	public AudioClip clipGetUnlikelium;
	public AudioClip clipGetPowerup;
	public AudioClip clipGetIntergalactic;
	public AudioClip clipGetToughGuy;
	
	public AudioClip clipFire;
	public AudioClip clipAutoFire;
	public AudioClip clipLightenUpFire;
	public AudioClip clipLazerFire;
	public AudioClip clipTripleTroubleFire;
	
	public void PlayAudioGemPickUp(Gems gem)
	{
		switch(gem)
		{
		case Gems.Unlikelium:
			audioSourceUnlikeliums.PlayOneShot(clipGetUnlikelium);
			audioSourceUnlikeliums.pitch += 0.025f;
			pitchAudioUnlikeliumDistance = LevelInfo.Environments.playerShip.DistanceTravelled+35;
			break;
		case Gems.Intergalactic:
			audioSourcePlayerShip.PlayOneShot(clipGetIntergalactic);
			break;
		case Gems.ToughGuy:
			audioSourcePlayerShip.PlayOneShot(clipGetToughGuy);
			break;
		default:
			audioSourcePlayerShip.PlayOneShot(clipGetPowerup);
			break;
		}
	}
	
	// Engine
	public AudioClip clipPlayerShipEngineIdle;
	public AudioClip clipPlayerShipEngineBoost;
	
	#endregion
	
	
	#region Enemies
	
	public AudioClip audioAsteroidExplode;
	public void PlayAudioAsteroidExplode()
	{
		audioSourceJeebles.PlayOneShot(audioAsteroidExplode);
	}
	
	#endregion
	
	#region Powerups
	
	public AudioClip clipPowerupPOW;
	
	#endregion
	
	#region Maxx Voice Overs
	
	// For special events
	void UpdateVoiceOvers()
	{
		if(Maxx.clip!=null && !Maxx.isPlaying) Maxx.clip = null;
	}
	
	// Maxx's ship crashing
	public AudioClip[] voicoverShipCrash;	
	public void PlayVoiceOverShipCrash()
	{
		MaxxPlay(voicoverShipCrash[Random.Range(0,voicoverShipCrash.Length)],0.0f,1f);
	}	
	
	// Jeebie destroyed
	public AudioClip[] voicoverJeebieDestroyed;	
	public void PlayVoiceOverJeebieDestroyed()
	{
		MaxxPlay(voicoverJeebieDestroyed[Random.Range(0,voicoverJeebieDestroyed.Length)],0.3f,1f);
	}	
	
	// Asteroid destroyed
	public AudioClip[] voicoverAsteroidDestroyed;	
	public void PlayVoiceOverAsteroidDestroyed()
	{
		MaxxPlay(voicoverAsteroidDestroyed[Random.Range(0,voicoverAsteroidDestroyed.Length)],0.3f,0.25f);
	}	
	
	// Lots Of Jeebies
	public AudioClip[] voicoverLotsOfJeebies;
	public void PlayVoiceOverLotsOfJeebies()
	{
		if( AlienShip.GlobalCount >= 5 && Random.Range(0,6)==1)
		{
			GameObject[] g = GameObject.FindGameObjectsWithTag("Enemy");
			int front = 0;
			foreach(var gg in g)
				if( gg.GetComponent<AlienShip>().IsFrontOfCamera )
					front++;
			if(front>=4)
				MaxxPlay(voicoverLotsOfJeebies[Random.Range(0,voicoverLotsOfJeebies.Length)],Random.Range(0f,1f),1);
		}
	}		
	
	// Lots Of Asteroids
	public AudioClip[] voicoverLotsOfAsteroids;	
	public void PlayVoiceOverLotsOfAsteroids()
	{
		if( Asteroid.GlobalCount >= 4 && Random.Range(0,3)==1)
		{
			GameObject[] g = GameObject.FindGameObjectsWithTag("Asteroid");
			int front = 0;
			foreach(var gg in g)
				if( gg.GetComponent<Asteroid>().IsFrontOfCamera )
					front++;
			if(front>=4)
				MaxxPlay(voicoverLotsOfAsteroids[Random.Range(0,voicoverLotsOfAsteroids.Length)],Random.Range(0f,1f),1);
		}	
	}	
	
	// Waiting For A Jeebie
	public AudioClip[] voicoverWaitingForAJeebie;	
	public void PlayVoiceOverWaitingForAJeebie()
	{
		MaxxPlay(voicoverWaitingForAJeebie[Random.Range(0,voicoverWaitingForAJeebie.Length)],Random.Range(0f,1f),0.15f);
	}
	
	// Near miss with obstacle
	public AudioClip[] voicoverNearMissWithObstacle;
	public void PlayVoiceOverNearMissWithObstacle()
	{
		MaxxPlay(voicoverNearMissWithObstacle[Random.Range(0,voicoverNearMissWithObstacle.Length)],0f,0.5f);
	}	
	
	// Use POW
	public AudioClip[] voicoverUsePOW;	
	public void PlayVoiceOverUsePOW()
	{
		MaxxPlay(voicoverUsePOW[Random.Range(0,voicoverUsePOW.Length)],0.1f,1);
	}	
	
	// Get Hold It Now, Hit It
	public AudioClip[] voicoverGetHoldItNowHitIt;	
	public void PlayVoiceGetHoldItNowHitIt()
	{
		MaxxPlay(voicoverGetHoldItNowHitIt[Random.Range(0,voicoverGetHoldItNowHitIt.Length)],0.3f,1);
	}
	
	// Use Triple Trouble
	public AudioClip[] voicoverUseTripleTrouble;	
	public void PlayVoiceUseTripleTrouble()
	{
		MaxxPlay(voicoverUseTripleTrouble[Random.Range(0,voicoverUseTripleTrouble.Length)],0.1f,0.5f);
	}	
	
	// Implementation
	public void MaxxPlay(AudioClip clip,float delay,float probablity)
	{
		if( LevelInfo.State.state == GameState.Play && Random.Range(0f,1f)<=probablity )
			StartCoroutine(MaxxPlayThread(clip,delay));
	}	
	private IEnumerator MaxxPlayThread(AudioClip clip,float delay)
	{
		yield return new WaitForSeconds(delay);
		if( !Maxx.isPlaying )
		{
			Maxx.clip = clip;
			Maxx.Play();
		}
	}
	
	#endregion
	
}
