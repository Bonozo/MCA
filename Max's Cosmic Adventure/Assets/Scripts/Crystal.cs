using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {
	
	public float RotateSpeed = 10f;
	private float DestroyDistance = 150f;
	
	private Vector3 rot = new Vector3(90f,172.346f,0);
	
	private Player player;
	
	// Use this for initialization
	void Start () {
		
		player = (Player)GameObject.FindObjectOfType(typeof(Player));
		if( player == null )
		{
			Destroy(this.gameObject);
			return;
		}
		
		transform.rotation = Quaternion.Euler(rot);
		Vector3 pos = transform.position;
		pos.y = Random.Range(-player.UpDownMaxHeight,player.UpDownMaxHeight);
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,RotateSpeed*Time.deltaTime);
		if(player == null || Vector3.Distance(transform.position,player.transform.position) >= DestroyDistance )
			Destroy(this.gameObject);
	}
}
