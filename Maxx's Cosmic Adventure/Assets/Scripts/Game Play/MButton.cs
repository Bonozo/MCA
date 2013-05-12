using UnityEngine;
using System.Collections;

public class MButton : MonoBehaviour {
	
	public UILabel powerupName;
	
	private Gems _currentPowerup = Gems.None;
	public Gems currentPowerup{
		get{
			return _currentPowerup;
		}
		set{
			
			if(_currentPowerup == Gems.FireBall)
				LevelInfo.Environments.playerFlamethrowerTransform.gameObject.SetActive(false);
			_currentPowerup = value;
			
			switch(_currentPowerup)
			{
			case Gems.In3s:
				powerupName.text = "Triple Trouble";
				break;
			case Gems.Pow:
				powerupCount = Store.Instance.powerupPOW.level+1;
				powerupName.text = "POW " + powerupCount;
				break;
			case Gems.FireBall:
				powerupCount = 3*(Store.Instance.powerupLighenUp.level+1);
				LevelInfo.Environments.playerFlamethrowerTransform.gameObject.SetActive(true);
				powerupName.text = "Lighten Up " + powerupCount;
				break;
			default:
				powerupName.text = "";
				break;
			}
		}
	}
	
	void Awake()
	{
		currentPowerup = Gems.None;
	}
	
	void Update()
	{
		In3sUpdate();
		
		#if UNITY_EDITOR || UNITY_STANDALONE
		if( Input.GetKeyDown(KeyCode.M) )
			OnPress(true);
		#endif
	}
	
	void OnPress(bool isDown)
	{
		if(isDown && LevelInfo.Environments.playerShip.Ready && !LevelInfo.Environments.playerShip.IsBoost && !LevelInfo.Environments.playerShip.Intergalactic) 
		{
			if(LevelInfo.State.state == GameState.Play && currentPowerup != Gems.None)
				StartPowerup();
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
			powerupCount--;
			if(powerupCount==0)
			{
				powerupName.text = "";
				currentPowerup = Gems.None;
			}
			else
				powerupName.text = "POW " + powerupCount;
			break;
		case Gems.FireBall:
			Instantiate(LevelInfo.Environments.prefabPlayerFireBall,LevelInfo.Environments.playerFireballTransform.position,LevelInfo.Environments.playerFireballTransform.rotation);			
			powerupCount--;
			if(powerupCount==0)
			{
				powerupName.text = "";
				currentPowerup = Gems.None;
			}
			else
				powerupName.text = "Lighten Up " + powerupCount;
			break;
		}
	}
	
	int in3scount = 0;
	GameObject[] in3smissle = new GameObject[3]; 
	
	void In3sPowerup()
	{
		while(in3scount < 3 )
		{
			in3smissle[in3scount] = (GameObject)Instantiate(LevelInfo.Environments.prefabPlayerMissle,
				LevelInfo.Environments.posPlayerMissle[in3scount].position,LevelInfo.Environments.posPlayerMissle[in3scount].rotation);
			in3smissle[in3scount].transform.parent = LevelInfo.Environments.posPlayerMissle[in3scount].transform;
			in3scount++;
		}
		
		currentPowerup = Gems.None;
		/*return;
		
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
		
		currentPowerup = Gems.None;*/
	}
	
	float in3sdistancetoshot = 200f;
	float wtin3s = 0.0f;
	void In3sUpdate()
	{
		if(in3scount>0)
		{
			wtin3s-=Time.deltaTime;
			if(wtin3s<=0f)
			{	
				wtin3s = 0.3f;
				
				float dist = float.PositiveInfinity;
				GameObject target = null;
				
				GameObject[] ship = GameObject.FindGameObjectsWithTag("Enemy");
				GameObject[] asteroid = GameObject.FindGameObjectsWithTag("Asteroid");
				
				foreach(GameObject g in ship)
				{		
					Vector3 toscreen = LevelInfo.Environments.mainCamera.WorldToScreenPoint(g.transform.position);
					if(g.GetComponent<MissledAlienShip>()==null && toscreen.x >= 0 && toscreen.x <= Screen.width && toscreen.y >= 0 && toscreen.y <= Screen.height && toscreen.z > 1f && toscreen.z < in3sdistancetoshot && toscreen.z < dist)
					{
						target = g;
						dist = toscreen.z;
					}	
				}
				
				foreach(GameObject g in asteroid)
				{
					Vector3 toscreen = LevelInfo.Environments.mainCamera.WorldToScreenPoint(g.transform.position);
					if(g.GetComponent<MissledAlienShip>()==null && toscreen.x >= 0 && toscreen.x <= Screen.width && toscreen.y >= 0 && toscreen.y <= Screen.height && toscreen.z > 1f && toscreen.z < in3sdistancetoshot && toscreen.z < dist)
					{
						target = g;
						dist = toscreen.z;
					}		
				}	
				
				if( target != null )
				{
					target.AddComponent<MissledAlienShip>();
					in3scount--;
					in3smissle[in3scount].transform.parent = null;
					in3smissle[in3scount].GetComponent<Bullet>().Activate();
					in3smissle[in3scount].GetComponent<Bullet>().ExplodeTargetWithOneShot(target);	
				}
			}
		}		
	}
	
	IEnumerator PowPowerup()
	{
		LevelInfo.Audio.audioSourcePlayerShip.PlayOneShot(LevelInfo.Audio.clipPowerupPOW);
		LevelInfo.Environments.ShockWave.Emit(1);
		
		yield return new WaitForSeconds(0.2f);
		
		GameObject[] ship = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] asteroid = GameObject.FindGameObjectsWithTag("Asteroid");	
		
		foreach(GameObject g in ship) g.SendMessage("Explode");
		foreach(GameObject g in asteroid) g.SendMessage("Explode");
	}
	
	int powerupCount = 0;
	/*IEnumerator FireBallPowerup()
	{
		timedpowerup = true;
		float time = Store.Instance.powerupLighenUp.LevelTime;
		while(time>0)
		{
			time -= Time.deltaTime;
			powerupName.text = "" + (int)(time+0.99f);
			yield return null;
		}
		
		timedpowerup = false;
		currentPowerup = Gems.None;
	}*/
}
