using UnityEngine;
using System.Collections;

public class Missles : MonoBehaviour {
	
	public UILabel powerupName;
	
	private Gems _currentPowerup = Gems.None;
	public Gems currentPowerup{
		get{
			return _currentPowerup;
		}
		set{
			_currentPowerup = value;
			
			if( timedpowerup )
			{
				StopAllCoroutines();
				timedpowerup=false;
			}
			
			switch(_currentPowerup)
			{
			case Gems.In3s:
				powerupName.text = "In 3's";
				break;
			case Gems.Pow:
				powerupName.text = "Pow";
				break;
			case Gems.FireBall:
				StartCoroutine(FireBallPowerup());
				break;
			default:
				powerupName.text = "";
				break;
			}
			
		}
	}
	
	private bool timedpowerup=false;
	
	void Awake()
	{
		currentPowerup = Gems.Pow;
	}
	
	void OnPress(bool isDown)
	{
		if(isDown) 
		{
			if(timedpowerup)
			{
				if(currentPowerup == Gems.FireBall)
					Instantiate(LevelInfo.Environments.prefabPlayerFireBall,LevelInfo.Environments.posPlayerMissle[0].position,LevelInfo.Environments.posPlayerMissle[0].rotation);			

			}
			else
			{
				if(LevelInfo.State.state == GameState.Play && currentPowerup != Gems.None)
					StartPowerup();
			}
		}
	}
	
	void StartPowerup()
	{
		switch(currentPowerup)
		{
		case Gems.In3s:
			In3sPowerup();
			break;
		case Gems.Pow:
			StartCoroutine(PowPowerup());
			break;
		}
	}

	void In3sPowerup()
	{
		GameObject[] ship = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] asteroid = GameObject.FindGameObjectsWithTag("Asteroid");
		
		GameObject[] target = {null,null,null};
		float[] dist = {float.PositiveInfinity,float.PositiveInfinity,float.PositiveInfinity};
		
		foreach(GameObject g in ship)
		{
			Vector3 toscreen = LevelInfo.Environments.mainCamera.WorldToScreenPoint(g.transform.position);
			if(toscreen.x >= 0 && toscreen.x <= Screen.width && toscreen.y >= 0 && toscreen.y <= Screen.height && toscreen.z > 1f)
			{
				for(int i=0;i<3;i++)
					if(toscreen.z < dist[i])
					{
						for(int j=i+1;j<3;j++)
						{
							target[j]=target[j-1];
							dist[j] = dist[j-1];
						}
						dist[i] = toscreen.z;
						target[i] = g;
						break;
					}
			}	
		}
		
		foreach(GameObject g in asteroid)
		{
			Vector3 toscreen = LevelInfo.Environments.mainCamera.WorldToScreenPoint(g.transform.position);
			if(toscreen.x >= 0 && toscreen.x <= Screen.width && toscreen.y >= 0 && toscreen.y <= Screen.height && toscreen.z > 1f)
			{
				for(int i=0;i<3;i++)
					if(toscreen.z < dist[i])
					{
						for(int j=i+1;j<3;j++)
						{
							target[j]=target[j-1];
							dist[j] = dist[j-1];
						}
						dist[i] = toscreen.z;
						target[i] = g;
						break;
					}
			}	
		}
		
		for(int i=0;i<3;i++)
		{
			GameObject missle = (GameObject)Instantiate(LevelInfo.Environments.prefabPlayerMissle,
				LevelInfo.Environments.posPlayerMissle[i].position,LevelInfo.Environments.posPlayerMissle[i].rotation);
			if( target[i] != null)
				missle.GetComponent<Bullet>().ExplodeTargetWithOneShot(target[i]);
		}
		
		currentPowerup = Gems.None;
	}
	
	IEnumerator PowPowerup()
	{
		LevelInfo.Environments.ShockWave.Emit(1);
		
		yield return new WaitForSeconds(0.2f);
		
		GameObject[] ship = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] asteroid = GameObject.FindGameObjectsWithTag("Asteroid");	
		
		foreach(GameObject g in ship) g.SendMessage("Explode");
		foreach(GameObject g in asteroid) g.SendMessage("Explode");
		
		currentPowerup = Gems.None;
	}
	
	IEnumerator FireBallPowerup()
	{
		timedpowerup = true;
		float time = 10f;
		while(time>0)
		{
			time -= Time.deltaTime;
			powerupName.text = "Fireball " + (int)(time+0.99f);
			yield return null;
		}
		
		timedpowerup = false;
		currentPowerup = Gems.None;
	}
}
