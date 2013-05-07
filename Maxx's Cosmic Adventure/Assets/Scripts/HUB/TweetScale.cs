using UnityEngine;
using System.Collections;

public class TweetScale : MonoBehaviour {
	
	public float min=1f, max=1.5f;
	public float speed = 1f;
	public float pause = 1f;
	
	private float time=0f;
	private float period;
	private Vector2 begin;
	
	void Start()
	{
		period = 2*(max-min)+pause;
		begin = transform.localScale;
	}
	
	void Update()
	{
		time += 0.016f*speed;
		if(time>period) time-=period;
		if(time>pause)
		{
			var c = time-pause;
			if(c>max-min) c=2*(max-min)-c;
			transform.localScale = (min+c)*begin;
		}
	}
}
