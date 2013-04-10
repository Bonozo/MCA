using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public float Wait = 3.0f;
	
	// Use this for initialization
	IEnumerator Start () {
		guiText.enabled = true;
		
		while( Wait > 0 )
		{
			Wait -= 0.016f;
			yield return null;
		}
		Application.LoadLevel(Application.loadedLevel);
	}
}
