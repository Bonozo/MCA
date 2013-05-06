using UnityEngine;
using System.Collections;

public class Overheat : MonoBehaviour {
	
	public UISprite[] parts;
	public float UpTime = 2f;
	public float DownTime = 2f;
	public bool FullReloadForUse;
	
	
	private float delay = 0;
	private bool reloading = false;

	void OnEnable () 
	{
		foreach(var c in parts) c.enabled=false;
	}
	
	private void SetPercent(float c)
	{
		int n = (int)(parts.Length*c);
		for(int i=0;i<n;i++) parts[parts.Length-1-i].enabled = true;
		for(int i=n;i<parts.Length;i++) parts[parts.Length-1-i].enabled = false;
	}
	
	public bool Up()
	{
		if( !reloading )
		{
			delay += Time.deltaTime*UpTime;
			delay = Mathf.Clamp01(delay);
			SetPercent(delay);
			if( delay == 1f && FullReloadForUse ) reloading = true;
		}
		return Overheated;
	}
	
	public void Down()
	{
		delay -= Time.deltaTime*DownTime;
		delay = Mathf.Clamp01(delay);
		SetPercent(delay);
		if( delay == 0f ) reloading = false;
	}
	
	public bool Overheated{
		get{
			var res = (delay == 1f || reloading);
			if(res) LevelInfo.Environments.tutorials.ButtonOverheated();
			return res;
		}
	}

}
