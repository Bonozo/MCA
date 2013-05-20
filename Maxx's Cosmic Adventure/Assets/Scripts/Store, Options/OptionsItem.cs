using UnityEngine;
using System.Collections;

public class OptionsItem : MonoBehaviour {

	public bool isEnabled;
	public string enabledText = "ON";
	public string disabledText = "OFF";
	public string prefName;
		
	public UISlicedSprite background;
	public UILabel backtext;
	
	public void Init()
	{
		isEnabled = PlayerPrefs.GetInt("options_" + prefName,isEnabled?1:0)==1;
		UpdateState();
	}
	
	void OnClick()
	{
		isEnabled = !isEnabled;
		PlayerPrefs.SetInt("options_" + prefName,isEnabled?1:0);
		UpdateState();
	}
	
	void UpdateState()
	{
		backtext.text = isEnabled?enabledText:disabledText;
		backtext.color = isEnabled?Color.green:Color.red;
	}
}
