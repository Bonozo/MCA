using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#region Alien Ship
	
	public enum Jeebie
	{
		BlueFighterPilot=0,
		BlueLeader=1,
		RedKamikaze=2,
		TripleKamikaze=3,
		PurpleFigher=4,
		Reactive=5
	}
	
	private readonly float first_jeebie_distance = 500f;
	private readonly float[] next_jeebie_stage_distance = new float[] {100f,150f,200f,300f};
	private float next_jeebie_distance = 0f;
	
	public bool GenerateAlienShip = false;
	public GameObject[] AlienShipPrefabs;
	/*
	public void GenerateNewAlienShip()
	{
		float maxdistance = 12000f;
		float currentdistance = LevelInfo.Environments.playerShip.DistanceTravelled;
		
		int maxindex = Mathf.Min((int)(AlienShipPrefabs.Length*currentdistance/maxdistance),AlienShipPrefabs.Length);
		int index = Random.Range(0,maxindex);
		
		int count = Mathf.Max(1,(int)(Random.Range(0,currentdistance+1000f)/1000f));
		float delta = Random.Range(0.8f,1.2f);
		
		if(index==2||index==3) count = Mathf.Min(count,3);
		else if(index==0&&count==2&&Random.Range(0,2)==0) delta=0f;

		StartCoroutine(StartAlienAttack((Jeebie)index,count,delta));
	}
	*/
	
	public void SpawnJeebieByDistance()
	{
		float distance = LevelInfo.Environments.playerShip.DistanceTravelled;
		
		if(distance<=1000f)
		{
			StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,1,0));
		}
		else if(distance<=2000f)
		{
			StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(1,3),0f));
		}
		else if(distance<=3000f)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.BlueLeader,1,0f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(1,3),Random.Range(0.5f,1f)));
		}
		else if(distance<=4000f)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.BlueLeader,1,0f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(1,4),0.5f));
		}
		else if(distance<=5000f)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.RedKamikaze,Random.Range(1,4),Random.Range(0.3f,0.5f)));
			else if(Random.Range(0,2)==1)//25%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,1,0f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(1,5),0.5f));
		}
		else if(distance<=6000f)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(2,5),0.5f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.RedKamikaze,Random.Range(2,5),Random.Range(0.3f,0.5f)));	
		}
		else if(distance<=7000f)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.TripleKamikaze,1,0f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.RedKamikaze,Random.Range(3,7),Random.Range(0.3f,0.5f)));	
		}
		else if(distance<=8000f)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,1,0f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(4,8),Random.Range(0.3f,0.5f)));	
		}
		else if(distance<=9000f)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.BlueLeader,1,0f));
			else 
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(1,3),0f));
		}
		else if(distance<=10000f)// Purple Attack
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(1,3),0.2f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(2,5),0.7f));
		}
		else if(distance<=11000f)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(2,5),0.3f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(6,10),Random.Range(0.3f,0.5f)));
		}
		else if(distance<=12000f)// Blue Fighter and Leader Attack
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.BlueLeader,2,Random.Range(1f,2f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(7,12),0.5f));	
		}
		else if(distance<=13000f) // Reactive and Triple Kamikaze Attack
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.Reactive,Random.Range(6,12),Random.Range(0.2f,0.4f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.TripleKamikaze,Random.Range(2,5),Random.Range(0.7f,1.2f)));
		}
		else if(distance<=14000)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.Reactive,Random.Range(6,13),Random.Range(0.2f,0.4f)));	
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(6,15),0.5f));	
		}
		else if(distance<=15000)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(7,15),0.7f));	
			else
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(3,6),Random.Range(0.6f,1.2f)));
		}
		else if(distance<=16000)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.Reactive,Random.Range(10,15),0.7f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(3,6),Random.Range(0.6f,1.2f)));
		}
		else if(distance<=17000)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.BlueLeader,Random.Range(3,8),1.1f));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(10,15),Random.Range(0.6f,0.9f)));
		}
		else if(distance<=18000)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(7,10),Random.Range(0.8f,1.2f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(12,16),Random.Range(0.6f,0.9f)));			
		}
		else if(distance<=19000)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(3,5),Random.Range(0.8f,1.2f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(13,18),Random.Range(0.6f,0.9f)));				
		}
		else if(distance<=20000)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.Reactive,Random.Range(7,12),Random.Range(0.3f,1.0f)));
			else if(Random.Range(0,2)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.RedKamikaze,Random.Range(7,12),Random.Range(0.3f,1.0f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.TripleKamikaze,Random.Range(7,10),Random.Range(0.7f,1.2f)));
		}
		else if(distance<=22000)
		{
			if(Random.Range(0,3)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(7,12),Random.Range(0.3f,1.0f)));
			else if(Random.Range(0,2)==1)//33%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(7,12),Random.Range(0.3f,1.0f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueLeader,Random.Range(7,10),Random.Range(0.7f,1.2f)));			
		}
		else if(distance<=24000)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(12,20),Random.Range(0.6f,1.0f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.TripleKamikaze,Random.Range(7,10),Random.Range(1f,1.2f)));			
		}
		else if(distance<=26000)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.Reactive,Random.Range(12,20),Random.Range(1f,1.2f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(12,20),Random.Range(1f,1.2f)));						
		}
		else if(distance<=28000)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(15,20),Random.Range(0.7f,1.2f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.BlueFighterPilot,Random.Range(7,10),Random.Range(0.8f,1.2f)));			
		}
		else if(distance<=30000)
		{
			if(Random.Range(0,2)==1)//50%
				StartCoroutine(StartAlienAttack(Jeebie.PurpleFigher,Random.Range(15,20),Random.Range(0.5f,1.0f)));
			else
				StartCoroutine(StartAlienAttack(Jeebie.TripleKamikaze,Random.Range(7,10),Random.Range(0.8f,1.1f)));		
		}
		else
		{
			var jeeb = (Jeebie)Random.Range(0,AlienShipPrefabs.Length);
			if(jeeb == Jeebie.TripleKamikaze || jeeb == Jeebie.BlueLeader)
				StartCoroutine(StartAlienAttack(jeeb,Random.Range(7,10),Random.Range(0.7f,1.5f)));
			else
				StartCoroutine(StartAlienAttack(jeeb,Random.Range(15,20),Random.Range(0.3f,1.2f)));
		}
	}
	
	public IEnumerator StartAlienAttack(Jeebie jeebie,int count,float delta)
	{
		while(count>0&&GenerateAlienShip)
		{
			yield return new WaitForSeconds(delta);
			LevelInfo.Environments.generator.SpawnJeebie((int)jeebie);
			count--;
		}
	}
	
	public void SpawnJeebie(int index)
	{
		if( GenerateAlienShip && AlienShip.GlobalCount<30)
			Instantiate(AlienShipPrefabs[index]);
	}
	
	#endregion
	
	#region Asteriod
	
	public bool GenerateAsteroid = false;
	public GameObject[] AsteroidPrefabs;
	public float AsteroidDistanceMin=30f, AsteroidDistanceMax=50f;

	public void GenerateNewAsteroid(int index)
	{
		if( GenerateAsteroid && Asteroid.GlobalCount<15) 
		{
			Instantiate(AsteroidPrefabs[index]);
		}
	}
	
	
	#endregion
	
	#region Unlikeliums
	
	public GameObject[] prefabUnlikelium;
	private float distance_Unlikelium_Min = 300f, distance_Unlikelium_Max = 600f;
	private float muchUnlikeliumsInFirstTimes_Meter = 1000f;
	private float distance_Unlikelium_Min_FirstTimes = 150f, distance_Unlikelium_Max_FirstTimes = 300f;
	private float next_unlikelium_distance = 0f;
	
	#endregion
	
	#region Gems
	
	public GameObject[] Gem;
	
	public void GenerateNewGem(Vector3 pos)
	{
		int rand = Random.Range(0,Gem.Length);
		Instantiate(Gem[rand],pos,Quaternion.identity);
	}
	
	#endregion
	
	#region Auto Spawning
	
	private const float Stage_One_Step = 50f, Stage_Two_Step_Enemy = 100f;
	private const float Stage_One_Distance = 500f, Stage_Three_Distance = 1000f;
	
	private float next_asteroid_time = Stage_One_Step;
	
	void Awake()
	{
		next_jeebie_distance = first_jeebie_distance + Random.Range(0f,200f);
		next_unlikelium_distance = Random.Range(100f,150f);
		
		
		next_asteroid_time = Random.Range(160f,300f);
	}
	
	void Update () 
	{
		if( LevelInfo.State.state != GameState.Play ) return;
		
		float distance = LevelInfo.Environments.playerShip.DistanceTravelled;
		
		if( distance >= next_asteroid_time )
		{
			if( Random.Range(0,2)==1 )
			{
				int len = AsteroidPrefabs.Length;
				if(distance<10000) len = AsteroidPrefabs.Length-1;
				if(distance<5000) len = AsteroidPrefabs.Length-2;
				if(distance<1000) len = AsteroidPrefabs.Length-3;
				if(distance<500) len = AsteroidPrefabs.Length-4;
				if(len<1) len = 1;
				GenerateNewAsteroid(Random.Range(0,len));
			}
			next_asteroid_time += Stage_One_Step;
			/*??*/if(next_asteroid_time<distance) next_asteroid_time=distance+Random.Range(0f,2f);
		}
		
		///////////////// Jeebies //////////////////////
		if(distance >= next_jeebie_distance)
		{
			SpawnJeebieByDistance();
			var next_distance = next_jeebie_stage_distance[Random.Range(0,next_jeebie_stage_distance.Length)];
			next_jeebie_distance += next_distance;
		}
		////////////////////////////////////////////////
		
		/*if( distance >= next_jeeble_time )
		{
			Debug.Log("jeebie spawn time");
			if( Random.Range(0,2)==1 )
			{
				//int len = distance >= Stage_Three_Distance ? AlienShipPrefabs.Length : 1 ;
				GenerateNewAlienShip();
			}
			next_jeeble_time += Stage_Two_Step_Enemy;	
			if(next_jeeble_time<distance) next_jeeble_time=distance+Random.Range(0f,2f);
		}*/
		
		////////////////// Unlikeliums /////////////////
		if( distance >= next_unlikelium_distance)
		{
			if(distance <= muchUnlikeliumsInFirstTimes_Meter )
				next_unlikelium_distance = distance + Random.Range(distance_Unlikelium_Min_FirstTimes,distance_Unlikelium_Max_FirstTimes);
			else
				next_unlikelium_distance = distance + Random.Range(distance_Unlikelium_Min,distance_Unlikelium_Max);
			Instantiate(prefabUnlikelium[Random.Range(0,prefabUnlikelium.Length)]);
		}
		////////////////////////////////////////////////
		
		if( GenerateUnlikeliumList )
		{
			utime -= Time.deltaTime;
			upos += udir*Time.deltaTime*LevelInfo.Settings.PlayerSpeed;
			upos += url*ucount*ucount*uright*Time.deltaTime*LevelInfo.Settings.PlayerSpeed;
			if(utime<=0f)
			{
				utime = utimedelta;
				if(uplus && ucount>Random.Range(5,8)) uplus=false;
				if(uplus) ucount++; else ucount--;
				if(!uplus && ucount==0)
				{
					ucount = 0; uplus=true;
					//url = Random.Range(0,2)==1-Random.Range(0,2)?0.01f:-0.01f;
					url = Random.Range(-1,2)*0.01f;
				}
				
				shazamcount++;
				
				int level = Store.Instance.powerupShazam.level;
				upos.y=0;
				if(level>0 && shazamcount%(6-level)==0)
					unlikeliums.Add((GameObject)Instantiate(LevelInfo.Environments.prefabUnlikeliumBronze,upos,Quaternion.identity));
				else
					unlikeliums.Add((GameObject)Instantiate(LevelInfo.Environments.prefabUnlikelium,upos,Quaternion.identity));
			}
		}
	
	}
	
	// Used after headstart, intergalactic, ...
	public void ResetSpawnDeltaTime()
	{
		next_asteroid_time = LevelInfo.Environments.playerShip.DistanceTravelled + Random.Range(0,Stage_One_Step);
	}
	
	#endregion
	
	#region Unlikelium Generator
	
	private bool GenerateUnlikeliumList = false;
	private float utimedelta = 0.3f;
	private float utime = 0f;
	private float url = 0;
	private int ucount = 5;
	private bool uplus = false;
	private int shazamcount = 0;
	
	private System.Collections.Generic.List<GameObject> unlikeliums = new System.Collections.Generic.List<GameObject>();
	private Vector3 upos,udir,uright;
	
	public void StartUnlikeliumGenerator()
	{
		StartCoroutine(StartUnlikeliumGeneratorThread());
	}
	
	public IEnumerator StartUnlikeliumGeneratorThread()
	{
		yield return new WaitForSeconds(1f);
		upos = LevelInfo.Environments.playerShip.transform.position;
		udir = LevelInfo.Environments.playerShip.transform.forward;
		uright = LevelInfo.Environments.playerShip.transform.right;
		uright.y=0f; uright.Normalize();
		upos+=udir*150f;
		utime = 0f;
		url = 0.0f;
		ucount=5;
		uplus = false;
		shazamcount = 0;
		GenerateUnlikeliumList = true;
	}
	
	public void StopUnlikeliumGenerator()
	{
		GenerateUnlikeliumList = false;
	}
	
	public void DeletaUnlikeliumList()
	{
		foreach(GameObject v in unlikeliums) if(v!=null) Destroy(v);
	}

	#endregion
}
