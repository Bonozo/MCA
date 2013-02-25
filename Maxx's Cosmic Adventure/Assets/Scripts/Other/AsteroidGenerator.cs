using UnityEngine;
using System.Collections;

public class AsteroidGenerator : MonoBehaviour {

	#region Parameters
	
	public GameObject AsteroidPrefab;
	public GameObject AsteroidWithBlueCrystal;
	public float BeginPositionMin=80f, BeginPositionMax=100f;
	public float GenerateRate = 5f;
	public float UpDownMaxHeight = 3f;
	public float FrontAngleMaxDelta = 40f;
	
	#endregion
	
	#region Variables
	
	private GameObject player;
	private float rate;
	
	private int count = 0;
	
	#endregion
	
	#region Start, Update
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("PlayerShip");
		rate = GenerateRate;
	}
	
	// Update is called once per frame
	void Update () {
		if( LevelInfo.State.state != GameState.Play)
			return;
		
		rate -= Time.deltaTime;
		if( rate <= 0 )
		{
			Vector3 pos = new Vector3(0,Random.Range(-UpDownMaxHeight,UpDownMaxHeight),Random.Range(BeginPositionMin,BeginPositionMax));
			pos += player.transform.position;
			GameObject n = Random.Range(1,3)==1?AsteroidPrefab:AsteroidWithBlueCrystal;
			GameObject newAlienShip = (GameObject)Instantiate(n,pos,RandomRotation());
			newAlienShip.transform.RotateAround (player.transform.position, Vector3.up, 
				player.transform.rotation.eulerAngles.y + Random.Range(-FrontAngleMaxDelta,FrontAngleMaxDelta) );			
			count++;
			rate = GenerateRate;
		}
	}
	
	public Quaternion RandomRotation()
	{
		Vector3 v = Vector3.zero;
		v.x = Random.Range(0f,360f);
		v.y = Random.Range(0f,360f);
		v.z = Random.Range(0f,360f);
		return Quaternion.Euler(v);
	}
	
	#endregion
}
