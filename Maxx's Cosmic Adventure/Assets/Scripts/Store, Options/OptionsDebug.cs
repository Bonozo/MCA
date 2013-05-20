using UnityEngine;
using System.Collections;

public class OptionsDebug : MonoBehaviour {
	
	public UILabel buttonName;
	void OnClick()
	{
		Options.Instance.debug = !Options.Instance.debug;
		buttonName.text = Options.Instance.debug?"OPTIONS":"DEBUG";
	}
}
