using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public Vector3 BeginPosition { set { transform.position = value; } }
	public Vector3 BeginRotation { set { transform.position = value; } }
	public float Speed = 10f;
	public float DeadTime = 10f;
	
	private bool targeted = false;
	private GameObject target;
	public GameObject particleSpark;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 	{
		
		if( targeted && target != null)
			transform.LookAt(target.transform.position);
		transform.Translate(Speed*Time.deltaTime*Vector3.forward);
		DeadTime -= Time.deltaTime;
		
		if( DeadTime <= 0 ) Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		Vector3 pos = col.contacts[0].point;
		Instantiate(particleSpark,pos,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
	public void ToTarget(GameObject v)
	{
		targeted = true;
		target = v;
	}
}
