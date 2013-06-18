using UnityEngine;
using System.Collections;

public class OptionsResetPlayerPrefabs : MonoBehaviour {

	void OnClick()
	{
		PlayerPrefs.DeleteAll();
	}
}
