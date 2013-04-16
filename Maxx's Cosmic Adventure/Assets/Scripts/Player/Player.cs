using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	#region Parameters
	
	public float FireDeltaTime;
	
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
	
	private int lastmeter = 0;
	private float _travelled = 0;
	private float travelled{
		get{
			return _travelled;
		}
		set{
			_travelled = value;
			int meter = (int)_travelled;
			if(meter > lastmeter )
			{
				int report = (meter-lastmeter)*LevelInfo.Settings.scoreDistanceMultiply;
				LevelInfo.Environments.score.score += report;
				lastmeter = meter;
			}
		}
	}
	public float DistanceTravelled { get { return travelled; }}
	private int lastunlikeliums = 0;
	private int _unlikeliums = 0;
	public int unlikeliums{
		get{
			return _unlikeliums;
		}
		set{
			_unlikeliums = value;
			if(_unlikeliums > lastunlikeliums )
			{
				Store.Instance.Unlikeliums += _unlikeliums-lastunlikeliums;
				lastunlikeliums = _unlikeliums;
			}
		}
	}
	
	
	private Vector3 lastPosition;
	
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
	
	void Update () 
	{	
		#if UNITY_EDITOR
		if( Input.GetKeyUp(KeyCode.PageUp) )
			StartSureShot();
		if( Input.GetKeyUp(KeyCode.PageDown) )
			StartFreezeWorld();
		
		if( Input.GetKeyUp(KeyCode.KeypadPlus) )
			LevelInfo.Environments.score.AddLive();
		if( Input.GetKeyUp(KeyCode.KeypadMinus) )
			LevelInfo.Environments.score.LostLive();
		#endif
		
		if(LevelInfo.State.state != GameState.Play ) return;
		
		UpdateHUB();
		
		if(!Ready) return;
		
		UpdateShip();
		
		FireSetUp();	
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
		LevelInfo.Environments.guiUnlikeliums.text = "" + unlikeliums;
	}
	
	#endregion
	
	#region Rising, Get Ready
	
	[System.NonSerializedAttribute]
	public bool Ready = false;
	
	private bool waitforcalibrate = false;
	
	private IEnumerator Rise()
	{
		LevelInfo.Environments.gameStartTimer.gameObject.SetActive(true);
		float riseTime = LevelInfo.Settings.PlayerWaitForRise;
		while( riseTime > 0 )
		{
			LevelInfo.Environments.gameStartTimer.text = "Get Ready!\n" + Mathf.Min(5,(int)(5f*riseTime/LevelInfo.Settings.PlayerWaitForRise+1f));
			
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
		
		if( PlayerPrefs.GetInt("first_play",0)==0)
		{
			PlayerPrefs.SetInt("first_play",1);
			waitforcalibrate = true;
			LevelInfo.Environments.popupCalibrate.SetActive(true);
			Time.timeScale = 0.0f;
		}
		else
		{
			LevelInfo.Environments.gameStartTimer.text = "GO!";
			yield return new WaitForSeconds(0.05f);
			Ready = true;
			yield return new WaitForSeconds(1.5f);
		}
		
		LevelInfo.Environments.gameStartTimer.gameObject.SetActive(false);
	}
	
	#endregion
	
	#region Maxx Ship Control
	
	private static float calibrate = 0f;
	private float calibratedelta()
	{
		float current = GameEnvironment.DeviceAngle;
		current -= calibrate;
		if(current > 180f) current-=360f;
		if(current < -180f) current+=360f;
		return current;
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
	
	void UpdateShip()
	{	
		// Up/Down Tilting
		float current = Mathf.Clamp(calibratedelta(),-30f,30f)*LevelInfo.Settings.MaxSpaceY/-30f;
		var y = transform.position.y;
		
		if( Mathf.Abs(y-current) > LevelInfo.Settings.PlayerUpDownIgnore )
		{
			y = Mathf.SmoothStep(y,current,LevelInfo.Settings.PlayerUpDownSpeed);
			transform.position = new Vector3(transform.position.x,y,transform.position.z);
		}
		
		// Rotation by tilt
		current = -GameEnvironment.InputAxis.x*90f;
		var rot = transform.rotation.eulerAngles;
		if( rot.z > 180.0f ) rot.z -= 360.0f;
		
		if( Mathf.Abs(rot.z-current) > LevelInfo.Settings.PlayerRotateIgnore)
		{
			rot.z = Mathf.SmoothStep(rot.z,current,LevelInfo.Settings.PlayerRotateSpeed);
			rot.z = Mathf.Clamp(rot.z,-LevelInfo.Settings.PlayerRotationMaxAngle,LevelInfo.Settings.PlayerRotationMaxAngle);
			rot.y -= rot.z*0.5f*Mathf.PI*Time.deltaTime;
			transform.rotation = Quaternion.Euler(rot);
		}
		
		// Ship moving
		transform.Translate(LevelInfo.Settings.PlayerSpeed*Time.deltaTime*Vector3.forward);
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
	}
	
	#endregion
	
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
	
	#region PowerUps
	
	private int _invincibility = 0;
	public bool Invincibility{
		get{
			return _invincibility>0;
		}
	}
	
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
		LevelInfo.Environments.playerAnimations.CloseTurette();
		
		FreezeWorld = false;
		Intergalactic = false;
		LoveUnlikelium = false;
		Magned = false;
		_invincibility = 0;
		
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
			_invincibility++;
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
			_invincibility--;
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
			_invincibility++;
			LevelInfo.Environments.generator.GenerateAlienShip = false;
			LevelInfo.Environments.generator.GenerateAsteroid = false;
			
			LevelInfo.Environments.generator.StartUnlikeliumGenerator();
			poweruptime = 10f+(float)Store.Instance.powerupShazam.level;
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
			_invincibility--;
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
