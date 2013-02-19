using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#region Alien Ship
	
	public bool GenerateAlienShip = false;
	public GameObject[] AlienShipPrefabs;
	public float AlienShipDistanceMin=30f, AlienShipDistanceMax=50f;
	public float AlienShipGenerateRate = 5f;
	public float AlienShipFrontAngleMaxDelta = 30f;
	
	
	public void GenerateNewAlienShip(int index)
	{
		if( GenerateAlienShip ) 
		{
			Vector3 pos = new Vector3(0,0,Random.Range(AlienShipDistanceMin,AlienShipDistanceMax));
			pos += LevelInfo.Environments.playerShip.transform.position;
			pos.y = 0;
		
			GameObject newAlienShip = (GameObject)Instantiate(AlienShipPrefabs[index],pos,Quaternion.identity);
			newAlienShip.transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, Vector3.up, 
				LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y + Random.Range(-AlienShipFrontAngleMaxDelta,AlienShipFrontAngleMaxDelta) );	
		}
	}
	
	#endregion
	
	#region Asteriod
	
	public bool GenerateAsteroid = false;
	public GameObject[] AsteroidPrefabs;
	public float AsteroidDistanceMin=30f, AsteroidDistanceMax=50f;
	public float AsteroidGenerateRate = 5f;
	public float AsteroidFrontAngleMaxDelta = 30f;
	
	

	
	public void GenerateNewAsteroid(int index)
	{
		if( GenerateAsteroid ) 
		{
			Instantiate(AsteroidPrefabs[index]);
			/*Vector3 pos = new Vector3(0,0,Random.Range(AsteroidDistanceMin,AsteroidDistanceMax));
			pos += LevelInfo.Environments.playerShip.transform.position;
			pos.y = 0;
		
			GameObject newAsteroid = (GameObject)Instantiate(AsteroidPrefabs[index],pos,Quaternion.identity);
			newAsteroid.transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, Vector3.up, 
				LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y + Random.Range(-AsteroidFrontAngleMaxDelta,AsteroidFrontAngleMaxDelta) );	
			newAsteroid.tag = "Asteroid";//??//*/
		}
	}
	
	
	#endregion
	
	#region Gems
	
	public GameObject[] Gem;
	
	public void GenerateNewGem(Vector3 pos)
	{
		int rand = Random.Range(0,Gem.Length);
		Instantiate(Gem[rand],pos,Quaternion.identity);
	}
	
	#endregion
	
	#region Auto Spawing
	
	private const float Stage_One_Step = 50f, Stage_Two_Step_Enemy = 100f;
	private const float Stage_One_Distance = 500f, Stage_Three_Distance = 1000f;
	
	private float next_asteroid_time = Stage_One_Step;
	private float next_jeeble_time = Stage_One_Distance;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( LevelInfo.Environments.playerShip == null ) return;
		
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
				int len = distance >= Stage_Three_Distance ? AlienShipPrefabs.Length : 1 ;
				GenerateNewAlienShip(Random.Range(0,len));
			}
			next_jeeble_time += Stage_Two_Step_Enemy;	
			/*??*/if(next_jeeble_time<distance) next_jeeble_time=distance+Random.Range(0f,2f);
		}
		
		if( GenerateUnlikeliumList )
		{
			utime -= Time.deltaTime;
			upos += udir*Time.deltaTime*LevelInfo.Environments.playerShip.Speed;
			upos += url*url*ucount*ucount*uright*Time.deltaTime*LevelInfo.Environments.playerShip.Speed;
			if(utime<=0f)
			{
				utime = utimedelta;
				if(uplus && ucount>Random.Range(5,8)) uplus=false;
				if(uplus) ucount++; else ucount--;
				if(!uplus && ucount==0)
				{
					ucount = 0; uplus=true;
					url = Random.Range(0,2)==1-Random.Range(0,2)?0.1f:-0.1f;
				}
				
				unlikeliums.Add((GameObject)Instantiate(LevelInfo.Environments.prefabUnlikelium,upos,Quaternion.identity));
			}
		}
	
	}
	
	#endregion
	
	#region Unlikelium Generator
	
	private bool GenerateUnlikeliumList = false;
	private float utimedelta = 0.3f;
	private float utime = 0f;
	private float url = 0;
	private int ucount = 5;
	private bool uplus = false;
	
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
