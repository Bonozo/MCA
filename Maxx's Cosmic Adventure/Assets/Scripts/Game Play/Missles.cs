using UnityEngine;
using System.Collections;

public class Missles : MonoBehaviour {
	
	public Transform[] bulletPositions;//len = 3
	
	private float SmartBombDistance = 75;
	public GameObject[] gui;
	
	private int _missleCount = 0;
	public int missleCount{
		get
		{
			return _missleCount;
		}
		set
		{
			_missleCount = Mathf.Min(value,gui.Length);
			for(int i=0;i<gui.Length;i++)
				gui[i].SetActive(i<_missleCount);
		}
	}
	
	private int currentActiveCount = 0;
	private GameObject[] bullet = new GameObject[3];
	
	void Awake()
	{
		missleCount = 1;
		//foreach(GameObject g in gui) g.SetActive(false);
	}
	
	void OnPress(bool isDown)
	{
		if( isDown )
		{
			/*??*/if( Time.deltaTime == 0.0f || LevelInfo.Environments.score.Lose ) return;
			if(missleCount>0 && currentActiveCount==0)
			{
				missleCount--;
				StartCoroutine(StartMission());
			}
		}
	}
	
	private IEnumerator StartMission()
	{
		for(int i=0;i<3;i++)
		{
			bullet[i] = (GameObject)Instantiate(LevelInfo.Environments.prebafIn3sProjectile,bulletPositions[i].position,bulletPositions[i].rotation);
			bullet[i].transform.parent = LevelInfo.Environments.playerShip.transform;
		}
		currentActiveCount = 3;
		
		while(currentActiveCount>0)
		{
			GameObject[] g = GameObject.FindGameObjectsWithTag("AlienShip");
			foreach(GameObject e in g)
				if(currentActiveCount>0 && Vector3.Distance(e.transform.position,LevelInfo.Environments.playerShip.transform.position) <= SmartBombDistance )
				{
					int  current = 3-currentActiveCount;
					currentActiveCount--;
				
					Vector3 pos = bullet[current].transform.position;
					Destroy(bullet[current]);
					
					var b = ((GameObject)Instantiate(LevelInfo.Environments.prefabPlayerProjectile,pos,Quaternion.identity)).GetComponent<Bullet>();
					b.Speed = 200;
					b.DeadTime = float.PositiveInfinity;
					e.tag = "AutoTargeted";
					b.ExplodeTargetWithOneShot(e);
					
				}
			yield return new WaitForSeconds(0.11f);
		}
	}
	

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.M) )
			missleCount++;
		
	}
	
	
}
