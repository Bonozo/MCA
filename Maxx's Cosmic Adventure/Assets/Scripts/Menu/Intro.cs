using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	public float wait = 2f;
	
	IEnumerator Start()
	{
		yield return new WaitForSeconds(wait);
		Application.LoadLevel("mainmenu");
	}
}
