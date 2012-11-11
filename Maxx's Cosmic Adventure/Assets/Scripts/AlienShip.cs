using UnityEngine;
using System.Collections;

public class AlienShip : MonoBehaviour {
	
	public GameObject AlienBullet;
	public Transform Centr,Up;
	
	public AudioClip ExplosionSoundEffect;
	
	public float Speed = 2f;
	public int FireFrequency = 100;
	public float FireRelax = 0.5f;
	
	public float AppearTime = 1f;
	public float ExplosionTime = 2f;
	
	private float fireDeltaTime = 0.0f;
	private Vector3 beginScale;
	private float appearTime;
	private bool showcrystal = false;
	private bool playedgotem = false;
	
	private GameObject targetingBox = null;
	private bool exploded = false;
	
	private int NumberHitsToDie = 5;
	
	// Use this for initialization
	void Start () {
		
		//Vector3 pos = transform.position;
		//pos.y = Random.Range(-player.UpDownMaxHeight,player.UpDownMaxHeight);
		//transform.position = pos;
		
		transform.rotation = DestinationRotation();
		
		beginScale = transform.localScale;
		transform.localScale *= 0.0f;
		appearTime = AppearTime;
		
		tag = "AlienShip";
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateTargetingBox();
		
		if( appearTime >= 0 )
		{
			transform.localScale = beginScale*(AppearTime-appearTime)/AppearTime;
			appearTime -= Time.deltaTime;
			return;
		}
		
		if( DestroyNeed() )
		{
			Destroy(this.gameObject);
			return;
		}
		
			
		
		////////////////// Transform Setup //////////////
		if( !exploded )
		{
			transform.Translate(Speed*Time.deltaTime*Vector3.forward);
		}
		else
		{
			transform.localScale *= 0.0f;
			ExplosionTime -= Time.deltaTime;
			if( !playedgotem && ExplosionTime <=  1.6f )
			{
				if( GameEnvironment.Probability(5))
					LevelInfo.Audio.PlayAudioGotEm();
				playedgotem = true;
			}
			if (ExplosionTime <= 0 )
			{
				if(showcrystal)
					LevelInfo.Environments.generator.GenerateNewGem(transform.position);
				Destroy(this.gameObject);
			}
		}
		///////////////////////////////////////////////
		
		///////////// Fire SetUp ///////////////
		if( fireDeltaTime > 0f ) fireDeltaTime -= Time.deltaTime;
		if( !exploded && Random.Range(1,FireFrequency) == 1 && fireDeltaTime <= 0f )
		{
			Instantiate(AlienBullet,transform.position,transform.rotation);
			
			fireDeltaTime = FireRelax;
		}
		////////////////////////////////////////
		
		//Vector3 pos = transform.position;
		//pos.y = 2*Mathf.Sin(100*Time.time);
		//transform.position = pos;
	}
	
	private void UpdateTargetingBox()
	{
		if( targetingBox != null )
		{
			Vector3 centr = LevelInfo.Environments.mainCamera.WorldToScreenPoint(Centr.position);
			Vector3 up = LevelInfo.Environments.mainCamera.WorldToScreenPoint(Up.position);
			
			centr.x /= Screen.width; centr.y /= Screen.height;
			up.x /= Screen.width; up.y /= Screen.height;
			float sc = Mathf.Abs(centr.y-up.y)*4;
			
			targetingBox.guiTexture.enabled = centr.z > 0;
			
			targetingBox.transform.position = new Vector3(centr.x,centr.y,targetingBox.transform.position.z);
			targetingBox.transform.localScale = new Vector3(sc,sc,targetingBox.transform.localScale.z);
		}
	}
	
	private bool DestroyNeed()
	{
		if( LevelInfo.Environments.playerShip == null ) return true;
		
		float y = transform.rotation.eulerAngles.y; if( y>=180f) y-=360f;
		float playery = LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y; if(playery>=180f) playery-=360f;
		if( Vector3.Distance(transform.position,LevelInfo.Environments.playerShip.transform.position) >= 50f && Mathf.Abs(y-playery) <= 45f )
			return true;
		
		return false;
	}
	
	Quaternion DestinationRotation()
	{
		Quaternion rot = Quaternion.LookRotation(-(transform.position-LevelInfo.Environments.playerShip.transform.position));
		rot.x = 0.0f;
		return rot;
	}
	
	void OnDestroy()
	{
		if( targetingBox != null ) Destroy(targetingBox);
	}
	
	void Explode(bool withplayer,Collision col)
	{
		if( withplayer ) showcrystal = true;
		Instantiate(LevelInfo.Environments.particleExplosionJeeb,Centr.transform.position,Quaternion.identity);
		exploded = true;
		
		Destroy(this.rigidbody);
		Destroy(this.collider);
		gameObject.tag = null;
		
		if(col != null) Destroy(col.gameObject);
		if( targetingBox != null ) Destroy(targetingBox);
		if(withplayer && LevelInfo.Environments.playerShip != null) LevelInfo.Environments.playerShip.AddScore();
		
		LevelInfo.Audio.audioSourceJeebles.PlayOneShot(ExplosionSoundEffect);
		LevelInfo.Audio.audioSourceJeebles.time = 0.5f;
		if( !withplayer ) playedgotem = true;
		
	}
	
	int t;
	
	void GetHit()
	{
		if( --NumberHitsToDie == 0 )
		{
			Explode(true,null);
		}	
	}
	
	void OnCollisionEnter(Collision col)
	{	
		if( exploded )
			return;
		
		if( col.gameObject.tag == "Bullet" )
		{
			if( --NumberHitsToDie == 0 )
			{
				Explode(true,col);
			}
		}
		else if( HitWithName(col.gameObject.name,"Asteroid") )
		{
			Explode(false,col);
		}
	}
	
	bool HitWithName(string name,string comparewith)
	{
		return name.Length >= comparewith.Length && name.Substring(0,comparewith.Length) == comparewith;
	}
	
	public void DestroyObject()
	{
		if( exploded) return;
		Explode(true,null);
	}
	
	public void EnableTargetingBox()
	{
		if( targetingBox == null && !exploded )
			targetingBox = (GameObject)Instantiate(LevelInfo.Environments.targetingBoxPrefab);
	}
}
