using UnityEngine;
using System.Collections;

public class ButtonStore : ButtonBase {
	
	Store store;
	
	void Start()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
	}
	
	void OnMouseUp()
	{
		store.Active = true;
	}
	
	protected override void Update()
	{
		foreach(Touch touch in Input.touches)
			if( guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
				store.Active = true;
		base.Update();
	}
}
