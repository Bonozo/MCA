using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#region Alien Ship
	
	public bool GenerateAlienShip = false;
	public GameObject[] AlienShipPrefabs;
	public float AlienShipDistanceMin=30f, AlienShipDistanceMax=50f;
	public float AlienShipFrontAngleMaxDelta = 30f;
	
	
	public void GenerateNewAlienShip(int index)
	{
		if( GenerateAlienShip && AlienShip.GlobalCount<30) 
			Instantiate(AlienShipPrefabs[index]);
	}
	
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

		StartCoroutine(StartAlienAttack(index,count,delta));
	}
	
	public IEnumerator StartAlienAttack(int index,int count,float delta)
	{
		while(count>0)
		{
			yield return new WaitForSeconds(delta);
			LevelInfo.Environments.generator.GenerateNewAlienShip(index);
			count--;
		}
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
	private float next_jeeble_time = Stage_One_Distance;
	
	void Awake()
	{
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
				int len = distance >= Stage_One_Distance ? AsteroidPrefabs.Length : 2 ;
				GenerateNewAsteroid(Random.Range(0,len));
			}
			next_asteroid_time += Stage_One_Step;
			/*??*/if(next_asteroid_time<distance) next_asteroid_time=distance+Random.Range(0f,2f);
		}
		
		if( distance >= next_jeeble_time )
		{
			if( Random.Range(0,2)==1 )
			{
				//int len = distance >= Stage_Three_Distance ? AlienShipPrefabs.Length : 1 ;
				GenerateNewAlienShip();
			}
			next_jeeble_time += Stage_Two_Step_Enemy;	
			/*??*/if(next_jeeble_time<distance) next_jeeble_time=distance+Random.Range(0f,2f);
		}
		
		// Unlikeliums
		if( distance >= next_unlikelium_distance)
		{
			if(distance <= muchUnlikeliumsInFirstTimes_Meter )
				next_unlikelium_distance = distance + Random.Range(distance_Unlikelium_Min_FirstTimes,distance_Unlikelium_Max_FirstTimes);
			else
				next_unlikelium_distance = distance + Random.Range(distance_Unlikelium_Min,distance_Unlikelium_Max);
			Instantiate(prefabUnlikelium[Random.Range(0,prefabUnlikelium.Length)]);
		}
		
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
		next_jeeble_time = LevelInfo.Environments.playerShip.DistanceTravelled + Random.Range(0,Stage_Two_Step_Enemy);
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
