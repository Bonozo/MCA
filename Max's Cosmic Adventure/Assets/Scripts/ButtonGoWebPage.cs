using UnityEngine;
using System.Collections;

public class ButtonGoWebPage : ButtonBase {
	
	public string ulr;
	
	void OnMouseUp()
	{
		Application.OpenURL(ulr);
	}
	
	protected override void Update()
	{
		foreach(Touch touch in Input.touches)
			if( guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
				Application.OpenURL(ulr);
		base.Update();
	}
}
