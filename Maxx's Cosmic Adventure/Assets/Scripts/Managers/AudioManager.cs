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
		PlayRandomGamePlay();
		VolumeSetup();
	}
	
	void Update()
	{
		VolumeSetup();
		if( LevelInfo.Environments.playerShip.DistanceTravelled >= pitchAudioUnlikeliumDistance )
			audioSourceUnlikeliums.pitch = 1f;
	}
	
	#endregion
	
	#region Game
	
	public AudioClip[] gameMusic;
	public void PlayRandomGamePlay()
	{
		audioSourceBackground.clip = gameMusic[Random.Range(0,gameMusic.Length)];
		audioSourceBackground.Play();
	}
	
	public AudioClip clipGameOver;
	
	#endregion
	
	
	#region Player
	public AudioClip clipGetUnlikelium;
	public AudioClip clipGetPowerup;
	public AudioClip clipGetIntergalactic;
	public AudioClip clipGetToughGuy;
	
	public AudioClip clipFire;
	public AudioClip clipAutoFire;
	public AudioClip clipLightenUpFire;
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
	
	/* player voice-overs with events
	 * 1. All time phrases 
	 * 2. Maxx's ship crashing
	 * 
	 * 3. Maxx Chases Jeebies
	 * 4. Maxx Destroys Asteroids
	 * 
	 * 5. Lots of Jeebies
	 * 6. Lots of Asteroids
	 * 
	 * 7. Evading jeebies
 	 * 8. Near miss with obstacle 
	 * 
	 * 9. Powerups use
	 * */
	
	// All time phrases 
	public AudioClip[] voicoverAllTime;
	public void PlayVoiceOverAllTime()
	{
		MaxxPlay(voicoverAllTime[Random.Range(0,voicoverAllTime.Length)],0.0f,1f);
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
		MaxxPlay(voicoverJeebieDestroyed[Random.Range(0,voicoverJeebieDestroyed.Length)],0.3f,0.7f);
	}	
	
	// Asteroid destroyed
	public AudioClip[] voicoverAsteroidDestroyed;	
	public void PlayVoiceOverAsteroidDestroyed()
	{
		MaxxPlay(voicoverAsteroidDestroyed[Random.Range(0,voicoverAsteroidDestroyed.Length)],0.3f,0.25f);
	}	
	
	// Lots Of Jeebies (not determined)
	public AudioClip[] voicoverLotsOfJeebies;	
	public void PlayVoiceOverLotsOfJeebies()
	{
		MaxxPlay(voicoverLotsOfJeebies[Random.Range(0,voicoverLotsOfJeebies.Length)],0,1);
	}		
	
	// Lots Of Asteroids (not determined)
	public AudioClip[] voicoverLotsOfAsteroids;	
	public void PlayVoiceOverLotsOfAsteroids()
	{
		MaxxPlay(voicoverLotsOfAsteroids[Random.Range(0,voicoverLotsOfAsteroids.Length)],0,1);
	}	
	
	// Waiting For A Jeebie (not determined)
	public AudioClip[] voicoverWaitingForAJeebie;	
	public void PlayVoiceOverWaitingForAJeebie()
	{
		MaxxPlay(voicoverWaitingForAJeebie[Random.Range(0,voicoverWaitingForAJeebie.Length)],0,1);
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
	
	// Implementation
	public void MaxxPlay(AudioClip clip,float delay,float probablity)
	{
		if( Random.Range(0f,1f)<=probablity )
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
