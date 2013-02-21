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
			
			if(last == GameState.Store )
				LevelInfo.Environments.HUB.SetActive(true);
			
			switch(_state)
			{
			case GameState.Play:
				break;
			case GameState.Paused:
				break;
			case GameState.Store:
				LevelInfo.Environments.HUB.SetActive(false);
				Store.Instance.ShowStore = true;
				break;
			}
			
			Time.timeScale = (state == GameState.Play?1f:0f);
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
