using UnityEngine;
using System.Collections;

public class AlienAttack : MonoBehaviour {
	
	void Start () 
	{
		//StartCoroutine(BlueFighterAttack(250,1f));
	}
	
	void Update () 
	{
	
	}
	
	private IEnumerator BlueFighterAttack(float time,float delta)
	{
		while(time>0)
		{
			yield return new WaitForSeconds(delta);
			
			LevelInfo.Environments.generator.GenerateNewAlienShip(0);
			
			time -= delta;
		}
	}
}
