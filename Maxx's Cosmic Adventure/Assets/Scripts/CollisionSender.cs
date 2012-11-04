using UnityEngine;
using System.Collections;

public class CollisionSender : MonoBehaviour {

	public Collision col { get ;private set; }
	public bool entered;
	public string[] IgnoreTags;
	
	// Use this for initialization
	void Start () {
		entered = false;
	}
	
	void OnCollisionEnter(Collision col)
	{
		foreach(var str in IgnoreTags)
			if( col.gameObject.tag == str )
				return;
		
			entered = true;
		this.col = col;
	}
	
	public void Restart()
	{
		col = null;
		entered = false;
	}
}
