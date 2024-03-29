using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
	
	public int unlikeliumValue = 1;
	public float RotateSpeed = 10f;
	public GameObject particle;
	public Gems gemType;
	
	private float AutodestructDistance = 500;
	private float MoveSpeed = 100f;
	
	[System.NonSerializedAttribute]
	public bool loveUnlikelium = false;
	
	// Use this for initialization
	void Start ()
	{
		tag = "Gem";
		if(gemType == Gems.Unlikelium && unlikeliumValue == 1 )
		LevelInfo.Environments.tutorials.SpawnedSimpleUnlikelium();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(LevelInfo.State.state != GameState.Play) return;
		if(gemType == Gems.Unlikelium)
		{
			var player = LevelInfo.Environments.playerShip;
			if( player.Magned &&  player.DistXZ(transform.position) <= 60f)
			{
				Vector3 dir = (LevelInfo.Environments.playerShip.transform.position - transform.position).normalized;
				transform.Translate(dir*Time.deltaTime*MoveSpeed,Space.World);
			}
		}
		
		if(particle != null )
			particle.transform.Rotate(0,-1.25f*RotateSpeed*Time.deltaTime,0);
		transform.Rotate(0,RotateSpeed*Time.deltaTime,0);
		
		if(LevelInfo.Environments.playerShip.DistXZ(transform.position) > AutodestructDistance )
		{
			if(loveUnlikelium) LevelInfo.Environments.generator.ReportLoveUnlikeliumPartAutodestruct();
			Destroy(this.gameObject);
		}
	}
}
