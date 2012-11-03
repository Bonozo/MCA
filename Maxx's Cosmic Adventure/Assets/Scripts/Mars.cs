using UnityEngine;
using System.Collections;

public class Mars : MonoBehaviour {
	
	public float Distance = 1000f;
	private Player player;
	
	// Use this for initialization
	void Start () {
		player = (Player)GameObject.FindObjectOfType(typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
		if( player != null )
		{
			Vector3 vec = player.transform.position;
			vec.z += Distance;
			vec.y = 0;
			transform.position = vec;
		}
	}
}
