using UnityEngine;
using System.Collections;

public class ButtonHeadStart : MonoBehaviour{
	
	void OnClick()
	{
		LevelInfo.Environments.playerShip.UseHeadStart();
	}
}
