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
	
	void Update()
	{
		if(LevelInfo.State.state != GameState.Play ) return;
		
		// barrel roll animations
		Vector2 swp = GameEnvironment.Swipe;
		
		if( swp != Vector2.zero && !IsPlayingAnyRollAnimation)
		{
			if(swp.y>=0.3f && Mathf.Abs(swp.y)>Mathf.Abs(swp.x))
				animation.Play("barrelrollup");
			if(swp.x<=-0.3f && Mathf.Abs(swp.x)>Mathf.Abs(swp.y))
				animation.Play("barrelrollleft");
			if(swp.x>=0.3f && Mathf.Abs(swp.x)>Mathf.Abs(swp.y))
				animation.Play("barrelrollright");
		}
	}
	
	public bool IsPlayingAnyRollAnimation{ get{
		return animation.IsPlaying("barrelrollup")||animation.IsPlaying("barrelrollleft")
				||animation.IsPlaying("barrelrollright");
		}}
	
	public void OpenTurette()
	{
		animation.Play("turette open");
	}
	
	public void CloseTurette()
	{
		animation.Play("turette close");
	}
	
	
}
