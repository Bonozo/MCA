using UnityEngine;
using System.Collections;

public class ButtonGoScene : ButtonBase {
	
	public string sceneName;
	
	void OnMouseUp()
	{
		Application.LoadLevel(sceneName);
	}
	
	protected override void Update()
	{
		foreach(Touch touch in Input.touches)
			if( guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
				Application.LoadLevel(sceneName);
		base.Update();
	}
}
