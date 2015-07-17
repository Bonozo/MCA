using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	
	bool error = false;
	void Update()
	{
		var f1 = GetDeviceAngle();
		var f2 = GetDeviceAngle2();
		if(f1!=f2) error = true;
		
		GetComponent<GUIText>().text = "" + f1 + "\n" + f2 + "\n";
		if(error) GetComponent<GUIText>().text += "error\n";
		
		/*guiText.text = " MY + " +  GameEnvironment.InputAxis + "\n";
		Vector3 acc = Input.acceleration;
		acc.x = Mathf.Round(acc.x*100f)/100f;
		acc.y = Mathf.Round(acc.y*100f)/100f;
		acc.z = Mathf.Round(acc.z*100f)/100f;
		guiText.text += "Origin " + acc;*/
	}
	
	public float GetDeviceAngle()
	{
		var acc = Input.acceleration;
		if(acc.y<=0f&&acc.z<=0f) return Mathf.Lerp(0f,90f,-acc.y);
		if(acc.y<=0f&&acc.z>=0f) return Mathf.Lerp(90f,180f,acc.z);
		if(acc.y>=0f&&acc.z>=0f) return Mathf.Lerp(180f,270f,acc.y);
		if(acc.y>=0f&&acc.z<=0f) return Mathf.Lerp(270,360f,-acc.z);
		return 0.0f;
	}
	
	public float GetDeviceAngle2()
	{
		var acc = Input.acceleration;
		if(acc.sqrMagnitude>1) acc.Normalize();
		if(acc.y<=0f&&acc.z<=0f) return Mathf.Lerp(0f,90f,-acc.y);
		if(acc.y<=0f&&acc.z>=0f) return Mathf.Lerp(90f,180f,acc.z);
		if(acc.y>=0f&&acc.z>=0f) return Mathf.Lerp(180f,270f,acc.y);
		if(acc.y>=0f&&acc.z<=0f) return Mathf.Lerp(270,360f,-acc.z);
		return 0.0f;
	}
}
