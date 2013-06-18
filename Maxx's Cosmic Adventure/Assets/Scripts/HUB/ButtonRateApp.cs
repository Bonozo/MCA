using UnityEngine;
using System.Collections;

public class ButtonRateApp : MonoBehaviour {
	
	public enum RateAppButtons
	{
		Rate,
		NotNow,
		Never
	}
	
	public static bool pushedActionIsRestart=false;
	public RateAppButtons buttonAction;
	
	void OnClick()
	{
		switch(buttonAction)
		{
		case RateAppButtons.Rate:
			Application.OpenURL(GlobalPersistentParamters.AppWebLink());
			PlayerPrefs.SetInt("rateapppopupcompleted",1);
			break;
		case RateAppButtons.NotNow:
			break;
		case RateAppButtons.Never:
			PlayerPrefs.SetInt("rateapppopupcompleted",1);
			break;
		}
		
		if(pushedActionIsRestart)
			Application.LoadLevel(Application.loadedLevel);
		else
			Application.LoadLevel("mainmenu");
	}
}
