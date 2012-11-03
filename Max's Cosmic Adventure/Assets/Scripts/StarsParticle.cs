using UnityEngine;
using System.Collections;

public class StarsParticle : MonoBehaviour {
	
	private Player player;
	public float Speed = 10f;
	
	// Use this for initialization
	void Start () {
		player = (Player)GameObject.FindObjectOfType(typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null ) return;
		transform.position = new Vector3(player.transform.position.x,0f,player.transform.position.z);
		transform.rotation = Quaternion.Euler(new Vector3(0,player.transform.rotation.eulerAngles.y,0));
		transform.Translate(Vector3.forward*Speed);
	}
}
