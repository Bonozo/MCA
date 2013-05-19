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
		vec.z += Distance; if(!Options.Instance.flightControls3D) vec.y=0;
		transform.position = vec;
		float sf = 1+Mathf.Clamp01(LevelInfo.Environments.playerShip.transform.position.z/40000f);
		transform.localScale = sf*scale;
	}
}
