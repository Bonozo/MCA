using UnityEngine;
using System.Collections;

public class PlayerShipAnimationController : MonoBehaviour {
	
	public Transform boneBase;
	public Transform boneTurret;
	
	void Awake()
	{
		animation["turette open"].layer = 2;
		animation["turette close"].layer = 2;
		
		animation["turette open"].AddMixingTransform(boneTurret,false);
		animation["turette close"].AddMixingTransform(boneTurret,false);
		
		animation["barrelrollright"].layer = 1;
		animation["barrelrollleft"].layer = 1;
		animation["barrelrollup"].layer = 1;
		
		animation["barrelrollright"].AddMixingTransform(boneBase,false);
		animation["barrelrollleft"].AddMixingTransform(boneBase,false);
		animation["barrelrollup"].AddMixingTransform(boneBase,false);
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
			case FingerGestures.SwipeDirection.Left: animation.Play("barrelrollleft");
				break;
			case FingerGestures.SwipeDirection.Right: animation.Play("barrelrollright");
				break;
			case FingerGestures.SwipeDirection.Up: animation.Play("barrelrollup");
				break;
			}
		}		
	}
	
	public bool IsPlayingAnyRollAnimation{ get{
		return animation.IsPlaying("barrelrollup")||animation.IsPlaying("barrelrollleft")
				||animation.IsPlaying("barrelrollright");
		}}
	
	bool turreteopened = false;
	public void OpenTurette()
	{
		animation.Play("turette open");
		turreteopened = true;
	}
	
	public void CloseTurette()
	{
		if(turreteopened)
		{
			animation.Play("turette close");
			turreteopened = false;
		}
	}
}