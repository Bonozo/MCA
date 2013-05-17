using UnityEngine;
using System.Collections;

public class SpawnableObject : MonoBehaviour {
	
	public bool autoInitTransform;
	public bool randomRotation;
	public bool randomHeight;
	public float distanceMin,distanceMax;
	public float frontAngleMaxDelta;
	public float autoDestroyDistance;
	
	// Use this for initialization
	void Awake () 
	{	
		if(Options.Instance.flightControls3D)
		{
			if( autoInitTransform )
			{	
				Vector3 pos = new Vector3(0,Random.Range(-10f,10f),Random.Range(distanceMin,distanceMax));
				pos += LevelInfo.Environments.playerShip.transform.position;
				transform.position = pos;
				
				transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, Vector3.up, 
						LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y + Random.Range(-frontAngleMaxDelta,frontAngleMaxDelta) );	
				
				transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, LevelInfo.Environments.playerShip.transform.right, 
						Random.Range(-10f,10f));	
			
				if(randomRotation)
				{
					transform.rotation = Random.rotation;
				}
				else
				{
					Quaternion rot = Quaternion.LookRotation(-(transform.position-LevelInfo.Environments.playerShip.transform.position));
					rot.x = 0.0f;
					transform.rotation = rot;
				}
			}
		}
		else
		{
			if( autoInitTransform )
			{
				Vector3 pos = new Vector3(0,0,Random.Range(distanceMin,distanceMax));
				pos += LevelInfo.Environments.playerShip.transform.position; pos.y=0;
				transform.position = pos;
			
				transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, Vector3.up, 
						LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y + Random.Range(-frontAngleMaxDelta,frontAngleMaxDelta) );	
			
				if(randomRotation)
				{
					transform.rotation = Random.rotation;
				}
				else
				{
					Quaternion rot = Quaternion.LookRotation(-(transform.position-LevelInfo.Environments.playerShip.transform.position));
					rot.x = 0.0f;
					transform.rotation = rot;
				}
			}
			if(randomHeight)
			{
				float height = Random.Range(-LevelInfo.Settings.MaxSpaceY,LevelInfo.Settings.MaxSpaceY);
				transform.position = new Vector3(transform.position.x,height,transform.position.z);
			}
		}
	}
	
	void Update()
	{
		float dist = Vector3.Distance(LevelInfo.Environments.playerShip.transform.position,transform.position);
		if( dist >= autoDestroyDistance )
			Destroy(this.gameObject);
	}
}
