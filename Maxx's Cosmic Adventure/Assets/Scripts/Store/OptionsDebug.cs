using UnityEngine;
using System.Collections;

public class OptionsDebug : MonoBehaviour {

	void OnClick()
	{
		Options.Instance.debug = !Options.Instance.debug;
	}
}
