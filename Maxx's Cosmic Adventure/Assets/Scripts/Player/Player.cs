using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	#region Parameters
	
	public float FireDeltaTime;
	
	public float BeginHeight = -3f;
	
	public float CameraZ = 9.5f; 
	public float CameraHeight = 1.5f;
	
	public ParticleSystem[] ExhaustArray;
	
	public GameObject leftturret,rightturret;
	
	public static int localAttempt = 0;
	
	[System.NonSerializedAttribute]
	public int allAttempt = 0;

	#endregion
	
	#region Variables
	
	private float fireDeltaTime = 0.0f;
	private float autofireDeltaTime = 0.0f;
	
	[System.NonSerializedAttribute]
	public float travelled = 0;
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
	
	#region Awake, Start, Update
	
	void Awake()
	{	
		#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
		if(localAttempt==0) calibrate = 45;
		#endif
		
		// Change/Save attemps game played
		localAttempt++;
		allAttempt = PlayerPrefs.GetInt("all_game_attempt_count",0);
		allAttempt++;
		PlayerPrefs.SetInt("all_game_attempt_count",allAttempt);
	}
	
	// Use this for initialization
	void Start () 
	{	
		CameraSetUp();
		
		lastPosition = transform.position;
		
		StartCoroutine("Rise");
	}
	
	void Update () 
	{	
		#if UNITY_EDITOR
		if( Input.GetKeyUp(KeyCode.PageUp) )
			StartSureShot();
		if( Input.GetKeyUp(KeyCode.PageDown) )
			StartFreezeWorld();
		if( Input.GetKeyUp(KeyCode.Home) )
			StartIntergalactic();
		if( Input.GetKeyUp(KeyCode.End) )
			StartLoveUnlikelium();
		
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
		ExhaustSetUp();
		CameraSetUp();
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
	
	private IEnumerator Rise()
	{
		LevelInfo.Environments.gameStartTimer.gameObject.SetActive(true);
		
		if(Store.Instance.powerupHeadStart.level>0)	
			StartCoroutine(EnableHeadStartButton());
		
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
		
		if(!useHeadStart)
			StartCoroutine(DisalbeHeadStartButton());
		
		if( PlayerPrefs.GetInt("tutorials",0)>=0)
		{
			LevelInfo.Environments.gameStartTimer.gameObject.SetActive(false);
			LevelInfo.Environments.popupStartTrainings.SetActive(true);
			Time.timeScale = 0.0f;
		}
		else
		{
			LevelInfo.Environments.gameStartTimer.text = "GO!";
			yield return new WaitForSeconds(0.05f);
			Ready = true;
			if(useHeadStart)
			{
				Store.Instance.powerupHeadStart.level--;
				StartCoroutine("StartHeadStartThread");
			}
			yield return new WaitForSeconds(1.5f);
			LevelInfo.Environments.gameStartTimer.gameObject.SetActive(false);
		}	
	}
	
	private bool useHeadStart = false;
	public void UseHeadStart()
	{
		if( !useHeadStart )
		{
			useHeadStart = true;
			StartCoroutine(DisalbeHeadStartButton());
		}
	}
	
	private IEnumerator StartHeadStartThread()
	{
		LevelInfo.Environments.generator.GenerateAlienShip = false;
		LevelInfo.Environments.generator.GenerateAsteroid = false;
		Intergalactic = true;
		intergalacticLocal = true;
		//_invincibility++;
		
		float poweruptime = 5f;
		while(poweruptime>0f)
		{
			poweruptime -= Time.deltaTime;
			yield return null;
		}
		
		intergalacticLocal = false;
		yield return new WaitForSeconds(1f);
		Intergalactic = false;
		LevelInfo.Environments.generator.GenerateAlienShip = true;
		LevelInfo.Environments.generator.GenerateAsteroid = true;
		LevelInfo.Environments.generator.ResetSpawnDeltaTime();
		//_invincibility--;
	}
	
	IEnumerator EnableHeadStartButton()
	{
		LevelInfo.Environments.popupHeadStart.transform.localPosition = new Vector3(-300f,-105.5f,0f);
		LevelInfo.Environments.popupHeadStart.SetActive(true);
		
		float time = 1.0f;
		while(time>0f)
		{
			time -= Time.deltaTime;
			LevelInfo.Environments.popupHeadStart.transform.Translate(0f,Time.deltaTime*0.5f,0f);
			yield return null;
		}
	}	
	
	IEnumerator DisalbeHeadStartButton()
	{
		float time = 1.0f;
		while(time>0f)
		{
			time -= Time.deltaTime;
			LevelInfo.Environments.popupHeadStart.transform.Translate(0f,-Time.deltaTime*0.5f,0f);
			yield return null;
		}
		LevelInfo.Environments.popupHeadStart.SetActive(false);
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
		if(vertified)
		{
			LevelInfo.Environments.popupCalibrate.SetActive(false);
			GetReady();
			LevelInfo.Environments.tutorials.StartTrainings();
		}
	}
	
	public void GetReady()
	{
		Ready = true;
		Time.timeScale = 1.0f;		
	}
	
	void UpdateShip()
	{	
		if(Options.Instance.flightControls3D)
		{
			// Up/Down Tilting
			float ytilt = calibratedelta(); if(Options.Instance.yInvert) ytilt =- ytilt;
			
			// Rotate Y
			float maxrotateangle = 15f;
			float current = Mathf.Clamp(ytilt,-30f,30f)*0.5f;
			var rot = transform.rotation.eulerAngles;
			if( rot.x > 180.0f ) rot.x -= 360.0f;
			if( Mathf.Abs(rot.x-current) > LevelInfo.Settings.PlayerRotateIgnore)
			{
				rot.x = Mathf.SmoothStep(rot.x,current,LevelInfo.Settings.PlayerRotateSpeed);
				rot.x = Mathf.Clamp(rot.x,-maxrotateangle,maxrotateangle);
				transform.rotation = Quaternion.Euler(rot);
			}
			
			// Rotation by tilt
			current = -GameEnvironment.InputAxis.x*90f;
			rot = transform.rotation.eulerAngles;
			if( rot.z > 180.0f ) rot.z -= 360.0f;
			
			if( Mathf.Abs(rot.z-current) > LevelInfo.Settings.PlayerRotateIgnore)
			{
				rot.z = Mathf.SmoothStep(rot.z,current,LevelInfo.Settings.PlayerRotateSpeed);
				rot.z = Mathf.Clamp(rot.z,-LevelInfo.Settings.PlayerRotationMaxAngle,LevelInfo.Settings.PlayerRotationMaxAngle);
				rot.y -= rot.z*0.5f*Mathf.PI*Time.deltaTime;
				transform.rotation = Quaternion.Euler(rot);
			}
		}
		else
		{
			// Up/Down Tilting
			float ytilt = calibratedelta(); if(Options.Instance.yInvert) ytilt =- ytilt;
			float current = Mathf.Clamp(ytilt,-30f,30f)*LevelInfo.Settings.MaxSpaceY/-30f;
			var y = transform.position.y;
			
			if( Mathf.Abs(y-current) > LevelInfo.Settings.PlayerUpDownIgnore )
			{
				y = Mathf.SmoothStep(y,current,LevelInfo.Settings.PlayerUpDownSpeed);
				transform.position = new Vector3(transform.position.x,y,transform.position.z);
			}
			
			// Rotation by tilt
			current = -GameEnvironment.InputAxis.x*90f;
			var rot = transform.rotation.eulerAngles; rot.x=0;
			if( rot.z > 180.0f ) rot.z -= 360.0f;
			
			if( Mathf.Abs(rot.z-current) > LevelInfo.Settings.PlayerRotateIgnore)
			{
				rot.z = Mathf.SmoothStep(rot.z,current,LevelInfo.Settings.PlayerRotateSpeed);
				rot.z = Mathf.Clamp(rot.z,-LevelInfo.Settings.PlayerRotationMaxAngle,LevelInfo.Settings.PlayerRotationMaxAngle);
				rot.y -= rot.z*0.5f*Mathf.PI*Time.deltaTime;
				transform.rotation = Quaternion.Euler(rot);
			}
		}
		// Ship moving
		float speed = LevelInfo.Settings.PlayerSpeed;
		if(BeastieBoost) speed*=2f;
		transform.Translate(speed*Time.deltaTime*Vector3.forward);
	}
	
	public void LostLifeSmoke(int stayedLives)
	{
		StartCoroutine(LostLifeSmokeThread(stayedLives));
	}
	
	private IEnumerator LostLifeSmokeThread(int stayedLives)
	{
		ParticleSystem particle = LevelInfo.Environments.playerLostLifeSmoke;
		if(!particle.enableEmission)
		{
			particle.enableEmission = true;
			_invincibility++;
			
			if(stayedLives>=4)
			{
				yield return new WaitForSeconds(1.0f);
			}
			if(stayedLives==3)
			{
				yield return new WaitForSeconds(1.5f);
			}
			else if(stayedLives==2)
			{
				yield return new WaitForSeconds(2.0f);				
			}
			else if(stayedLives==1)
			{
				yield return new WaitForSeconds(1.5f);
				particle.enableEmission = false;		
				yield return new WaitForSeconds(0.5f);
				particle.enableEmission = true;
				yield return new WaitForSeconds(1.0f);
			}

			_invincibility--;
			particle.enableEmission = false;
		}
	}
	
	#endregion
	
	#region Firing

	void TryStandardShot(bool effectOverheat)
	{
		if( fireDeltaTime > 0f ) fireDeltaTime -= FreezeMultiply*Time.deltaTime;
		bool ovh = LevelInfo.Environments.fireOverheat.Overheated;
		if(ovh) LevelInfo.Environments.tutorials.FireOverheated();
		if( effectOverheat ) LevelInfo.Environments.fireOverheat.Up();
		if( !ovh && LevelInfo.Environments.fireOverheat.Overheated ) {/*PLevelInfo.Audio.PlayAudioWeaponExpire();*/}
		if( fireDeltaTime <= 0 && (!effectOverheat || !LevelInfo.Environments.fireOverheat.Overheated) )
		{
			LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Audio.clipFire,0.4f);
			Instantiate(LevelInfo.Environments.prefabPlayerProjectile,LevelInfo.Environments.playerLeftFireTransform.position,transform.rotation );
			Instantiate(LevelInfo.Environments.prefabPlayerProjectile,LevelInfo.Environments.playerRightFireTransform.position,transform.rotation );
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
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Audio.clipAutoFire);
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
		if(Intergalactic) return;
		
		// Standart shot
		if(FireButtonPressed)
			TryStandardShot(true);
		else
			LevelInfo.Environments.fireOverheat.Down();
	}
	
	#endregion
	
	#region Camera
	
	void CameraSetUp()
	{	
		float boost = lastboost;
		if(boost<1f) boost = 1f-0.25f*(1f-boost);
		
		transform.Translate(-Vector3.forward*boost);
		
		if(Options.Instance.flightControls3D)
		{
			LevelInfo.Environments.mainCamera.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			LevelInfo.Environments.mainCamera.transform.rotation = transform.rotation;
		}
		else
		{
			LevelInfo.Environments.mainCamera.transform.position = new Vector3(transform.position.x,0f,transform.position.z);
			LevelInfo.Environments.mainCamera.transform.rotation = transform.rotation;		
		}
		
		if( Options.Instance.cameraRotation)
		{
			Vector3 crot = LevelInfo.Environments.mainCamera.transform.rotation.eulerAngles;
			if(crot.z>180f) crot.z -= 360f;
			crot.z = 0.75f*crot.z;
			if(crot.x>180f) crot.x -= 360f;
			crot.x = 0.75f*crot.x;
			crot.x += 20f;
			LevelInfo.Environments.mainCamera.transform.rotation = Quaternion.Euler(crot);
		}
		else
		{
			Vector3 crot = LevelInfo.Environments.mainCamera.transform.rotation.eulerAngles;
			crot.z = 0.0f;
			crot.x = 20.0f;
			LevelInfo.Environments.mainCamera.transform.rotation = Quaternion.Euler(crot);		
		}
		
		LevelInfo.Environments.mainCamera.transform.Translate(-Vector3.forward*CameraZ);
		LevelInfo.Environments.mainCamera.transform.Translate(0,CameraHeight,0);	
		
		transform.Translate(Vector3.forward*boost);
	}
	
	#endregion
	
	#region Exhaust
	
	float boostx=0f;
	float lastboost = 1f;
	void ExhaustSetUp()
	{	
		bool boostend = true;
		bool reduce=false;
		bool brake = LevelInfo.Environments.maximumSwipeDown.Value>=0.1f;
		float currentspeed = lastboost;
		
		if( intergalacticLocal )
		{
			currentspeed += 60f*Time.deltaTime;
			LevelInfo.Audio.audioSourcePlayerShipEngine.clip = LevelInfo.Audio.clipPlayerShipEngineBoost;	
		}
		else if(LoveUnlikelium)
		{
			currentspeed += 30f*Time.deltaTime;
			LevelInfo.Audio.audioSourcePlayerShipEngine.clip = LevelInfo.Audio.clipPlayerShipEngineBoost;
		}
		else if(BoostButtonPressed && !LevelInfo.Environments.fuelOverheat.Up() && currentspeed<=6f)
		{
			boostend = false;
			boostx =  Mathf.Min(2f,boostx+0.5f*Time.deltaTime);
			currentspeed += 30f*Time.deltaTime;
			LevelInfo.Audio.audioSourcePlayerShipEngine.clip = LevelInfo.Audio.clipPlayerShipEngineBoost;		
		}
		else
		{
			if(currentspeed<1f&&!brake) currentspeed = Mathf.Min(1f,currentspeed+30*Time.deltaTime);
			if(currentspeed>1f) currentspeed = Mathf.Max(1f,currentspeed-30f*Time.deltaTime);
			
			LevelInfo.Environments.fuelOverheat.Down();
			LevelInfo.Audio.audioSourcePlayerShipEngine.clip = LevelInfo.Audio.clipPlayerShipEngineIdle;
			reduce=true;
		}
		
		if(LevelInfo.Environments.fuelOverheat.Overheated)
			LevelInfo.Environments.tutorials.BoostOverheated();
		
		if(boostend) boostx = Mathf.Max(0f,boostx-Time.deltaTime);
		if( !LevelInfo.Audio.audioSourcePlayerShipEngine.isPlaying ) LevelInfo.Audio.audioSourcePlayerShipEngine.Play();
		
		if(reduce)
		{ 
			if(currentspeed<=1f&&brake)
			{	
				float vl = 12f*Mathf.Min(LevelInfo.Environments.maximumSwipeDown.Value-0.1f,0.3f);
				
				currentspeed -= 5*Time.deltaTime;	
				currentspeed = Mathf.Max(1f - vl,currentspeed);
			}
		}
		else
		{
			if(intergalacticLocal) currentspeed = Mathf.Clamp(currentspeed,1f,50f);
			else currentspeed = Mathf.Clamp(currentspeed,1f,LoveUnlikelium?8f:6f);
		}
		
		foreach(ParticleSystem e in ExhaustArray)
			e.startSpeed = Mathf.Max(1f,lastboost);
		float delta = currentspeed-lastboost;
		lastboost = currentspeed;
		
		transform.Translate(Vector3.forward*boostx*30f*Time.deltaTime);
		LevelInfo.Settings.PlayerSpeed += 5*delta;
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
	public float FreezeMultiply{ get{ return FreezeWorld?2f:1f;} }
	
	[System.NonSerializedAttribute]
	public bool Intergalactic = false;
	private bool intergalacticLocal = false;
	
	[System.NonSerializedAttribute]
	public bool LoveUnlikelium = false;
	
	[System.NonSerializedAttribute]
	public bool Magned = false;
	
	[System.NonSerializedAttribute]
	public bool BeastieBoost = false;
	
	public void ClearAllPowerups()
	{
		StopCoroutine("SureShotThread");
		StopCoroutine("FreezeWorldThread");
		StopCoroutine("IntergalacticThread");
		StopCoroutine("LoveUnlikeliumThread");
		StopCoroutine("MagnedThread");
		StopCoroutine("BeastieBoost");
		
		AutoFire = false;
		LevelInfo.Environments.playerAnimations.CloseTurette();
		LevelInfo.Environments.guiPowerupCountDown.fillAmount = 0;
		
		FreezeWorld = false;
		//RenderSettings.ambientLight = ambientStandard;
		
		//intergalacticLocal = false;
		Intergalactic = false;
		
		LoveUnlikelium = false;
		LevelInfo.Environments.generator.StopUnlikeliumGenerator();
		
		Magned = false;
		
		BeastieBoost = false;
		
		_invincibility = 0;
		
		if(Time.timeScale>0) Time.timeScale = 1.0f;
		LevelInfo.Environments.guiPowerUpTime.text = "";
	}
	
	private void SetPowerupFillAmount(float cur,float all)
	{
		LevelInfo.Environments.guiPowerupCountDown.fillAmount = Mathf.Clamp01(cur/all);
	}
	
	public void StartSureShot()
	{
		StartCoroutine("SureShotThread");
	}

	private float poweruptime = 0f;
	private IEnumerator SureShotThread()
	{
		if(!AutoFire) 
		{
			ClearAllPowerups();
			AutoFire = true;
			
			poweruptime = Store.Instance.powerupSureShot.TimedLevelTime;
			LevelInfo.Environments.guiPowerupCountDown.color = new Color(0,1f,0,1f);
			
			LevelInfo.Environments.playerAnimations.OpenTurette();
			yield return new WaitForSeconds(0.5f);
		
			
			while ( poweruptime > 0f )
			{
				TryAutoShot();
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				SetPowerupFillAmount(poweruptime,Store.Instance.powerupSureShot.TimedLevelTime);
				yield return new WaitForEndOfFrame();
			}
		
			LevelInfo.Environments.guiPowerUpTime.text = "";
			
			LevelInfo.Environments.playerAnimations.CloseTurette();
			AutoFire = false;
		}
		else
			poweruptime = Store.Instance.powerupSureShot.TimedLevelTime;
	}
	
	public void StartFreezeWorld()
	{
		StartCoroutine("FreezeWorldThread");
	}
	
	//private readonly Color ambientStandard = new Color(101f/255f,101f/255f,101f/255f,1f);
	//private readonly Color ambientFreezeWorld = new Color(0f,0.6f,1f,1f);
	//private readonly Color ambientFreezeWorld = new Color(0.8f,0.8f,0.8f,1f);
	
	private IEnumerator FreezeWorldThread()
	{
		if(!FreezeWorld ) 
		{
			ClearAllPowerups();
			FreezeWorld = true;
			//RenderSettings.ambientLight = ambientFreezeWorld;
			poweruptime = Store.Instance.powerupFreeze.TimedLevelTime*0.5f;
			LevelInfo.Environments.guiPowerupCountDown.color = new Color(0,1f,1f,1f);
			while ( poweruptime > 0f )
			{
				if(Time.timeScale>0) Time.timeScale = 0.5f;
				
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				SetPowerupFillAmount(poweruptime,Store.Instance.powerupFreeze.TimedLevelTime*0.5f);
				yield return new WaitForEndOfFrame();
			}
		
			LevelInfo.Environments.guiPowerUpTime.text = "";
			FreezeWorld = false;
			//RenderSettings.ambientLight = ambientStandard;
			Time.timeScale = 1f;
		}
		else
			poweruptime = Store.Instance.powerupFreeze.TimedLevelTime*0.5f;
	}
	
	public void StartIntergalactic()
	{
		StartCoroutine("IntergalacticThread");
	}
	
	private IEnumerator IntergalacticThread()
	{	
		if(!Intergalactic)
		{
			ClearAllPowerups();
			Intergalactic = true;
			intergalacticLocal = true;
			//_invincibility++;
			LevelInfo.Environments.generator.GenerateAlienShip = false;
			LevelInfo.Environments.generator.GenerateAsteroid = false;
			
			poweruptime = Store.Instance.powerupIntergalactic.TimedLevelTime*0.5f;
			LevelInfo.Environments.guiPowerupCountDown.color = new Color(0.1f,0.1f,0.1f,1f);
			while(poweruptime>0f)
			{
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				SetPowerupFillAmount(poweruptime,Store.Instance.powerupIntergalactic.TimedLevelTime*0.5f);
				yield return null;
			}
			LevelInfo.Environments.guiPowerUpTime.text = "";
			intergalacticLocal = false;
			yield return new WaitForSeconds(1f);
			Intergalactic = false;
			LevelInfo.Environments.generator.GenerateAlienShip = true;
			LevelInfo.Environments.generator.GenerateAsteroid = true;
			LevelInfo.Environments.generator.ResetSpawnDeltaTime();
			//_invincibility--;
		}
	}	

	public void StartLoveUnlikelium()
	{
		StartCoroutine("LoveUnlikeliumThread");
	}
	
	private IEnumerator LoveUnlikeliumThread()
	{	
		if(!LoveUnlikelium)
		{
			ClearAllPowerups();
			LoveUnlikelium = true;
			//_invincibility++;
			//LevelInfo.Environments.generator.GenerateAlienShip = false;
			//LevelInfo.Environments.generator.GenerateAsteroid = false;
			
			//LevelInfo.Environments.generator.StartUnlikeliumGenerator();
			//poweruptime = 10f+(float)Store.Instance.powerupShazam.level;
			
			poweruptime = Store.Instance.powerupShazam.TimedLevelTime;
			LevelInfo.Environments.generator.StartShazam();
			
			LevelInfo.Environments.guiPowerupCountDown.color = new Color(1f,0.5f,0,1f);
			while(poweruptime>0f)
			{
				poweruptime -= Time.deltaTime;
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				SetPowerupFillAmount(poweruptime,Store.Instance.powerupShazam.TimedLevelTime);
				yield return null;
			}
			LevelInfo.Environments.generator.EndShazam();
			//LevelInfo.Environments.generator.StopUnlikeliumGenerator();
			LevelInfo.Environments.guiPowerUpTime.text = "";
			
			//float time = 4f;
			//while(time>0f)
			//{
			//	time -= Time.deltaTime;
			//	yield return null;
			//}
			//LevelInfo.Environments.generator.DeletaUnlikeliumList();
			
			//LevelInfo.Environments.generator.GenerateAlienShip = true;
			//LevelInfo.Environments.generator.GenerateAsteroid = true;
			//_invincibility--;
			LoveUnlikelium = false;
		}
		else
			poweruptime = Store.Instance.powerupShazam.TimedLevelTime;
	}		
	
	public void StartMagned()
	{
		StartCoroutine("MagnedThread");
	}
	
	private IEnumerator MagnedThread()
	{
		if(!Magned ) 
		{
			ClearAllPowerups();
			Magned = true;
			poweruptime = Store.Instance.powerupMagned.TimedLevelTime;
			LevelInfo.Environments.guiPowerupCountDown.color = new Color(0.33f,0.1f,0.54f,1f);
			while ( poweruptime > 0f )
			{
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				SetPowerupFillAmount(poweruptime,Store.Instance.powerupMagned.TimedLevelTime);
				yield return new WaitForEndOfFrame();
			}
	
			LevelInfo.Environments.guiPowerUpTime.text = "";
			Magned = false;
		}
		else
			poweruptime = Store.Instance.powerupMagned.TimedLevelTime;
	}
	
	public void StartBeastieBoost()
	{
		StartCoroutine("BeastieBoostThread");
	}
	private IEnumerator BeastieBoostThread()
	{
		if(!BeastieBoost ) 
		{
			ClearAllPowerups();
			BeastieBoost = true;
			poweruptime = Store.Instance.powerupBeastieBoost.TimedLevelTime;
			LevelInfo.Environments.guiPowerupCountDown.color = new Color(0.937f,0.5f,0.5f,1f);
			while ( poweruptime > 0f )
			{
				LevelInfo.Environments.guiPowerUpTime.text = "" + Mathf.CeilToInt(poweruptime);
				poweruptime -= Time.deltaTime;
				SetPowerupFillAmount(poweruptime,Store.Instance.powerupBeastieBoost.TimedLevelTime);
				yield return new WaitForEndOfFrame();
			}
	
			LevelInfo.Environments.guiPowerUpTime.text = "";
			BeastieBoost = false;
		}
		else
			poweruptime = Store.Instance.powerupBeastieBoost.TimedLevelTime;
	}
	#endregion
	
	#region Properties
	
	// Distance
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
	
	// Buttons
	bool BoostButtonPressed{ get{ return LevelInfo.Environments.bButton.isDown||Input.GetKey(KeyCode.B); }}
	bool FireButtonPressed{ get{ return LevelInfo.Environments.fireLeftButton.isDown||LevelInfo.Environments.fireRightButton.isDown||Input.GetKey(KeyCode.F); }}
	
	public bool IsBoost{ get { return BoostButtonPressed; }}
	
	#endregion
}
