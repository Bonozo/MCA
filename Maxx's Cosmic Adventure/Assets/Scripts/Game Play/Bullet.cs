using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public Projectile projectileType = Projectile.None;
	public int Power=1;
	public float Speed = 200f;
	public float DeadTime = 10f;	
	public bool Active = true;
	
	public Vector3 BeginPosition { set { transform.position = value; } }
	public Vector3 BeginRotation { set { transform.position = value; } }
	
	private bool explodewithoneshot = false;
	private bool targeted = false;
	private GameObject target;
	
	void Update ()
	{	
		if(LevelInfo.State.state != GameState.Play) return;
		
		if(!Active) 
		{
			UpdateLRHitch();
			return;
		}
		
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
				
				if(projectileType == Projectile.AutoFire)
					Stats.Instance.ReportShotJeebieWithWeapons(Projectile.AutoFire);
				
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
	
	void OnTriggerEnter(Collider col)
	{
		if(targeted) return;
		
		// reporting
		if(col.gameObject.CompareTag("Enemy"))
			Stats.Instance.ReportKilledJeebieWithAWeapon(projectileType);
		
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
	
	public void Activate()
	{
		Active = true;
	}
	
	
	
	float len = 0.05f;
	float delta = 0.025f*0.5f;
	void UpdateLRHitch()
	{
		if(Time.deltaTime>0)
		{
			delta += 0.1f*Time.deltaTime;
			delta%=len;
			transform.Translate(transform.right*(delta-len*0.5f));
		}
	}
}
