using UnityEngine;
using System.Collections;

	public enum Gems
	{
		Unlikelium,
		SureShot,
		Shield,
		Magnet
	}

public class Gem : MonoBehaviour {
	
	public float RotateSpeed = 10f;
	private float MoveSpeed = 30f;
	public Gems gemType;
	
	
	private bool magned = false;
	private Vector3 rot = new Vector3(90f,172.346f,0);
	
	// Use this for initialization
	void Start () {
		
		if( LevelInfo.Environments.playerShip == null )
		{
			Destroy(this.gameObject);
			return;
		}
		
		transform.rotation = Quaternion.Euler(rot);
		Vector3 pos = transform.position;
		pos.y = 0f;//Random.Range(-LevelInfo.Environments.playerShip.UpDownMaxHeight,LevelInfo.Environments.playerShip.UpDownMaxHeight);
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( magned )
		{
			Vector3 dir = (LevelInfo.Environments.playerShip.transform.position - transform.position).normalized;
			transform.Translate(dir*Time.deltaTime*MoveSpeed,Space.World);
		}
		
		transform.Rotate(0,0,RotateSpeed*Time.deltaTime);
		
	}
	
	public void ActivateMagnet()
	{
		if( gemType == Gems.Unlikelium )
			magned = true;
	}
}
