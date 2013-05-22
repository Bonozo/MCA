using UnityEngine;
using System.Collections;

public class OptionsMusicPlay : MonoBehaviour {
	
	public AudioClip clip;
	public UISprite spritePlay;
	
	private bool _isPlaying = false;
	public bool isPlaying{
		get{
			return _isPlaying;
		}
		set{
			_isPlaying = value;
			spritePlay.spriteName = _isPlaying?"stop":"play button";
		}
	}
	
	void OnClick()
	{
		if(isPlaying)
			Options.Instance.PlayMainLoop();
		else
			Options.Instance.PlayClip(this);
	}
}
