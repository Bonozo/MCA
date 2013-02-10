using UnityEngine;
using System.Collections;

public class MissleProjectile : MonoBehaviour {
	
	float len = 0.025f;
	float delta;
	
	void Start()
	{
		delta = len*0.5f;
	}
	
	void Update()
	{
		delta += 0.1f*Time.deltaTime;
		delta%=len;
		transform.Translate(transform.right*(delta-len*0.5f));
	}
}
