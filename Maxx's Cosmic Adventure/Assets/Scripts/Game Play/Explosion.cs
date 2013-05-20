using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	public float Lifetime = 5f;
	
	private GameState lastState = GameState.Play;
	private bool finished = false;
	
	void Start()
	{
		Destroy(this.gameObject,Lifetime);
	}
	
	void Update()
	{
		if(finished) return;
		
		if(LevelInfo.State.state == GameState.Play)
		{
			audio.volume = Options.Instance.volumeSFX;
			if(lastState != GameState.Play)
				audio.Play();
			else if(!audio.isPlaying) finished = true;
			
		}
		if(LevelInfo.State.state != GameState.Play && lastState == GameState.Play) audio.Pause();
		
		lastState = LevelInfo.State.state;
	}
	
}
