using UnityEngine;
using System.Collections;

public class AlienShipGenerator : MonoBehaviour {
	
	#region Parameters
	
	public GameObject[] AlienShipPrefab;
	public float BeginPositionMin=30f, BeginPositionMax=50f;
	public float GenerateRate = 5f;
	public float FrontAngleMaxDelta = 40f;
	public float UpDownMaxHeight = 3f;
	
	#endregion
	
	#region Variables
	
	private GameObject player;
	private float rate;
	
	#endregion
	
	#region Start, Update
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("PlayerShip");
		rate = GenerateRate;
	}
	
	// Update is called once per frame
	void Update () {
		if( player == null || player.GetComponent<Detonator>().enabled)
			return;
		
		rate -= Time.deltaTime;
		if( rate <= 0 )
		{
			Vector3 pos = new Vector3(0,Random.Range(-UpDownMaxHeight,UpDownMaxHeight),Random.Range(BeginPositionMin,BeginPositionMax));
			pos += player.transform.position;
			GameObject newAlienShip = (GameObject)Instantiate(AlienShipPrefab[Random.Range(0,AlienShipPrefab.Length)],pos,Quaternion.identity);
			newAlienShip.transform.RotateAround (player.transform.position, Vector3.up, 
				player.transform.rotation.eulerAngles.y + Random.Range(-FrontAngleMaxDelta,FrontAngleMaxDelta) );			
			rate = GenerateRate;
		}
	}
	
	#endregion
}
