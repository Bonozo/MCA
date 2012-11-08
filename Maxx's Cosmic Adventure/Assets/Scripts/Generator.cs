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
	
	public GameObject GemUnlikeliumPrefab;
	public GameObject GemSureShotPrefab;
	public GameObject GemShieldPrefab;
	public GameObject GemMagnetPrefab;
	
	public void GenerateNewGem(Vector3 pos,Gems gemtype)
	{
		GameObject prefab = null;
		switch(gemtype)
		{
		case Gems.Unlikelium:
			prefab = GemUnlikeliumPrefab;
			break;
		case Gems.SureShot:
			prefab = GemSureShotPrefab;
			break;
		case Gems.Shield:
			prefab = GemShieldPrefab;
			break;
		case Gems.Magnet:
			prefab = GemMagnetPrefab;
			break;
		}
		
		Instantiate(prefab,pos,Quaternion.identity);
	}
	
	public void GenerateNewGem(Vector3 pos)
	{
		int rand = Random.Range(0,sizeof(Gems));//??//
		GenerateNewGem(pos,(Gems)rand);
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
				int len = distance >= Stage_One_Distance ? 7 : 2 ;
				GenerateNewAsteroid(Random.Range(0,len));
			}
			next_asteroid_time += Stage_One_Step;
		}
		
		if( distance >= next_jeeble_time )
		{
			if( Random.Range(0,2)==1 )
			{
				int len = distance >= Stage_Three_Distance ? 2 : 1 ;
				GenerateNewAlienShip(Random.Range(0,len));
			}
			next_jeeble_time += Stage_Two_Step_Enemy;		
		}
		
		
	
	}
	
	#endregion
}
