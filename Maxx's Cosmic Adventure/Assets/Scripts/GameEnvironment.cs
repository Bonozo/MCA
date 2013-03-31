using UnityEngine;
using System.Collections;

public class GameEnvironment : MonoBehaviour {
	
	#if UNITY_ANDROID || UNITY_IPHONE
	private static Vector2 startPos;
	public static Vector2 Swipe { get {
		Vector2 res = Vector2.zero;
		if (Input.touchCount > 0) 
		{
        	var touch = Input.touches[0];
			Vector2 pos = touch.position;
				
			pos.x /= Screen.width;
			pos.y /= Screen.height;
			
			switch (touch.phase)
			{
			case TouchPhase.Began:
                startPos = pos;
                break;
            case TouchPhase.Ended:
		        res = pos - startPos;        
                break;
			}
		}
		return res;
	}}
	
	private static bool moved=false;
	
	public static Vector3 LastFireCoord { get; private set; }
	
	public static bool FireButton { get {
		
		// Touch detection
		if (Input.touchCount > 0) 
		{
        	var touch = Input.touches[0];
			
			switch (touch.phase)
			{
			case TouchPhase.Began:
                moved = false;
                break;
			case TouchPhase.Moved:
				moved = true;
				break;
            case TouchPhase.Ended:
		        if(!moved)
				{
					LastFireCoord = Input.touches[0].position;
					return true;       
				}
				break;
			}
		}
			
		return false;
			
	}}
	
	public static Vector3 InputAxis { get {
		Vector3 dir = Vector3.zero;
		dir.x = -Input.acceleration.y;
		dir.z = Input.acceleration.x;
		
		if(dir.sqrMagnitude > 1)
			dir.Normalize();
		dir.y = -dir.x;
		dir.x = dir.z;
		dir.z = 0;
			
		return dir;
	}}
	
	public static float DeviceAngle{ 
		get{
			var acc = Input.acceleration;
			if(acc.sqrMagnitude>1) acc.Normalize();
			if(acc.y<=0f&&acc.z<=0f) return Mathf.Lerp(0f,90f,-acc.y);
			if(acc.y<=0f&&acc.z>=0f) return Mathf.Lerp(90f,180f,acc.z);
			if(acc.y>=0f&&acc.z>=0f) return Mathf.Lerp(180f,270f,acc.y);
			if(acc.y>=0f&&acc.z<=0f) return Mathf.Lerp(270,360f,-acc.z);
			return 0.0f;
			}
	}
	
	#else
	private static Vector2 last;
	public static Vector2 Swipe { get {
		Vector2 mousepos = Input.mousePosition;
		mousepos.x /= Screen.width;
		mousepos.y /= Screen.height;

		Vector2 res = Vector2.zero;
		if( Input.GetMouseButtonDown(0) )
		{
			last = mousepos;
		}
		if( Input.GetMouseButtonUp(0) )
		{
			res = mousepos-last;
		}
		
		return res;
	}}
	
	public static Vector3 LastFireCoord { get; private set; }
	
	public static bool FireButton { get {
		if( Input.GetMouseButtonUp(0) )
		{
			LastFireCoord = Input.mousePosition;
			return true;     		
		}
			
		return false;
			
	}}
	
	public static Vector3 InputAxis { get {
		return new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0f);
	}}
	
	public static float DeviceAngle{ 
		get{
			float yaxis = 90f*Input.GetAxis("Vertical");
			if(yaxis<0f) yaxis=360f+yaxis;
			return -yaxis;
			}
	}
	
	#endif
	
	#region Helpful
	
	public static bool Probability(int cases)
	{
		return Random.Range(0,cases)==1;
	}
	
	#endregion
}
