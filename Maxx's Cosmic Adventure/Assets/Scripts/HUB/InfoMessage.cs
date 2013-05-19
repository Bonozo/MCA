using UnityEngine;
using System.Collections;

public class InfoMessage : MonoBehaviour {
	
	public UILabel label;
	public ColorPlayNGUI colorPlay;
	
	public Color colorStart = new Color(0.55f,0f,1f,1f);
	public Color colorEnd = new Color(0.55f,0f,1f,0.5f);
	
	private float timetoshow=0f;
	
	void OnEnable()
	{
		label.gameObject.SetActive(timetoshow>0f);
	}
	
	public void ShowMessage(string message)
	{
		label.text = message;
		colorPlay.Reset(colorStart, colorEnd,0.5f,0f,0f);
		timetoshow=1.5f;
		label.transform.localPosition = new Vector3(label.transform.localPosition.x,-500f,label.transform.localPosition.z);
	}
	
	void Update()
	{
		if(timetoshow>0f) 
		{
			timetoshow -= 0.016f;
			label.transform.localPosition = new Vector3(label.transform.localPosition.x,label.transform.localPosition.y+200f*0.016f,label.transform.localPosition.z);
		}
		label.gameObject.SetActive(timetoshow>0f&&Time.timeScale!=0.0f);
	}
}
