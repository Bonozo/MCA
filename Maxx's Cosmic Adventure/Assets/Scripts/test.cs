using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	
	public Texture tex;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		 
	}
	
	
	float percentage = 0f; // Up to you here, something like current / max
	
	void OnGUI()
	{
		//GUI.BeginGroup(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 300, 800, 600));
        //GUI.Box(new Rect(0, 0, 100, 100), "This box is now centered! - here you would put your main menu");
        //GUI.EndGroup();
		
		percentage += Time.deltaTime*0.10f;
		
		GUI.DrawTexture( new Rect( 10, 10, 100, 10 ), tex ); // Note the 100 for the width
		GUI.BeginGroup( new Rect( 10, 10, 100 * percentage, 10 ) );
		//GUI.color = Color.blue;
	    GUI.DrawTexture( new Rect( 0, 0, 100, 10 ), tex ); // Note the 100 for the width
		GUI.EndGroup();
		
		
	}
}
