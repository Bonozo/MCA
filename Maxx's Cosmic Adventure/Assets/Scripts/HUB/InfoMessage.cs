using UnityEngine;
using System.Collections;

public class InfoMessage : MonoBehaviour {
	
	public UILabel label;
	public ColorPlayNGUI colorPlay;
	
	private float timetoshow=0f;
	
	void OnEnable()
	{
		label.gameObject.SetActive(timetoshow>0f);
	}
	
	public void ShowMessage(string message)
	{
		label.text = message;
		colorPlay.Reset(new Color(0f,0f,0f,0f), new Color(1f,0f,0f,1f),1f,0f,0f);
		timetoshow=2f;
	}
	
	void Update()
	{
		if(timetoshow>0f) timetoshow -= Time.deltaTime;
		label.gameObject.SetActive(timetoshow>0f&&Time.timeScale!=0.0f);
	}
}
