using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#region Alien Ship
	
	public bool GenerateAlienShip = false;
	public GameObject[] AlienShipPrefabs;
	public float AlienShipDistanceMin=30f, AlienShipDistanceMax=50f;
	public float AlienShipGenerateRate = 5f;
	public float AlienShipFrontAngleMaxDelta = 30f;
	
	private float _alienShipRate;
	
	public void GenerateNewAlienShip()
	{
		if( GenerateAlienShip ) 
		{
			Vector3 pos = new Vector3(0,0,Random.Range(AlienShipDistanceMin,AlienShipDistanceMax));
			pos += LevelInfo.Environments.playerShip.transform.position;
			pos.y = 0;
		
			GameObject newAlienShip = (GameObject)Instantiate(AlienShipPrefabs[Random.Range(0,AlienShipPrefabs.Length)],pos,Quaternion.identity);
			newAlienShip.transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, Vector3.up, 
				LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y + Random.Range(-AlienShipFrontAngleMaxDelta,AlienShipFrontAngleMaxDelta) );	
			newAlienShip.tag = "AlienShip";//??//
		}
		_alienShipRate = Time.time + AlienShipGenerateRate;
	}
	
	#endregion
	
	#region Asteriod
	
	public bool GenerateAsteroid = false;
	public GameObject[] AsteroidPrefabs;
	public float AsteroidDistanceMin=30f, AsteroidDistanceMax=50f;
	public float AsteroidGenerateRate = 5f;
	public float AsteroidFrontAngleMaxDelta = 30f;
	
	private float _asteroidRate;
	

	
	public void GenerateNewAsteroid()
	{
		if( GenerateAsteroid ) 
		{
			Vector3 pos = new Vector3(0,0,Random.Range(AsteroidDistanceMin,AsteroidDistanceMax));
			pos += LevelInfo.Environments.playerShip.transform.position;
			pos.y = 0;
		
			GameObject newAsteroid = (GameObject)Instantiate(AsteroidPrefabs[Random.Range(0,AsteroidPrefabs.Length)],pos,Quaternion.identity);
			newAsteroid.transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, Vector3.up, 
				LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y + Random.Range(-AsteroidFrontAngleMaxDelta,AsteroidFrontAngleMaxDelta) );	
			newAsteroid.tag = "Asteroid";//??//
		}
		_asteroidRate = Time.time + AsteroidGenerateRate;
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
	
	// Use this for initialization
	void Start () {
	
		_alienShipRate = Time.time + AlienShipGenerateRate;
		_asteroidRate = Time.time + AsteroidGenerateRate;
	}
	
	// Update is called once per frame
	void Update () {
		if( LevelInfo.Environments.playerShip == null ) return;
		
		if(  Time.time >= _alienShipRate) GenerateNewAlienShip();
		if(  Time.time >= _asteroidRate) GenerateNewAsteroid();
	
	}
}
