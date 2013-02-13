using UnityEngine;
using System.Collections;

public class ColorPlay : MonoBehaviour {
	
	public Color colmin = new Color(0.5f,0.5f,0.5f,0.5f),colmax = new Color(0.5f,0.5f,0.5f,0.5f);
	public float speed = 0.1f;
	public float pause = 0.5f;
	public float upPause = 0f;
	public bool pauseInStart = false;
	
	
	private float pauseTime = 0f;
	private bool up=true;
	private float percent = 0f;
	
	// Use this for initialization
	void Start () {
		if(pauseInStart) pauseTime=pause;
	}
	
	// Update is called once per frame
	void Update () {
		if(pauseTime>0) {pauseTime-=0.016f; return;}
		
		percent += 0.016f * speed * (up?1:-1);
		if(percent>1||percent<0) { percent = Mathf.Clamp01(percent); up=!up; }
		if(percent==0) pauseTime=pause;
		if(percent==1) pauseTime=upPause;
		
		guiTexture.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
	}
}
