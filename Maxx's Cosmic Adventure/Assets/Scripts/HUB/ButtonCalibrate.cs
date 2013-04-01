using UnityEngine;
using System.Collections;

public class ButtonCalibrate : MonoBehaviour {
	
	public bool vertified = false;
	
	void OnPress(bool isDown)
	{
		LevelInfo.Environments.playerShip.Calibrate(vertified);
	}
}
