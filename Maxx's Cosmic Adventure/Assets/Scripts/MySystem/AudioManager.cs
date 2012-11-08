using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	#region Main
	
	public AudioSource audioSourceBackground;
	public AudioSource audioSourcePlayerShip;
	public AudioSource audioSourceJeebles;
	
	public void StopEffects()
	{
		audioSourcePlayerShip.Stop();
		audioSourceJeebles.Stop();
	}
	
	public void StopAll()
	{
		audioSourceBackground.Stop();
		StopEffects();
	}
	
	// Use this for initialization
	void Start () {
		audioSourceBackground.volume = Option.hSlideBackgroundVolume;
		audioSourcePlayerShip.volume = Option.hSlideEffectsVolume;
		audioSourceJeebles.volume = Option.hSlideEffectsVolume;
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
			audioSourcePlayerShip.PlayOneShot(audioGemUnlikeliumPickUp);
		}
	}
	
	public void PlayAudioWeaponExpire()
	{
		audioSourcePlayerShip.PlayOneShot(audioWeaponExpire);
	}
	
	#endregion
	
	
	
}
