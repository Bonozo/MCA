using UnityEngine;
using System.Collections;

public class MultipleJeebieSync : MonoBehaviour {
	
	public float maxTiltAngle = 30f;
	public AlienShip[] ship;
	
	void Awake()
	{
		var tilt = Random.Range(-maxTiltAngle,maxTiltAngle);
		var delay = Random.Range(0f,1.5f);
		foreach(var s in ship)
		{
			s.synsTiltAngle = tilt;
			s.synsTiltDelay = delay;
		}
	}
	
}
