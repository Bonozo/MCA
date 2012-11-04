using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	#region Parameters
	
	public Texture TextureGage;
	public Overheat overheatFire;
	
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
	public AudioClip AudioFire;
	public AudioClip ExplosionSoundEffect;
	
	public AudioClip AudioEngineNormal;
	public AudioClip AudioEngineBoost;
	public AudioClip AudioGameOver;
	public AudioSource SoundGamePlay;
	
	public ParticleSystem[] ExhaustArray;
	
	public float SmartBombDistance = 50;
	
	private float FireRightPowerSpeed = 0.2f;
	
	public CollisionSender colSender;
	
	#endregion
	
	#region Variables
	
	private TouchInput touchInput;
	private Score score;
	
	private GameObject leftbf,rightbf;
	private float fireDeltaTime = 0.0f;
	private float riseTime;
	private int numberUnlikelium = 0;
	
	private float travelled = 0;
	private Vector3 lastPosition;
	
	#endregion
	
	#region PowerUps
	
	private bool powerupAutoFire = false;
	
	#endregion
	
	#region Start Update
	
	// Use this for initialization
	void Start () {
		touchInput = (TouchInput)GameObject.FindObjectOfType(typeof(TouchInput));
		score = (Score)GameObject.FindObjectOfType(typeof(Score));
		leftbf = GameObject.Find("LeftBulletPipe"); 
		rightbf = GameObject.Find("RightBulletPipe");
		riseTime = WaitForRise;
		CameraSetUp();
		
		lastPosition = transform.position;
		lasty = GameEnvironment.InputAxis.y;
	}
	
	// Update is called once per frame
	void Update () {
		if( Time.deltaTime == 0.0f || score.Lose ) return;
		
		// calc travelled
		travelled += DistXZ(lastPosition,transform.position);
		lastPosition = transform.position;
		LevelInfo.Environments.guiDistanceTravelled.text = "" + (int)travelled /*+ " ly"*/;
		
		if( colSender.entered )
		{
			ManualOnCollisionEnter(colSender.col);
			colSender.Restart();
			return;
		}
		
		
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
		
		/*if( GetComponent<Detonator>().enabled ) 
		{
			if( Application.platform == RuntimePlatform.Android )
				Handheld.Vibrate();
			foreach(ParticleSystem e in ExhaustArray)
				e.enableEmission = false;
			return;
		}*/
		
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
	
	private IEnumerator SureShot()
	{
		if(!powerupAutoFire ) 
		{
			powerupAutoFire = true;
		
			float time = 10f;
			while ( time > 0f )
			{
				TryAutoShot();
				overheatFire.Down();
				LevelInfo.Environments.guiPowerUpTime.text = "Sure Shot " + Mathf.CeilToInt(time);
				time -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		
			LevelInfo.Environments.guiPowerUpTime.text = "";
			powerupAutoFire = false;
		}
	}
	
	void TryStandardShot(bool effectOverheat)
	{
		if( fireDeltaTime > 0f ) fireDeltaTime -= Time.deltaTime;
		if( effectOverheat ) overheatFire.Up();
		if( fireDeltaTime <= 0 && (!effectOverheat || !overheatFire.Overheated) )
		{
			LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(AudioFire);
			Instantiate(Bullet,leftbf.transform.position,Quaternion.Euler(0f,transform.rotation.eulerAngles.y,0f) );
			Instantiate(Bullet,rightbf.transform.position,Quaternion.Euler(0f,transform.rotation.eulerAngles.y,0f) );
			fireDeltaTime = FireDeltaTime;
		}			
	}
	
	void TryAutoShot()
	{
		if( fireDeltaTime > 0f ) fireDeltaTime -= Time.deltaTime;
		if( fireDeltaTime <= 0 )
		{
			GameObject[] g = GameObject.FindGameObjectsWithTag("AlienShip");
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
				Vector3 midpos = 0.5f*(leftbf.transform.position+rightbf.transform.position);
				GameObject j1 = (GameObject)Instantiate(Bullet,midpos,Quaternion.identity );
				GameObject j2 = (GameObject)Instantiate(Bullet,midpos,Quaternion.identity );
				j1.transform.LookAt(g[index].transform);
				j2.transform.LookAt(g[index].transform);
				j1.transform.position = leftbf.transform.position;
				j2.transform.position = rightbf.transform.position;
				j1.SendMessage("ToTarget",g[index]);
				j2.SendMessage("ToTarget",g[index]);
				g[index].SendMessage("EnableTargetingBox");
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(AudioFire);
			}
			
			fireDeltaTime = 0.1f;	
		}
	}
	
	void FireSetUp()
	{
		if( powerupAutoFire ) return;
				
		/*if( Input.GetKey(KeyCode.L) || touchInput.PowerUpAutoWithPhase(TouchPhase.Ended) )
		{
			StartCoroutine(SureShot());
			return;
		}*/
		
		// Standart shot
		if( Input.GetKey(KeyCode.F) || touchInput.FireRight || touchInput.FireLeft )
		{
			TryStandardShot(true);
		}
		else
		{
			overheatFire.Down();
		}
		
		// Auto-shoot to nearest target
		/*if( Input.GetKeyUp(KeyCode.G) || touchInput.FireLeftWithPhase(TouchPhase.Began) )
		{
			TryAutoShot();
		}*/
		
		if( touchInput.MWithPhase(TouchPhase.Ended) || Input.GetKeyUp(KeyCode.M) )
		{
			GameObject[] g = GameObject.FindGameObjectsWithTag("AlienShip");
			foreach(GameObject e in g)
				if( Vector3.Distance(e.transform.position,transform.position) <= SmartBombDistance )
					e.SendMessage("DestroyObject");	
			g = GameObject.FindGameObjectsWithTag("Asteroid");
			foreach(GameObject e in g)
				if( Vector3.Distance(e.transform.position,transform.position) <= SmartBombDistance )
					e.SendMessage("DestroyObject");	
		}
		
		/*if( !touchInput.Left && !touchInput.Right && GameEnvironment.FireButton )
		{
			audio.PlayOneShot(AudioFire);
			//Vector3 pos = GameEnvironment.LastFireCoord;
			Ray ray = mainCamera.ScreenPointToRay (GameEnvironment.LastFireCoord);
			Quaternion rot = Quaternion.LookRotation(ray.direction);
			
			Instantiate(Bullet,leftbf.transform.position,rot );
			Instantiate(Bullet,rightbf.transform.position,rot );			
		}*/
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
		
		//#endif	
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
	
	void EnableAllUnlikeliumsMagnet()
	{
		GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
		foreach(var g in gems )
		{
			g.SendMessage("ActivateMagnet");
		}
	}
	
	void ManualOnCollisionEnter(Collision col)
	{
		if( score.Lose) return;
		
		if( col.gameObject.tag == "Gem" )
		{
			Gems gemtype = col.gameObject.GetComponent<Gem>().gemType;
			switch( gemtype )
			{
			case Gems.Unlikelium:
				numberUnlikelium++;
				break;
			case Gems.SureShot:
				StartCoroutine(SureShot());
				break;
			case Gems.Shield:
				LevelInfo.Environments.score.AddLive();
				break;
			case Gems.Magnet:
				EnableAllUnlikeliumsMagnet();
				break;
			}
			Destroy(col.gameObject);
		}
		
		bool lose = HitWithName(col.gameObject.name,"AlienBullet") || HitWithName(col.gameObject.name,"AlienShip")
			|| HitWithName(col.gameObject.name,"Asteroid");
		if( lose )
		{
			score.LostLive();
			#if UNITY_ANDROID
			if( Option.Vibration )
				Handheld.Vibrate();
			#endif
			if( score.Lose )
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
	
	#region GUI
	
	void OnGUI()
	{
		if( Time.timeScale == 0.0f ) return;
		
		/*
		// Draw Gage
		float w = Screen.width, h = Screen.height;
		GUI.DrawTexture(new Rect(0.03f*w,0.8f*h,0.94f*w,0.2f*h),TextureGage);
		
		// Right Progress Bar
		float percent = 1-fireRightPower;
		GUI.BeginGroup(new Rect(0.79f*w,0.8f*h,percent*0.17f*w,0.2f*h));
		GUI.color = new Color(0.5f,0.0f,0.5f,0.5f);
		GUI.DrawTexture(new Rect(-0.76f*w,0,0.94f*w,0.2f*h),TextureGage);
		GUI.EndGroup();
		GUI.color = Color.white;
		*/
		
		
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
