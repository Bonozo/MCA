using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	#region Game Environments
	
	public float upDownMaxHeight = 3.0f;
	
	#endregion
	
	#region Game
	
	public Camera mainCamera;
	public Player playerShip;
	public GameObject HUB;
	public Generator generator;
	public Score score;
	public Missles missles;
	public Pause pause;
	public InfoMessage infoMessage;
	
	public GameObject FPS;
	
	#endregion
	
	#region HUB
	
	public Overheat fuelOverheat;
	public Overheat fireOverheat;
	
	public UILabel guiPowerUpTime;
	public UILabel guiDistanceTravelled;
	public UILabel guiUnlikeliums;
	
	#endregion
	
	#region Particles
	
	public GameObject particleExplosionJeeb;
	public GameObject particleExplosionAsteroid;
	public GameObject particleSpark;
	
	#endregion
	
	#region Prefabs
	
	public GameObject targetingBoxPrefab;
	
	public GameObject prefabPlayerProjectile;
	public GameObject prefabPlayerAutoFireProjectile;
	public GameObject prefabPlayerMissle;
	public GameObject prefabPlayerFireBall;
	
	public GameObject prefabUnlikelium;
	
	#endregion
	
	#region Player Positions
	
	public Transform playerLeftFireTransform;
	public Transform playerRightFireTransform;
	public Transform playerAutoFireTransform;
	
	public Transform[] posPlayerMissle;
	public ParticleSystem ShockWave;
	
	#endregion
	
	
}
