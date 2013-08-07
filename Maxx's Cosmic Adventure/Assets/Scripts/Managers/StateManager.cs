using UnityEngine;
using System.Collections;

#region Main Enumerations

public enum GameState
{
	Play,
	Paused,
	Options,
	Store,
	Stats,
	Lose,
	None
}

public enum Gems
{
	None,
	Unlikelium,
	SureShot,
	Magnet,
	In3s,
	FreezeWorld,
	Pow,
	FireBall,
	LoveUnlikelium,
	Intergalactic,
	ToughGuy,
	Lazer
}

public enum Jeebie
{
	BlueFighterPilot=0,
	BlueLeader=1,
	RedKamikaze=2,
	TripleKamikaze=3,
	PurpleFigher=4,
	Reactive=5
}

public enum Asteroids
{
	White,
	Blue,
	Green,
	Red,
	Yellow,
	Purple
}

public enum Projectile
{
	Standard,
	AutoFire,
	Fireball,
	Missie,
	Laser,
	None
}

#endregion

public class StateManager : MonoBehaviour {
	
	#region game Parameters
	
	#endregion
	
	private GameState _state = GameState.None;
	public GameState state{
		get
		{
			return _state;
		}
		set
		{
			GameState last = _state;
			_state = value;
			
			LevelInfo.Environments.HUB.SetActive(HUBActiveHelper(_state));
			
			if(last == GameState.Store && LevelInfo.Environments.score.Lose)
			{
				if(Dying)
				{
					_state = GameState.Play;
					Time.timeScale = 1f;
				}
				else
				{
					_state = GameState.Lose;
				}
				return;
			}
			
			switch(_state)
			{
			case GameState.Play:
				LevelInfo.Audio.ResumeMusic();
				break;
			case GameState.Paused:
				LevelInfo.Audio.StopEffects();
				LevelInfo.Audio.PauseMusic();
				break;
			case GameState.Store:
				LevelInfo.Audio.StopEffects();
				LevelInfo.Audio.PauseMusic();
				LevelInfo.Environments.HUB.SetActive(false);
				Store.Instance.ShowStore = true;
				break;
			case GameState.Options:
				LevelInfo.Environments.HUB.SetActive(false);
				Options.Instance.ShowOptions = true;
				break;
			case GameState.Stats:
				LevelInfo.Environments.HUB.SetActive(false);
				Stats.Instance.ShowStats = true;
				break;
			case GameState.Lose:				
				StartCoroutine(ShowGameOverScreenThread());
				break;
			}
			
			Time.timeScale = (state == GameState.Play?1f:0f);
		}
	}
	
	public void StartDying()
	{
		StartCoroutine(DyingThread());
	}
	
	[System.NonSerializedAttribute]
	public bool Dying = false;
	
	private IEnumerator DyingThread()
	{
		Dying = true;
	
		LevelInfo.Audio.StopEffects();
		LevelInfo.Audio.PauseMusic();
		LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Audio.clipGameOver);
		
		LevelInfo.Environments.popupDying.SetActive(true);
			
		float time=8f;
		while(Dying && time>0f)
		{
			time -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		DyingToLose();
	}
	
	private void DyingToLose()
	{
		if(Dying)
		{
			Dying = false;
			LevelInfo.Environments.popupDying.SetActive(false);
			state = GameState.Lose;
		}
	}
	
	public void KeepFighting()
	{
		Dying = false;
		LevelInfo.Environments.popupDying.SetActive(false);
		LevelInfo.Environments.score.AddShield(0.2f);
		
		LevelInfo.Audio.StopEffects();
		LevelInfo.Audio.ResumeMusic();
	}
	
	private IEnumerator ShowGameOverScreenThread()
	{
		yield return StartCoroutine(ShowMissionsComplete());
		
		Stats.Instance.ClearInSingleRunMissions();
		
		int distance = Mathf.FloorToInt(LevelInfo.Environments.playerShip.DistanceTravelled);
		int highscore = PlayerPrefs.GetInt("high_score",0);
		
		LevelInfo.Audio.StopEffects();
		LevelInfo.Audio.PauseMusic();
		
		LevelInfo.Environments.playerShip.ClearAllPowerups();
		
		foreach(var r in LevelInfo.Environments.playerShip.ExhaustArray )
			r.enableEmission = false;
		
		LevelInfo.Environments.popupLose.SetActive(true);
		var results = LevelInfo.Environments.popupLoseLabelResults;
		results.text = 
			"Distance Traveled: " + distance +
			"\nUnlikelium Collected: " + LevelInfo.Environments.score.unlikeliumsCollected +
			"\nAsteroids Destroyed: " + LevelInfo.Environments.score.asteroidsDestoyed +
			"\nJeebies Defeated: " + LevelInfo.Environments.score.jeebiesDestoyed + 
			"\n\nScore: " + LevelInfo.Environments.score.totalScore;
		
		LevelInfo.Environments.popupLoseLabelNewRecord.gameObject.SetActive(false);
		LevelInfo.Environments.popupLoseLabelBonus.gameObject.SetActive(false);
		
		if(distance>highscore) PlayerPrefs.SetInt("high_score",distance);
		
		yield return StartCoroutine(WaitSeconds(1.0f));
		
		LevelInfo.Environments.popupLoseLabelNewRecord.gameObject.SetActive(distance>highscore);
		
		yield return StartCoroutine(WaitSeconds(1.0f));
		
		int bonus=0;
		if(highscore<5000&&distance>=5000) 		bonus+=100;
		if(highscore<10000&&distance>=10000) 	bonus+=250;
		if(highscore<15000&&distance>=15000) 	bonus+=500;
		if(highscore<20000&&distance>=20000) 	bonus+=1000;
		if(highscore<30000&&distance>=30000) 	bonus+=2500;
		if(highscore<50000&&distance>=50000) 	bonus+=5000;
		if(highscore<100000&&distance>=100000) 	bonus+=10000;
		if(highscore<200000&&distance>=200000) 	bonus+=25000;
		
		if(bonus>0)
		{
			LevelInfo.Environments.popupLoseLabelBonus.text = "BUNUS\nUNLIKELIUMS\n"+bonus;
			Store.Instance.Unlikeliums += bonus;
			LevelInfo.Environments.popupLoseLabelBonus.gameObject.SetActive(true);
		}
	}
	
	private IEnumerator ShowMissionsComplete()
	{
		// Show Completed Missions
		float speed = 6000f;
		
		foreach(var obj in LevelInfo.Environments.missionCompletePopups) obj.SetActive(false);
		LevelInfo.Environments.missionCompleteRoot.SetActive(true);
		
		int current=0;
		for(int i=0;i<3;i++)
		{
			if(Stats.Instance.Completed(i))
			{
				yield return StartCoroutine(WaitSeconds(0.5f));
				var sprite = LevelInfo.Environments.missionCompletePopups[current];
				LevelInfo.Environments.missionCompletePopupsLabel[current].text = Stats.Instance.CurrentMission(i).missionDescription;
				sprite.transform.localPosition = new Vector3(-2000f,sprite.transform.localPosition.y,0f);
				sprite.SetActive(true);
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Audio.clipCompleteMission);
				while(sprite.transform.localPosition.x<0f)
				{
					sprite.transform.localPosition += new Vector3(speed*0.016f,0f,0f);
					if(sprite.transform.localPosition.x>=0) sprite.transform.localPosition = new Vector3(0f,sprite.transform.localPosition.y,0f);
					yield return new WaitForEndOfFrame();
				}
				
				Stats.Instance.MissionComplete(i);
				i--;
				current++;
				yield return StartCoroutine(WaitSeconds(0.5f));
			}
		}
		
		if(current!=0)
			yield return StartCoroutine(WaitSeconds(5f));

		LevelInfo.Environments.missionCompleteRoot.SetActive(false);
	}
	
	private IEnumerator WaitSeconds(float sec)
	{
		while(sec>0f) {sec-=0.016f; yield return null;}
	}
	
	private bool HUBActiveHelper(GameState _state)
	{
		return _state == GameState.Lose || _state == GameState.Paused || _state == GameState.Play;
	}
	
	public bool HUBActive{
		get{
			return HUBActiveHelper(state);
		}
	}
		
	void Awake()
	{
		Application.targetFrameRate = 60;
	}
	
	void Start()
	{
		state = GameState.Play;
	}
	
	private bool _goMenuWhenLose = false;
	public bool goMenuWhenLose{
		get{
			return _goMenuWhenLose;
		}
		set{
			_goMenuWhenLose = value;
			LevelInfo.Environments.popupLose.SetActive(!value);
			LevelInfo.Environments.popupQuit.SetActive(value);
		}
	}
	
	void Update()
	{
		if( state == GameState.Lose && Input.GetKeyUp(KeyCode.Escape) )
		{
			goMenuWhenLose = !goMenuWhenLose;
		}
	}
}
