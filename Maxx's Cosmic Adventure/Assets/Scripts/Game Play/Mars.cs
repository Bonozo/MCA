using UnityEngine;
using System.Collections;

public class Mars : MonoBehaviour {
	
	public float Distance = 1000f;
	
	private Vector3 scale;
	
	void Awake()
	{
		scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vec = LevelInfo.Environments.playerShip.transform.position;
		vec.z += Distance;
		vec.y = 0;
		transform.position = vec;
		float sf = 1+Mathf.Max(LevelInfo.Environments.playerShip.transform.position.z,0)/5000f;
		transform.localScale = sf*scale;
	}
}
