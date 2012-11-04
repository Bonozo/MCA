using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	#region Game
	
	public Camera mainCamera;
	public Generator generator;
	public Score score;
	
	public float upDownMaxHeight = 3.0f;
	
	#endregion
	
	#region GUI
		
	public GUIText guiPowerUpTime;
	public GUIText guiDistanceTravelled;

	
	#endregion
	
	#region Player
	
	public Player playerShip;
	public Overheat fuelOverheat;
	public Overheat fireOverheat;
	
	#endregion
}
