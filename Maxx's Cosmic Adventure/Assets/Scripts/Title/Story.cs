using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Story")]
public class Story : MonoBehaviour {
	
	public AudioSource storyMusic;
	public Texture2D[] slide;
	public float slideTime = 0.1f;
	public Texture2D popUp;
	public Texture2D skip;
	
	private bool slided = false;
	private int slidedindex = 0;
	private float slidedtime = 0;
	private bool increse = false;
	
	private Color popupcolor = new Color(1f,1f,1f,1f);
	private bool showpopup = false;
	private bool firstpageslided = false;
	
	private int current = 0;
	
	public AudioClip audioPageTurn;
	
	void Awake()
	{
		var source = gameObject.AddComponent<AudioSource>();
		source.bypassEffects = true;
		source.volume = Options.Instance.volumeSFX;
		
		storyMusic.volume = Options.Instance.volumeMusic;
	}
	
	IEnumerator Start()
	{
		// show swipe popup
		while(!firstpageslided)
		{
			if(!firstpageslided)
			{
				showpopup = true;
				
				float showpopuptime = 0;
				while(showpopuptime<1f)
				{
					showpopuptime += Time.deltaTime;
					popupcolor.a = showpopuptime;
					yield return new WaitForEndOfFrame();
				}
				yield return new WaitForSeconds(4f);
				showpopuptime = 1;
				while(showpopuptime>0f)
				{
					showpopuptime -= Time.deltaTime;
					popupcolor.a = showpopuptime;
					yield return new WaitForEndOfFrame();
				}
				
				showpopup = false;
				yield return new WaitForSeconds(10f);
			}
		}
	}
	
	void Update()
	{
		if( slided )
		{
			slidedtime += Time.deltaTime;
			if( slidedtime >= slideTime )
				slided = false;
			return;
		}
		
		if( GameEnvironment.Swipe.x > 0f )
		{
			GetComponent<AudioSource>().PlayOneShot(audioPageTurn);
			current--;
			if( current < 0 ) current = 0;
			else 
			{
				slided = true;
				slidedindex = current;
				slidedtime = 0f;
				increse = false;
			}
		}
		if( GameEnvironment.Swipe.x < 0f )
		{
			GetComponent<AudioSource>().PlayOneShot(audioPageTurn);
			current++;
			firstpageslided = true;
			if( current == slide.Length )
			{
				current--;
				Application.LoadLevel("load");
			}
			else
			{
				slided = true;
				slidedindex = current-1;
				slidedtime = 0f;
				increse = true;
			}
		}
	}
	
	
	void OnGUI()
	{
		if( slided )
		{
			Rect rect = new Rect(0,0,Screen.width,Screen.height);
			if( increse ) rect.x = -slidedtime*Screen.width/slideTime;
			else  rect.x = -(slideTime-slidedtime)*Screen.width/slideTime;
			GUI.DrawTexture(rect, slide[slidedindex]);
			rect.x += Screen.width;
			GUI.DrawTexture(rect, slide[slidedindex+1]);
		}
		else
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), slide[current] );
		
		GUI.backgroundColor = Color.clear;
		
		if( GUI.Button(new Rect(Screen.width*0.85f,Screen.height*0.9f,Screen.width*0.15f,Screen.height*0.1f),skip) )
			Application.LoadLevel("load");
		
		GUI.color = popupcolor;
		
		if(showpopup && !firstpageslided)
			GUI.DrawTexture(new Rect(Screen.width*0.35f,Screen.height*0.35f,Screen.width*0.3f,Screen.height*0.3f),popUp,ScaleMode.StretchToFill);	
	}
}
