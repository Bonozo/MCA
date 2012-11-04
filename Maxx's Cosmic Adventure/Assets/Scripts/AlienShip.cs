using UnityEngine;
using System.Collections;

public class AlienShip : MonoBehaviour {
	
	public GameObject AlienBullet;
	public GameObject TargetingBoxPrefab;
	public Transform Centr,Up;
	
	public AudioClip ExplosionSoundEffect;
	public AudioClip GotEm;
	
	public float Speed = 2f;
	public float MaxDistanceToAutoDestroy = 150f;
	public int FireFrequency = 100;
	public float FireRelax = 0.5f;
	
	public float AppearTime = 1f;
	public float ExplosionTime = 2f;
	
	private Player player;
	private float fireDeltaTime = 0.0f;
	private bool expose = false;
	private Vector3 beginScale;
	private float appearTime;
	private bool showcrystal = false;
	private bool playedgotem = false;
	
	private Camera MainCamera;
	private GameObject targetingBox = null;
	
	private int NumberHitsToDie = 5;
	
	// Use this for initialization
	void Start () {
		player = (Player)GameObject.FindObjectOfType(typeof(Player));
		MainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
		
		//Vector3 pos = transform.position;
		//pos.y = Random.Range(-player.UpDownMaxHeight,player.UpDownMaxHeight);
		//transform.position = pos;
		
		transform.rotation = DestinationRotation();
		
		beginScale = transform.localScale;
		transform.localScale *= 0.0f;
		appearTime = AppearTime;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( targetingBox != null )
		{
			Vector3 centr = MainCamera.WorldToScreenPoint(Centr.position);
			Vector3 up = MainCamera.WorldToScreenPoint(Up.position);
			
			centr.x /= Screen.width; centr.y /= Screen.height;
			up.x /= Screen.width; up.y /= Screen.height;
			float sc = Mathf.Abs(centr.y-up.y)*4;
			
			targetingBox.guiTexture.enabled = centr.z > 0;
			
			targetingBox.transform.position = new Vector3(centr.x,centr.y,targetingBox.transform.position.z);
			targetingBox.transform.localScale = new Vector3(sc,sc,targetingBox.transform.localScale.z);
		}
		
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
		if( ! GetComponent<Detonator>().enabled )
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
					LevelInfo.Audio.audioSourceJeebles.PlayOneShot(GotEm);
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
		if( !expose && Random.Range(1,FireFrequency) == 1 && fireDeltaTime <= 0f )
		{
			Instantiate(AlienBullet,transform.position,transform.rotation);
			
			fireDeltaTime = FireRelax;
		}
		////////////////////////////////////////
		
		//Vector3 pos = transform.position;
		//pos.y = 2*Mathf.Sin(100*Time.time);
		//transform.position = pos;
		
		if( Vector3.Distance(transform.position,player.transform.position) >= MaxDistanceToAutoDestroy ) 
			Destroy(this.gameObject);
	}
	
	private bool DestroyNeed()
	{
		if( player == null ) return true;
		if(player.GetComponent<Detonator>().enabled && Vector3.Distance(player.transform.position,transform.position) <= 3f ) return true;
		
		float y = transform.rotation.eulerAngles.y; if( y>=180f) y-=360f;
		float playery = player.transform.rotation.eulerAngles.y; if(playery>=180f) playery-=360f;
		if( Vector3.Distance(transform.position,player.transform.position) >= 50f && Mathf.Abs(y-playery) <= 45f )
			return true;
		
		return false;
	}
	
	Quaternion DestinationRotation()
	{
		Quaternion rot = Quaternion.LookRotation(-(transform.position-player.transform.position));
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
		GetComponent<Detonator>().enabled = true;
		
		Destroy(this.rigidbody);
		Destroy(this.collider);
		gameObject.tag = null;
		
		if(col != null) Destroy(col.gameObject);
		if( targetingBox != null ) Destroy(targetingBox);
		if(withplayer && player != null) player.SendMessage("AddScore");
		expose = true;
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
		if( GetComponent<Detonator>().enabled )
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
		if( GetComponent<Detonator>().enabled ) return;
		showcrystal = true;
		GetComponent<Detonator>().enabled = true;
		Destroy(this.rigidbody);
		if( targetingBox != null ) Destroy(targetingBox);
		player.SendMessage("AddScore");
		expose = true;
		LevelInfo.Audio.audioSourceJeebles.PlayOneShot(ExplosionSoundEffect);
		LevelInfo.Audio.audioSourceJeebles.time = 0.5f;	
	}
	
	public void EnableTargetingBox()
	{
		if( targetingBox == null && !GetComponent<Detonator>().enabled )
			targetingBox = (GameObject)Instantiate(TargetingBoxPrefab);
	}
}
