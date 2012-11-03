using UnityEngine;
using System.Collections;

public class GameEnvironment : MonoBehaviour {
	
	#region Store
	
	public static float Unlikelium = 100;
	
	public static string[] Offence = new string[]
	{
		"Offence 1",
		"Offence 2",
		"Offence 3",
		"Offence 4",
		"Offence 5",
		"Offence 7",
		"Offence 8",
		"Offence 9",
		"Offence 10"
	};
	
	public static string[] Defence = new string[]
	{
		"Defence 1",
		"Defence 2",
		"Defence 3",
		"Defence 4",
		"Defence 5",
		"Defence 6",
		"Defence 7",
		"Defence 8",
		"Defence 9",
		"Defence 10",
		"Defence 11",
	};
	
	#endregion
	
	#if !UNITY_ANDROID
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
	
	#else
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
		dir.y = dir.z; dir.z = 0;
			
		return dir;
	}}
	
	#endif
	
	#region Helpful
	
	public static bool HitWithName(string name,string comparewith)
	{
		return name.Length >= comparewith.Length && name.Substring(0,comparewith.Length) == comparewith;
	}
	
	public static bool Probability(int cases)
	{
		return Random.Range(0,cases)==1;
	}
	
	#endregion
}
