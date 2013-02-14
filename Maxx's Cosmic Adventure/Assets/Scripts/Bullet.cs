using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public int Power=1;
	public float Speed = 200f;
	public float DeadTime = 10f;	
	
	public Vector3 BeginPosition { set { transform.position = value; } }
	public Vector3 BeginRotation { set { transform.position = value; } }
	
	private bool explodewithoneshot = false;
	private bool targeted = false;
	private GameObject target;
	
	
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
			if( d <= 0.1f ) 
			{
				if(target == null ) Debug.LogError("target is null");
				if(explodewithoneshot)
					target.SendMessage("Explode");
				else
					target.SendMessage("GetHit",Power);
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
		Instantiate(LevelInfo.Environments.particleSpark,pos,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(targeted) return;
		
		if( col.gameObject.CompareTag("Asteroid") || col.gameObject.CompareTag("Enemy") )
		{
			col.gameObject.SendMessage("GetHit",Power);	
			Instantiate(LevelInfo.Environments.particleSpark,transform.position,Quaternion.identity);
			Destroy(this.gameObject);
		}
	}
	
	public void ToTarget(GameObject v)
	{
		targeted = true;
		target = v;
	}
	
	public void ExplodeTargetWithOneShot(GameObject v)
	{
		targeted = true;
		explodewithoneshot = true;
		target = v;
	}
}
