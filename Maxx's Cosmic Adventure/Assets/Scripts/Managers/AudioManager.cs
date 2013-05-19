using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	#region Main
	
	public AudioSource audioSourceBackground;
	public AudioSource audioSourcePlayerShip;
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
	
	#endregion
	
	
	#region Player
	public AudioClip clipGetUnlikelium;
	public AudioClip clipFire;
	public AudioClip clipAutoFire;
	
	public AudioClip audioWeaponPickUp;
	public AudioClip audioWeaponExpire;
	
	public void PlayAudioGemPickUp(Gems gem)
	{
		if( gem == Gems.Unlikelium )
		{
			audioSourceUnlikeliums.PlayOneShot(clipGetUnlikelium);
			audioSourceUnlikeliums.pitch += 0.025f;
			pitchAudioUnlikeliumDistance = LevelInfo.Environments.playerShip.DistanceTravelled+35;
		}
	}
	
	public void PlayAudioWeaponExpire()
	{
		audioSourcePlayerShip.PlayOneShot(audioWeaponExpire);
	}
	
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
	
	#region Maxx!!!!!
	
	/* player voice-overs with events
	 * 1. All time phrases 
	 * 2. Maxx's ship crashing
	 * 
	 * 3. Evading jeebies
 	 * 4. Near miss with obstacle 
 	 * 
	 * 5. Lots of Jeebies
	 * 6. Lots of Asteroids
	 * 
	 * 7. Maxx Chases Jeebies
	 * 8. Maxx Destroys Asteroids
	 * 
	 * 9. Powerups use
	 * */
	
	public AudioClip[] explodedJeebie;	
	public void MaxxExplodedJeebie()
	{
		MaxxPlay(explodedJeebie[Random.Range(0,explodedJeebie.Length)],0.3f,0.7f);
	}
	
	public AudioClip[] explodedAsteroid;
	public void MaxxExplodedAsteroid()
	{
		MaxxPlay(explodedAsteroid[Random.Range(0,explodedAsteroid.Length)],0.3f,0.7f);
	}
	
	public AudioClip[] maxxAsteroidBelt;
	public void MaxxAsteroidBelt()
	{
		MaxxPlay(maxxAsteroidBelt[Random.Range(0,maxxAsteroidBelt.Length)],0.0f,0.3f);
	}
	
	public AudioClip[] maxxJeebieAttack;
	public void MaxxJeebieAttack()
	{
		MaxxPlay(maxxJeebieAttack[Random.Range(0,maxxJeebieAttack.Length)],1.1f,0.5f);
	}	
	
	public AudioClip[] maxxGetsPowerup;
	public void MaxxGetsPowerup()
	{
		MaxxPlay(maxxGetsPowerup[Random.Range(0,maxxGetsPowerup.Length)],1f,0.5f);
	}	
	
	public AudioClip[] maxxGetsHit;
	public void MaxxLostLife()
	{
		MaxxPlay(maxxGetsHit[Random.Range(0,maxxGetsHit.Length)],0f,0.8f);
	}	
	
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
