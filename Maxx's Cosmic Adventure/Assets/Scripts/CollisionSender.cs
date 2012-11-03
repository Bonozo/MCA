using UnityEngine;
using System.Collections;

public class CollisionSender : MonoBehaviour {
	
	public Collision col { get ;private set; }
	public bool entered;
	
	// Use this for initialization
	void Start () {
		entered = false;
	}
	
	void OnCollisionEnter(Collision col)
	{
		entered = true;
		this.col = col;
	}
	
	public void Restart()
	{
		col = null;
		entered = false;
	}
}
