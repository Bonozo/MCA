using UnityEngine;
using System.Collections;

public class OptionsSlider : MonoBehaviour {
	
	public float value;
	public string prefName;
	
	public void Init()
	{
		value = PlayerPrefs.GetFloat("options_" + prefName,value);
		this.GetComponent<UISlider>().sliderValue = value;
	}
	
	void OnSliderChange(float val)
	{
		value = val;
		PlayerPrefs.SetFloat("options_" + prefName,value);
	}
}
