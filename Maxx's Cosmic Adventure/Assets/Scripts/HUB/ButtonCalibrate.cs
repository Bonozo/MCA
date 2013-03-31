using UnityEngine;
using System.Collections;

public class ButtonCalibrate : MonoBehaviour {

	void OnPress(bool isDown)
	{
		LevelInfo.Environments.playerShip.calibrate = GameEnvironment.DeviceAngle;
	}
}
