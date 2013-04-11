using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	#region Parameters
	
	public float RotationMaxAngle = 20.0f;
	public float RotationAngleChangeFactor = 20.0f;
	public float RotationToRotateFactor = 20.0f;
	public float FireDeltaTime;
	
	public float ExplosionTime = 3.0f;
	

	public float BeginHeight = -3f;
	
	public float CameraZ = 9.5f; 
	public float CameraHeight = 1.5f;
	
	public AudioClip AudioFire;
	public AudioClip ExplosionSoundEffect;
	
	public AudioClip AudioEngineNormal;
	public AudioClip AudioEngineBoost;
	public AudioClip AudioGameOver;
	
	public ParticleSystem[] ExhaustArray;
	
	public GameObject leftturret,rightturret;
	
	public static int attempt = 0;
	
	#endregion
	
	#region Variables
	
	private TouchInput touchInput;
	
	private float fireDeltaTime = 0.0f;
	private float autofireDeltaTime = 0.0f;
	
	private float travelled = 0;
	public float DistanceTravelled { get { return travelled; }}
	
	private Vector3 lastPosition;
	
	#endregion
	
	#region PowerUps
	
	[System.NonSerializedAttribute]
	public bool AutoFire = false;
	
	[System.NonSerializedAttribute]
	public bool FreezeWorld = false;
	
	[System.NonSerializedAttribute]
	public bool Intergalactic = false;
	
	[System.NonSerializedAttribute]
	public bool LoveUnlikelium = false;
	
	[System.NonSerializedAttribute]
	public bool Magned = false;
	
	public void ClearAllPowerups()
	{
		StopAllCoroutines();
		AutoFire = false;
		FreezeWorld = false;
		Intergalactic = false;
		LoveUnlikelium = false;
		Magned = false;
		if(Time.timeScale>0) Time.timeScale = 1.0f;
		LevelInfo.Environments.guiPowerUpTime.text = "";
	}
	
	public void StartSureShot()
	{
		StartCoroutine(SureShotThread());
	}

	private float poweruptime = 0f;
	private IEnumerator SureShotThread()
	{
		if(!AutoFire) 
		{
			AutoFire = true;
			LevelInfo.Environments.playerAnimations.OpenTurette();
			yield return new WaitForSeconds(0.5f);
		
			poweruptime = Store.Instance.powerupSureShot.LevelTime;
			while ( poweruptime > 0f )
			{
				TryAutoShot();
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		
			LevelInfo.Environments.guiPowerUpTime.text = "";
			
			LevelInfo.Environments.playerAnimations.CloseTurette();
			AutoFire = false;
		}
		else
			poweruptime = Store.Instance.powerupSureShot.LevelTime;
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
			poweruptime = Store.Instance.powerupFreeze.LevelTime*0.5f;
			while ( poweruptime > 0f )
			{
				if(Time.timeScale>0) Time.timeScale = 0.5f;
				
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		
			LevelInfo.Environments.guiPowerUpTime.text = "";
			FreezeWorld = false;
			Time.timeScale = 1f;
		}
		else
			poweruptime = Store.Instance.powerupFreeze.LevelTime*0.5f;
	}
	
	public void StartIntergalactic()
	{
		StartCoroutine(IntergalacticThread());
	}
	
	private IEnumerator IntergalacticThread()
	{	
		if(!Intergalactic)
		{
			Intergalactic = true;
			LevelInfo.Environments.generator.GenerateAlienShip = false;
			LevelInfo.Environments.generator.GenerateAsteroid = false;
			
			poweruptime = Store.Instance.powerupIntergalactic.LevelTime*0.5f;
			while(poweruptime>0f)
			{
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				yield return null;
			}
			LevelInfo.Environments.guiPowerUpTime.text = "";
			LevelInfo.Environments.generator.GenerateAlienShip = true;
			LevelInfo.Environments.generator.GenerateAsteroid = true;
			Intergalactic = false;
		}
	}	

	public void StartLoveUnlikelium()
	{
		StartCoroutine(LoveUnlikeliumThread());
	}
	
	private IEnumerator LoveUnlikeliumThread()
	{	
		if(!LoveUnlikelium)
		{
			LoveUnlikelium = true;
			LevelInfo.Environments.generator.GenerateAlienShip = false;
			LevelInfo.Environments.generator.GenerateAsteroid = false;
			
			LevelInfo.Environments.generator.StartUnlikeliumGenerator();
			poweruptime = Store.Instance.powerupShazam.LevelTime;
			while(poweruptime>0f)
			{
				poweruptime -= Time.deltaTime;
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				yield return null;
			}
			LevelInfo.Environments.generator.StopUnlikeliumGenerator();
			LevelInfo.Environments.guiPowerUpTime.text = "";
			
			float time = 4f;
			while(time>0f)
			{
				time -= Time.deltaTime;
				yield return null;
			}
			LevelInfo.Environments.generator.DeletaUnlikeliumList();
			
			LevelInfo.Environments.generator.GenerateAlienShip = true;
			LevelInfo.Environments.generator.GenerateAsteroid = true;
			LoveUnlikelium = false;
		}
	}		
		
	
	public void StartMagned()
	{
		StartCoroutine(MagnedThread());
	}
	
	private IEnumerator MagnedThread()
	{
		if(!Magned ) 
		{
			Magned = true;
			poweruptime = Store.Instance.powerupMagned.LevelTime;
			while ( poweruptime > 0f )
			{
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
	
			LevelInfo.Environments.guiPowerUpTime.text = "";
			Magned = false;
		}
		else
			poweruptime = Store.Instance.powerupMagned.LevelTime;
	}
	#endregion
	
	#region Start Update
	
	void Awake()
	{
		attempt++;
	}
	
	// Use this for initialization
	void Start () 
	{	
		touchInput = (TouchInput)GameObject.FindObjectOfType(typeof(TouchInput));
		CameraSetUp();
		
		lastPosition = transform.position;
		
		StartCoroutine(Rise());
	}
	
	// Update is called once per frame
	void Update () 
	{	
		#if UNITY_EDITOR
		
		if( Input.GetKeyUp(KeyCode.PageUp) )
			StartSureShot();
		if( Input.GetKeyUp(KeyCode.PageDown) )
			StartFreezeWorld();
		
		#endif
		
		if(LevelInfo.State.state != GameState.Play ) return;
		
		UpdateHUB();
		
		if(!Ready) return;
		
		
		FireSetUp();	
		RotationSetUp();
		TransformSetUp();
		CameraSetUp();
		ExhaustSetUp();
		SoundSetUp();
	}
	
	#endregion
	
	#region HUB
	
	private void UpdateHUB()
	{
		// calc travelled
		travelled += DistXZ(lastPosition,transform.position);
		lastPosition = transform.position;
		LevelInfo.Environments.guiDistanceTravelled.text = "" + (int)travelled /*+ " ly"*/;
		LevelInfo.Environments.guiUnlikeliums.text = "" + Store.Instance.Unlikeliums + " u";
	}
	
	#endregion
	
	#region Get Ready
	
	[System.NonSerializedAttribute]
	public bool Ready = false;
	
	private bool waitforcalibrate = false;
	
	private IEnumerator Rise()
	{
		float riseTime = LevelInfo.Settings.PlayerWaitForRise;
		while( riseTime > 0 )
		{
			Vector3 pos = transform.position;
			pos.y = riseTime*BeginHeight/LevelInfo.Settings.PlayerWaitForRise;
			transform.position = pos;
			riseTime -= Time.deltaTime;
			if( riseTime <= 0 )
			{
				pos.y = 0;
				transform.position = pos;
				LevelInfo.Environments.generator.GenerateAlienShip = true;
				LevelInfo.Environments.generator.GenerateAsteroid = true;
			}
			yield return new WaitForEndOfFrame();
		}	
		
		if(attempt==1) // First attemp of game launch
		{
			waitforcalibrate = true;
			LevelInfo.Environments.popupCalibrate.SetActive(true);
			Time.timeScale = 0.0f;
		}
		else
			Ready = true;
	}
	
	public void Calibrate(bool vertified)
	{
		calibrate = GameEnvironment.DeviceAngle;
		if(waitforcalibrate && vertified)
		{
			Ready = true;
			waitforcalibrate = false;
			LevelInfo.Environments.popupCalibrate.SetActive(false);
			Time.timeScale = 1.0f;
		}
	}
	
	#endregion
	
	#region Transform
	
	private bool upblock = true;
	private bool downblock = true;
	private float calibrate = 0f;
	float calibratedelta()
	{
		float current = GameEnvironment.DeviceAngle;
		current -= calibrate;
		if(current > 180f) current-=360f;
		if(current < -180f) current+=360f;
		return current;
	}
	
	void TransformSetUp()
	{	
		float current = Mathf.Clamp(calibratedelta(),-30f,30f)*LevelInfo.Settings.MaxSpaceY/-30f;
		var y = transform.position.y;
		
		float uplock = upblock?0.25f:0f;
		
		float downlock = downblock?0.25f:0f;
		
		if( current > y+uplock ) 
		{
			y = Mathf.Min(current,y+Time.deltaTime*LevelInfo.Settings.PlayerUpDownSpeed);
			upblock = false;
			downblock = true;
		}
		if( current < y-downlock ) 
		{
			y = Mathf.Max(current,y-Time.deltaTime*LevelInfo.Settings.PlayerUpDownSpeed);
			downblock = false;
			upblock = true;
		}
		
		transform.position = new Vector3(transform.position.x,y,transform.position.z);
		
		transform.Translate(LevelInfo.Settings.PlayerSpeed*Time.deltaTime*Vector3.forward);	
	}
	
	/*void OnGUI()
	{
		GUI.Label(new Rect(100,100,2000,2000),"ship y: " + transform.position.y + "\nc delta: " + calibratedelta());
	}*/
	
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
			Instantiate(LevelInfo.Environments.prefabPlayerProjectile,LevelInfo.Environments.playerLeftFireTransform.position,Quaternion.Euler(0f,transform.rotation.eulerAngles.y,0f) );
			Instantiate(LevelInfo.Environments.prefabPlayerProjectile,LevelInfo.Environments.playerRightFireTransform.position,Quaternion.Euler(0f,transform.rotation.eulerAngles.y,0f) );
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
				if( DistXZ(g[i].transform.position,transform.position) <= LevelInfo.Settings.SureShotDistance &&
					toscreen.x >= 0 && toscreen.x <= Screen.width && 
					toscreen.y >= 0 && toscreen.y <= Screen.height && toscreen.z > 1f && toscreen.z < minvalue )
				{
					minvalue = toscreen.z;
					index = i;
				}
			}
			if(index != -1)
			{
				/*Vector3 midpos = 0.5f*(leftturret.transform.position+rightturret.transform.position);
				GameObject j1 = (GameObject)Instantiate(BulletYellow,midpos,Quaternion.identity );
				GameObject j2 = (GameObject)Instantiate(BulletYellow,midpos,Quaternion.identity );
				j1.transform.LookAt(g[index].transform);
				j2.transform.LookAt(g[index].transform);
				j1.transform.position = leftturret.transform.position;
				j2.transform.position = rightturret.transform.position;
				j1.SendMessage("ToTarget",g[index]);
				j2.SendMessage("ToTarget",g[index]);
				g[index].SendMessage("EnableTargetingBox");*/
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(AudioFire);
				GameObject bullet = (GameObject)Instantiate(LevelInfo.Environments.prefabPlayerAutoFireProjectile,LevelInfo.Environments.playerAutoFireTransform.position,Quaternion.Euler(0f,transform.rotation.eulerAngles.y,0f) );
				bullet.transform.LookAt(g[index].transform);
				bullet.GetComponent<Bullet>().ToTarget(g[index]);
				g[index].SendMessage("EnableTargetingBox");
			}
			
			autofireDeltaTime = 0.1f;	
		}
	}
	
	void FireSetUp()
	{
		if( Intergalactic ) return;
		
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
		bool reduce=false;
		float currentspeed = ExhaustArray[0].startSpeed;
		
		if( Intergalactic )
		{
			currentspeed += 60f*Time.deltaTime;
			LevelInfo.Audio.audioSourcePlayerShip.clip = AudioEngineBoost;		
		}
		else if( LoveUnlikelium || (touchInput.B  && !LevelInfo.Environments.fuelOverheat.Up()) )
		{
			currentspeed += 30f*Time.deltaTime;
			LevelInfo.Audio.audioSourcePlayerShip.clip = AudioEngineBoost;
		}
		else
		{
			LevelInfo.Environments.fuelOverheat.Down();
			currentspeed -= 30f*Time.deltaTime;
			LevelInfo.Audio.audioSourcePlayerShip.clip = AudioEngineNormal;
			reduce=true;
		}
		if( !LevelInfo.Audio.audioSourcePlayerShip.isPlaying ) LevelInfo.Audio.audioSourcePlayerShip.Play();
		
		if(reduce)
		{ 
			currentspeed = Mathf.Clamp(currentspeed,1f,float.PositiveInfinity);
		}
		else
		{
			if(Intergalactic) currentspeed = Mathf.Clamp(currentspeed,1f,50f);
			else currentspeed = Mathf.Clamp(currentspeed,1f,LoveUnlikelium?8f:6f);
		}
		
		foreach(ParticleSystem e in ExhaustArray)
			e.startSpeed = currentspeed;
		float delta = currentspeed-lastexhaust;
		lastexhaust = currentspeed;
		
		transform.Translate(Vector3.forward*delta);
		LevelInfo.Settings.PlayerSpeed += 4*delta;
	}
	
	void SoundSetUp()
	{
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
	
	#region Helpful
	
	private float DistXZ(Vector3 a,Vector3 b)
	{
		a.y=b.y=0;
		return Vector3.Distance(a,b);
	}
	
	public float DistXZ(Vector3 pos)
	{
		var a = transform.position;
		a.y=pos.y=0;
		return Vector3.Distance(a,pos);
	}
	
	#endregion
}
