using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	#region Parameters

	public float RotationMaxAngle = 20.0f;
	public float RotationAngleChangeFactor = 20.0f;
	public float RotationToRotateFactor = 20.0f;
	public float UpDownChangeFactor = 0.1f;
	public float Speed = 10f;
	public float FireDeltaTime;
	
	public float ExplosionTime = 3.0f;
	
	public float WaitForRise = 3f;
	public float WaitForStart = 2f;
	public float BeginHeight = -3f;
	
	public float CameraZ = 9.5f;
	public float CameraHeight = 1.5f;
	
	public float UpDownMaxHeight = 3f;
	
	public GameObject Bullet;
	public GameObject BulletYellow;
	public AudioClip AudioFire;
	public AudioClip ExplosionSoundEffect;
	
	public AudioClip AudioEngineNormal;
	public AudioClip AudioEngineBoost;
	public AudioClip AudioGameOver;
	public AudioSource SoundGamePlay;
	
	public ParticleSystem[] ExhaustArray;
	
	public GameObject leftbf,rightbf;
	public GameObject leftturret,rightturret;
	
	#endregion
	
	[System.NonSerialized]
	public int numberUnlikelium = 0;
	
	#region Variables
	
	private TouchInput touchInput;
	
	private float fireDeltaTime = 0.0f;
	private float autofireDeltaTime = 0.0f;
	private float riseTime;
	
	
	private float travelled = 0;
	public float DistanceTravelled { get { return travelled; }}
	
	private Vector3 lastPosition;
	
	#endregion
	
	#region PowerUps
	
	[System.NonSerializedAttribute]
	public bool AutoFire = false;
	
	[System.NonSerializedAttribute]
	public bool FreezeWorld = false;
	
	
	public void StartSureShot()
	{
		StartCoroutine(SureShotThread());
	}

	private IEnumerator SureShotThread()
	{
		if(!AutoFire) 
		{
			AutoFire = true;
		
			float time = 10f;
			while ( time > 0f )
			{
				TryAutoShot();
				LevelInfo.Environments.guiPowerUpTime.text = "Sure Shot " + Mathf.CeilToInt(time);
				time -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		
			LevelInfo.Environments.guiPowerUpTime.text = "";
			AutoFire = false;
		}
	}
	
	public void StartFreezeWorld()
	{
		StartCoroutine(FreezeWorldThread());
	}
	
	private IEnumerator FreezeWorldThread()
	{
		if(!FreezeWorld ) 
		{
			FreezeWorld = true;
			yield return new WaitForSeconds(10f);
			FreezeWorld = false;
		}
	}
	
	
	#endregion
	
	#region Start Update
	
	void Awake()
	{
		LevelInfo.Environments.FPS.SetActive(Option.ShowFPS);
	}
	
	// Use this for initialization
	void Start () {
		
		touchInput = (TouchInput)GameObject.FindObjectOfType(typeof(TouchInput));
		riseTime = WaitForRise;
		CameraSetUp();
		
		lastPosition = transform.position;
		lasty = GameEnvironment.InputAxis.y;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(LevelInfo.State.state != GameState.Play ) return;
		
		// calc travelled
		travelled += DistXZ(lastPosition,transform.position);
		lastPosition = transform.position;
		LevelInfo.Environments.guiDistanceTravelled.text = "" + (int)travelled /*+ " ly"*/;
		
		if( riseTime > 0 )
		{
			Vector3 pos = transform.position;
			pos.y = riseTime*BeginHeight/WaitForRise;
			transform.position = pos;
			riseTime -= Time.deltaTime;
			if( riseTime <= 0 )
			{
				pos.y = 0;
				transform.position = pos;
				LevelInfo.Environments.generator.GenerateAlienShip = true;
				LevelInfo.Environments.generator.GenerateAsteroid = true;
			}
			return;
		}
		
		//// animation playing setup ////
		if( !animation.isPlaying)
		{
			Vector2 swp = GameEnvironment.Swipe;
			if( swp.y>=0.3f && Mathf.Abs(swp.y)>Mathf.Abs(swp.x) || Input.GetKeyUp(KeyCode.R) )
				animation.Play("barrelrollup");
			if( swp.x<=-0.3f && Mathf.Abs(swp.x)>Mathf.Abs(swp.y) || Input.GetKeyUp(KeyCode.Q) )
				animation.Play("barrelrollleft");
			if( swp.x>=0.3f && Mathf.Abs(swp.x)>Mathf.Abs(swp.y) || Input.GetKeyUp(KeyCode.E) )
				animation.Play("barrelrollright");
		}
		/////////////////////////////////
		
		FireSetUp();
		
		RotationSetUp();
		
		TransformSetUp();

		
		CameraSetUp();
		
		ExhaustSetUp();
		
		SoundSetUp();
	}
	
	#endregion

	
	#region Transform
	
	private float deltaY = 0f;
	private float deltaYtime = 0f;
	private const float deltaYtimeMax = 0.3f;
	private float ymoveindex = 0;
	
	private float lasty = 0;
	
	void TransformSetUp()
	{	
		float delta = GameEnvironment.InputAxis.y-lasty;
		lasty = GameEnvironment.InputAxis.y;
		
		deltaYtime += Time.deltaTime;
		
		if( delta != 0 && deltaYtime <= deltaYtimeMax )deltaY += delta;
		else { deltaY = 0; deltaYtime = 0f; }
		
		if( deltaY >= 0.3f ) ymoveindex = 1f;
		if( deltaY <= -0.3f ) ymoveindex = -1f;
			
		
		float y = transform.position.y;
		
		if( ymoveindex == 0f )
		{
			float ly = y;
			if( y > 0 ) y -= Time.deltaTime*UpDownChangeFactor;
			if( y < 0 ) y += Time.deltaTime*UpDownChangeFactor;
			if( y*ly < 0 ) y = 0;
		}
		else
		{
			y += ymoveindex*Time.deltaTime*UpDownChangeFactor*10f;
			if( y >= UpDownMaxHeight ) { y= UpDownMaxHeight;ymoveindex=0; }
			if( y <=-UpDownMaxHeight ) { y=-UpDownMaxHeight;ymoveindex=0; }
		}
		
		transform.position = new Vector3(transform.position.x,y,transform.position.z);
		transform.Translate(Speed*Time.deltaTime*Vector3.forward);	
	}
	
	#endregion
	
	#region Firing

	void TryStandardShot(bool effectOverheat)
	{
		if( fireDeltaTime > 0f ) fireDeltaTime -= Time.deltaTime;
		bool ovh = LevelInfo.Environments.fireOverheat.Overheated;
		if( effectOverheat ) LevelInfo.Environments.fireOverheat.Up();
		if( !ovh && LevelInfo.Environments.fireOverheat.Overheated ) {/*PLevelInfo.Audio.PlayAudioWeaponExpire();*/}
		if( fireDeltaTime <= 0 && (!effectOverheat || !LevelInfo.Environments.fireOverheat.Overheated) )
		{
			LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(AudioFire);
			Instantiate(Bullet,leftbf.transform.position,Quaternion.Euler(0f,transform.rotation.eulerAngles.y,0f) );
			Instantiate(Bullet,rightbf.transform.position,Quaternion.Euler(0f,transform.rotation.eulerAngles.y,0f) );
			fireDeltaTime = FireDeltaTime;
		}			
	}
	
	void TryAutoShot()
	{
		if( autofireDeltaTime > 0f ) autofireDeltaTime -= Time.deltaTime;
		if( autofireDeltaTime <= 0 )
		{
			GameObject[] g = GameObject.FindGameObjectsWithTag("Enemy");
			int index = -1; float minvalue = float.PositiveInfinity;
			for(int i=0;i<g.Length;i++)
			{
				Vector3 toscreen = LevelInfo.Environments.mainCamera.WorldToScreenPoint(g[i].transform.position);
				if(toscreen.x >= 0 && toscreen.x <= Screen.width && 
					toscreen.y >= 0 && toscreen.y <= Screen.height && toscreen.z > 1f && toscreen.z < minvalue )
				{
					minvalue = toscreen.z;
					index = i;
				}
			}
			if(index != -1)
			{
				Vector3 midpos = 0.5f*(leftturret.transform.position+rightturret.transform.position);
				GameObject j1 = (GameObject)Instantiate(BulletYellow,midpos,Quaternion.identity );
				GameObject j2 = (GameObject)Instantiate(BulletYellow,midpos,Quaternion.identity );
				j1.transform.LookAt(g[index].transform);
				j2.transform.LookAt(g[index].transform);
				j1.transform.position = leftturret.transform.position;
				j2.transform.position = rightturret.transform.position;
				j1.SendMessage("ToTarget",g[index]);
				j2.SendMessage("ToTarget",g[index]);
				g[index].SendMessage("EnableTargetingBox");
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(AudioFire);
			}
			
			autofireDeltaTime = 0.1f;	
		}
	}
	
	void FireSetUp()
	{
		
		// Standart shot
		if( Input.GetKey(KeyCode.F) || touchInput.FireRight || touchInput.FireLeft )
			TryStandardShot(true);
		else
			LevelInfo.Environments.fireOverheat.Down();
		
		if( AutoFire ) return;
		
		if( Input.GetKeyUp(KeyCode.L) )
		{
			StartCoroutine(SureShotThread());
			return;
		}
		// Auto-shoot to nearest target
		/*if( Input.GetKeyUp(KeyCode.G) || touchInput.FireLeftWithPhase(TouchPhase.Began) )
			TryAutoShot();
		*/
	}
	
	#endregion
	
	private float rotationdeltaable = 5f;
	
	void RotationSetUp()
	{
		Vector3 rot = transform.rotation.eulerAngles;
		if( rot.z > 180.0f ) rot.z -= 360.0f;
		
		float xaxis = -GameEnvironment.InputAxis.x*90f;
		float delta = RotationAngleChangeFactor*Time.deltaTime;
		if( xaxis - rot.z > rotationdeltaable ) rot.z += delta;
		if(  rot.z - xaxis > rotationdeltaable ) rot.z -= delta;
		if( Mathf.Abs(xaxis) <= rotationdeltaable && Mathf.Abs(rot.z) <= rotationdeltaable )
			rot.z = 0;
		rot.z = Mathf.Clamp(rot.z,-RotationMaxAngle,RotationMaxAngle);
		
		rot.y -= rot.z*RotationToRotateFactor*Time.deltaTime;
		
		transform.rotation = Quaternion.Euler(rot);	
	}
	
	void CameraSetUp()
	{
		transform.Translate(-Vector3.forward*lastexhaust);
		
		LevelInfo.Environments.mainCamera.transform.position = new Vector3(transform.position.x,0f,transform.position.z);
		LevelInfo.Environments.mainCamera.transform.rotation = transform.rotation;
		
		
		Vector3 crot = LevelInfo.Environments.mainCamera.transform.rotation.eulerAngles;
		crot.z = 0.0f;
		crot.x = 20.0f;
		LevelInfo.Environments.mainCamera.transform.rotation = Quaternion.Euler(crot);
		
		LevelInfo.Environments.mainCamera.transform.Translate(-Vector3.forward*CameraZ);
		LevelInfo.Environments.mainCamera.transform.Translate(0,CameraHeight,0);
		
		transform.Translate(Vector3.forward*lastexhaust);
	}
	
	float lastexhaust = 1f;
	void ExhaustSetUp()
	{	
		float currentspeed = ExhaustArray[0].startSpeed;
		if( touchInput.B && !LevelInfo.Environments.fuelOverheat.Up())
		{
			currentspeed += 0.5f;
		}
		else
		{
			LevelInfo.Environments.fuelOverheat.Down();
			currentspeed -= 0.5f;
		}
		currentspeed = Mathf.Clamp(currentspeed,1f,8f);
		foreach(ParticleSystem e in ExhaustArray)
			e.startSpeed = currentspeed;
		float delta = currentspeed-lastexhaust;
		lastexhaust = currentspeed;
		
		transform.Translate(Vector3.forward*delta);
		Speed += 4*delta;
	}
	
	void SoundSetUp()
	{
		if( touchInput.B )
			LevelInfo.Audio.audioSourcePlayerShip.clip = AudioEngineBoost;
		else
			LevelInfo.Audio.audioSourcePlayerShip.clip = AudioEngineNormal;
		if( !LevelInfo.Audio.audioSourcePlayerShip.isPlaying ) LevelInfo.Audio.audioSourcePlayerShip.Play();
	}
	
	bool HitWithName(string name,string comparewith)
	{
		return name.Length >= comparewith.Length && name.Substring(0,comparewith.Length) == comparewith;
	}
	
	/*??*/
	void ManualOnCollisionEnter(Collision col)
	{
		if( LevelInfo.State.state != GameState.Play ) return;
		
		bool lose = HitWithName(col.gameObject.name,"AlienBullet") || HitWithName(col.gameObject.name,"AlienShip")
			|| HitWithName(col.gameObject.name,"Asteroid");
		if( lose )
		{
			LevelInfo.Environments.score.LostLive();
			#if UNITY_ANDROID || UNITY_IPHONE
			if( Option.Vibration )
				Handheld.Vibrate();
			#endif
			if( LevelInfo.State.state == GameState.Lose )
			{
				transform.localScale *= 0;
				GameObject c = GameObject.Find("GameOverText");
				if(c != null )
					c.GetComponent<GameOver>().enabled = true;
				
				foreach(var r in ExhaustArray )
					r.enableEmission = false;
				
				LevelInfo.Audio.StopAll();
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(AudioGameOver);
				StopAllCoroutines();
			}
		}
		Destroy(col.gameObject);
	}
	
	public float x,y,z;
	
	private int scorepoint = 0;
	public void AddScore()
	{
		scorepoint++;
	}
	
	#region Get Hit
	
	public void GetAsteroidBump()
	{
		
	}
	
	public void GetEnemyShipBump()
	{
		
	}
	
	public void GetEnemyBulletBump()
	{
		
	}
	
	#endregion
	
	#region GUI
	
	void OnGUI()
	{
		if( Time.timeScale == 0.0f ) return;
		
		GUI.Label(new Rect(0,50,100,50), "Score : " + scorepoint + "\nUnlikelium : " + numberUnlikelium);
		//GUI.Label(new Rect(0,165,400,400), "Acceleration : " + GameEnvironment.InputAxis);
	}
	
	#endregion
	
	#region Helpful
	
	private float DistXZ(Vector3 a,Vector3 b)
	{
		a.y=b.y=0;
		return Vector3.Distance(a,b);
	}
	
	#endregion
}
