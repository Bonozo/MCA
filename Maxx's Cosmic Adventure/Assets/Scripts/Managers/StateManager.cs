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
	Shield,
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
				LevelInfo.Environments.playerShip.transform.localScale *= 0;
				LevelInfo.Environments.playerShip.ClearAllPowerups();
				GameObject c = GameObject.Find("GameOverText");
				if(c != null )
					c.GetComponent<GameOver>().enabled = true;
				
				foreach(var r in LevelInfo.Environments.playerShip.ExhaustArray )
					r.enableEmission = false;
				
				LevelInfo.Audio.StopAll();
				LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Environments.playerShip.AudioGameOver);
				break;
			}
			
			Time.timeScale = (state == GameState.Play?1f:0f);
		}
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
