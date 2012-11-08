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
		if( target == null || target.transform.localScale.x == 0 ) targeted = false;
		if( targeted)
		{
			transform.LookAt(target.transform.position);
		
			float d = Vector3.Distance(target.transform.position,transform.position);
			transform.Translate( Mathf.Min(d,Speed*Time.deltaTime)*Vector3.forward );
			d = Vector3.Distance(target.transform.position,transform.position);
			if( d == 0.0f ) 
			{
				if(target == null ) Debug.LogError("turget is null");
				target.SendMessage("GetHit");
				Destroy(this.gameObject);
				return;
			}
		}
		else
		{
			transform.Translate( Speed*Time.deltaTime*Vector3.forward );
		}
		DeadTime -= Time.deltaTime;
		
		if( DeadTime <= 0 ) Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if( GameEnvironment.HitWithName(col.gameObject.name,"Bullet") ) return;
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
