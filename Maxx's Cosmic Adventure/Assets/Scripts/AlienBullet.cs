using UnityEngine;
using System.Collections;

public class AlienBullet : MonoBehaviour {
	
	public Vector3 BeginPosition { set { transform.position = value; } }
	public Vector3 BeginRotation { set { transform.position = value; } }
	public float Speed = 10f;
	public float DeadTime = 10f;
	
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("PlayerShip");
	}
	
	// Update is called once per frame
	void Update () {
		if( player == null )
		{
			Destroy(this.gameObject);
			return;
		}
		
		transform.Translate(Speed*Time.deltaTime*Vector3.forward);
		DeadTime -= Time.deltaTime;
		if( DeadTime <= 0 ) Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
	}
		
	private float MaxAxeDistance(Vector3 a,Vector3 b)
	{
		return Mathf.Max(Mathf.Abs(a.x-b.x),Mathf.Abs(a.y-b.y),Mathf.Abs(a.z-b.z));
	}
}
