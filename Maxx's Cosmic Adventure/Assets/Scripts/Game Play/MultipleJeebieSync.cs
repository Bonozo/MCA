using UnityEngine;
using System.Collections;

public class MultipleJeebieSync : MonoBehaviour {
	
	public float maxTiltAngle = 30f;
	public AlienShip[] ship;
	
	void Start()
	{
		var tilt = Random.Range(0,maxTiltAngle)*(IsRightOfCamera?-1:1);
		var delay = Random.Range(0f,1f);
		var time = Random.Range(0.4f,1.5f);
		foreach(var s in ship)
		{
			s.synsTiltAngle = tilt;
			s.synsTiltDelay = delay;
			s.synsTiltTime = time;
		}
	}
	
	public bool IsRightOfCamera{
		get{
		var sc = LevelInfo.Environments.mainCamera.WorldToScreenPoint(transform.position);
		sc.x /= Screen.width;
		return sc.x>0.5f;
		}
	}
	
}
