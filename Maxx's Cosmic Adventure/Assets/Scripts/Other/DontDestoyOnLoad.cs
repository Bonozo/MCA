using UnityEngine;
using System.Collections;

public class DontDestoyOnLoad : MonoBehaviour {

	void Awake()
	{
		Debug.Log("lenght = " + GameObject.FindGameObjectsWithTag("SoundObject").Length);
		if( GameObject.FindGameObjectsWithTag("SoundObject").Length > 1 )
			Destroy(this.gameObject);
		else
			DontDestroyOnLoad(gameObject);
	}
}
