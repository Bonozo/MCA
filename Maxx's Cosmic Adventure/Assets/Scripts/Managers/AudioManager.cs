using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	#region Main
	
	public AudioSource audioSourceBackground;
	public AudioSource audioSourcePlayerShip;
	public AudioSource audioSourceJeebles;
	public AudioSource audioSourceUnlikeliums;
	
	public void StopEffects()
	{
		audioSourcePlayerShip.Stop();
		audioSourceJeebles.Stop();
		audioSourceUnlikeliums.Stop();
	}
	
	public void StopAll()
	{
		audioSourceBackground.Stop();
		StopEffects();
	}
	
	// Use this for initialization
	void Start () 
	{
		audioSourceBackground.volume = Options.hSlideBackgroundVolume;
		audioSourcePlayerShip.volume = Options.hSlideEffectsVolume;
		audioSourceJeebles.volume = Options.hSlideEffectsVolume;
		audioSourceUnlikeliums.volume = Options.hSlideEffectsVolume;
	}
	
	private float pitchAudioUnlikeliumDistance = 0.0f;
	
	void Update()
	{
		if( LevelInfo.Environments.playerShip.DistanceTravelled >= pitchAudioUnlikeliumDistance )
			audioSourceUnlikeliums.pitch = 1f;
	}
	
	#endregion
	
	
	#region Player
	public AudioClip audioGemUnlikeliumPickUp;
	public AudioClip audioWeaponPickUp;
	public AudioClip audioWeaponExpire;
	
	public void PlayAudioGemPickUp(Gems gem)
	{
		if( gem == Gems.Unlikelium )
		{
			audioSourceUnlikeliums.PlayOneShot(audioGemUnlikeliumPickUp);
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
	
	public AudioClip audioGotEm;
	public void PlayAudioGotEm()
	{
		audioSourceJeebles.PlayOneShot(audioGotEm);
	}
	
	public AudioClip audioAsteroidExplode;
	public void PlayAudioAsteroidExplode()
	{
		audioSourceJeebles.PlayOneShot(audioAsteroidExplode);
	}
	
	#endregion
	
	#region Powerups
	
	public AudioClip clipPowerupPOW;
	
	#endregion
	
}
