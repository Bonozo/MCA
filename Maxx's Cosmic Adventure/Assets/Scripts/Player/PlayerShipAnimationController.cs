using UnityEngine;
using System.Collections;

public class PlayerShipAnimationController : MonoBehaviour {
	
	public Transform boneBase;
	public Transform boneTurret;
	
	void Awake()
	{
		GetComponent<Animation>()["turette open"].layer = 2;
		GetComponent<Animation>()["turette close"].layer = 2;
		
		GetComponent<Animation>()["turette open"].AddMixingTransform(boneTurret,false);
		GetComponent<Animation>()["turette close"].AddMixingTransform(boneTurret,false);
		
		GetComponent<Animation>()["barrelrollright"].layer = 1;
		GetComponent<Animation>()["barrelrollleft"].layer = 1;
		GetComponent<Animation>()["barrelrollup"].layer = 1;
		
		GetComponent<Animation>()["barrelrollright"].AddMixingTransform(boneBase,false);
		GetComponent<Animation>()["barrelrollleft"].AddMixingTransform(boneBase,false);
		GetComponent<Animation>()["barrelrollup"].AddMixingTransform(boneBase,false);
	}
	
	void OnEnable()
	{
		FingerGestures.OnFingerSwipe += HandleFingerGesturesOnFingerSwipe;
	}
	
	void OnDisable()
	{
		FingerGestures.OnFingerSwipe -= HandleFingerGesturesOnFingerSwipe;
	}
	
	void HandleFingerGesturesOnFingerSwipe (int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if( velocity > 250f && LevelInfo.Environments.playerShip.Ready && !LevelInfo.Environments.playerShip.Intergalactic && !IsPlayingAnyRollAnimation)
		{
			switch(direction)
			{
			case FingerGestures.SwipeDirection.Left: GetComponent<Animation>().Play("barrelrollleft");
				break;
			case FingerGestures.SwipeDirection.Right: GetComponent<Animation>().Play("barrelrollright");
				break;
			case FingerGestures.SwipeDirection.Up: GetComponent<Animation>().Play("barrelrollup");
				break;
			}
		}		
	}
	
	public bool IsPlayingAnyRollAnimation{ get{
		return GetComponent<Animation>().IsPlaying("barrelrollup")||GetComponent<Animation>().IsPlaying("barrelrollleft")
				||GetComponent<Animation>().IsPlaying("barrelrollright");
		}}
	
	bool turreteopened = false;
	public void OpenTurette()
	{
		GetComponent<Animation>().Play("turette open");
		turreteopened = true;
	}
	
	public void CloseTurette()
	{
		if(turreteopened)
		{
			GetComponent<Animation>().Play("turette close");
			turreteopened = false;
		}
	}
}