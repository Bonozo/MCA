using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
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
}
