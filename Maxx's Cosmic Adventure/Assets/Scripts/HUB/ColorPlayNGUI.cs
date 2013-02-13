using UnityEngine;
using System.Collections;

public class ColorPlayNGUI : MonoBehaviour {
	
	public UISprite sprite;
	public UILabel label;
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
		if(pauseTime>0) {pauseTime-=Time.deltaTime; return;}
		
		percent += Time.deltaTime * speed * (up?1:-1);
		if(percent>1||percent<0) { percent = Mathf.Clamp01(percent); up=!up; }
		if(percent==0) pauseTime=pause;
		if(percent==1) pauseTime=upPause;
		
		if(sprite!=null) sprite.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
		if(label!=null) label.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
	}
	
	public void Reset(Color min,Color max,float speed,float pauseDown,float pauseUp)
	{
		up=true;
		percent=0;
		this.speed = speed;
		colmin = min;
		colmax = max;
		upPause = pauseUp;
		pause = pauseDown;
		Update();
	}
}
