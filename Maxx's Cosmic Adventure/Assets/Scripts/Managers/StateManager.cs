using UnityEngine;
using System.Collections;

#region Main Enumerations

public enum GameState
{
	Play,
	Paused,
	Options,
	Store,
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
	ToughGuy
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
			
			if(last == GameState.Store && LevelInfo.Environments.score.Lives==0)
			{
				_state = GameState.Lose;
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
				LevelInfo.Environments.HUB.SetActive(false);
				Store.Instance.ShowStore = true;
				break;
			case GameState.Options:
				LevelInfo.Environments.HUB.SetActive(false);
				Options.Instance.ShowOptions = true;
				break;
			case GameState.Lose:				
				StartCoroutine(ShowGameOverScreenThread());
				break;
			}
			
			Time.timeScale = (state == GameState.Play?1f:0f);
		}
	}
	
	private IEnumerator ShowGameOverScreenThread()
	{
		/* old version
		 * 
		 * var names = LevelInfo.Environments.popupLoseLabelNames;
		var results = LevelInfo.Environments.popupLoseLabelResults;
		
		float startdelay = 1f;
		float deltadelay = 0.3f;
		
		names.text = results.text = "";
		
		LevelInfo.Environments.popupLose.SetActive(true);
		
		yield return StartCoroutine(WaitSeconds(startdelay));
		
		names.text += "Distance Traveled:";
		results.text += Mathf.FloorToInt(LevelInfo.Environments.playerShip.DistanceTravelled);
		yield return StartCoroutine(WaitSeconds(deltadelay));
		
		names.text +=   "\n"+"Unlikelium Collected:";
		results.text += "\n"+LevelInfo.Environments.score.unlikeliumsCollected;
		yield return StartCoroutine(WaitSeconds(deltadelay));
		
		names.text +=   "\n"+"Asteroids Destroyed:";
		results.text += "\n"+LevelInfo.Environments.score.asteroidsDestoyed;
		yield return StartCoroutine(WaitSeconds(deltadelay));
		
		names.text +=   "\n"+"Jeebies Defeated:";
		results.text += "\n"+LevelInfo.Environments.score.jeebiesDestoyed;
		yield return StartCoroutine(WaitSeconds(startdelay));
		
		names.text +=   "\n\n"+"Score:";
		results.text += "\n\n"+LevelInfo.Environments.score.totalScore;*/
		
		int distance = Mathf.FloorToInt(LevelInfo.Environments.playerShip.DistanceTravelled);
		int highscore = PlayerPrefs.GetInt("high_score",0);
		
		LevelInfo.Environments.playerShip.ClearAllPowerups();
		
		foreach(var r in LevelInfo.Environments.playerShip.ExhaustArray )
			r.enableEmission = false;
		
		LevelInfo.Audio.StopAll();
		LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Audio.clipGameOver);
		
		LevelInfo.Environments.popupLose.SetActive(true);
		var results = LevelInfo.Environments.popupLoseLabelResults;
		results.text = 
			"Distance Traveled: " + distance +
			"\nUnlikelium Collected: " + LevelInfo.Environments.score.unlikeliumsCollected +
			"\nAsteroids Destroyed: " + LevelInfo.Environments.score.asteroidsDestoyed +
			"\nJeebies Defeated: " + LevelInfo.Environments.score.jeebiesDestoyed + 
			"\n\nScore: " + LevelInfo.Environments.score.totalScore;
		
		LevelInfo.Environments.popupLoseLabelNewRecord.gameObject.SetActive(false);
		if(distance>highscore) PlayerPrefs.SetInt("high_score",distance);
		
		yield return StartCoroutine(WaitSeconds(0.5f));
		
		LevelInfo.Environments.popupLoseLabelNewRecord.gameObject.SetActive(distance>highscore);
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
