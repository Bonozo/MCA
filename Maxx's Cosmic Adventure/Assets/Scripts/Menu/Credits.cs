using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	
	public GameObject text;
	public float lenght = 300f;
	public float speed = 100f;
	
	private float currentHeight = 0f;

	void Update () 
	{
		if( Input.touchCount == 0 && !Input.GetMouseButton(0))
			currentHeight += Time.deltaTime*speed;
		
		if(currentHeight>lenght)
		{
			MainMenu.Instance.State = MainMenu.MenuState.Title;
		}
		
		var v = text.transform.localPosition;
		v.y = currentHeight;
		text.transform.localPosition = v;
	}
	
	void OnEnable()
	{
		currentHeight = 0;
		FingerGestures.OnDragMove += HandleFingerGesturesOnDragMove;
	}
	
	void OnDisable()
	{
		FingerGestures.OnDragMove -= HandleFingerGesturesOnDragMove;
	}

	void HandleFingerGesturesOnDragMove (Vector2 fingerPos, Vector2 delta)
	{
		currentHeight += delta.y;
		currentHeight = Mathf.Clamp(currentHeight,0f,lenght);
	}
}
