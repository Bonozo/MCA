using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviour {
	
	public Texture standartTexture;
	public Texture pressedTexture;
	
	virtual protected void Update()
	{
		foreach(Touch touch in Input.touches)
		{
			if( this.guiTexture.HitTest(touch.position ) )
			{
				if( touch.phase != TouchPhase.Ended )
					guiTexture.texture = pressedTexture;
				return;
			}
		}
		guiTexture.texture = standartTexture;
	}
}
