using UnityEngine;
using System.Collections;

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
	
			if(last == GameState.Store) LevelInfo.Audio.ResumeMusic();
			
			switch(_state)
			{
			case GameState.Play:
				break;
			case GameState.Paused:
				LevelInfo.Audio.StopEffects();
				break;
			case GameState.Store:
				LevelInfo.Audio.PauseMusic();
				LevelInfo.Environments.HUB.SetActive(false);
				Store.Instance.ShowStore = true;
				break;
			case GameState.Options:
				LevelInfo.Environments.HUB.SetActive(false);
				Options.Instance.ShowOptions = true;
				break;
			case GameState.Lose:
				//LevelInfo.Environments.playerShip.transform.localScale *= 0;
				LevelInfo.Environments.playerShip.ClearAllPowerups();
				
				foreach(var r in LevelInfo.Environments.playerShip.ExhaustArray )
					r.enableEmission = false;
				
				LevelInfo.Audio.StopAll();
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Environments.playerShip.AudioGameOver);
				
				StartCoroutine(ShowGameOverScreenThread());
				
				break;
			}
			
			Time.timeScale = (state == GameState.Play?1f:0f);
		}
	}
	
	private IEnumerator ShowGameOverScreenThread()
	{
		var names = LevelInfo.Environments.popupLoseLabelNames;
		var results = LevelInfo.Environments.popupLoseLabelResults;
		
		float startdelay = 1f;
		float deltadelay = 0.5f;
		
		names.text = results.text = "";
		
		LevelInfo.Environments.popupLose.SetActive(true);
		
		yield return StartCoroutine(WaitSeconds(startdelay));
		
		names.text += "DISTANCE:";
		results.text += Mathf.FloorToInt(LevelInfo.Environments.playerShip.DistanceTravelled);
		yield return StartCoroutine(WaitSeconds(deltadelay));
		
		names.text +=   "\n"+"JEEBIES:";
		results.text += "\n"+LevelInfo.Environments.score.jeebiesDestoyed;
		yield return StartCoroutine(WaitSeconds(deltadelay));
		
		names.text +=   "\n"+"ASTEROIDS:";
		results.text += "\n"+LevelInfo.Environments.score.asteroidsDestoyed;
		yield return StartCoroutine(WaitSeconds(deltadelay));
		
		names.text +=   "\n"+"POWERUPS:";
		results.text += "\n"+LevelInfo.Environments.score.powerupsCollected;
		yield return StartCoroutine(WaitSeconds(deltadelay));
		
		names.text +=   "\n"+"UNLIKELIUMS:";
		results.text += "\n"+LevelInfo.Environments.score.unlikeliumsCollected;
		yield return StartCoroutine(WaitSeconds(startdelay));
		
		
		names.text +=   "\n\n"+"TOTAL SCORE:";
		results.text += "\n\n"+LevelInfo.Environments.score.totalScore;
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
	
	void Update()
	{
		
	}
}
