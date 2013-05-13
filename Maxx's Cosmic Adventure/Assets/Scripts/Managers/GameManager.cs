using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	#region Game Environments
	
	public float upDownMaxHeight = 3.0f;
	
	#endregion
	
	#region Game
	
	public Camera mainCamera;
	public Player playerShip;
	public PlayerShipAnimationController playerAnimations;
	
	public GameObject HUB;
	public Generator generator;
	public Score score;
	public Pause pause;

	public MButton mButton;
	public HUBButton bButton;
	public HUBButton fireLeftButton,fireRightButton;
	
	public InfoMessage infoMessage;
	public MaximumSwipeDown maximumSwipeDown;
	
	public GameObject popupLose;
	public UILabel popupLoseLabelNames;
	public UILabel popupLoseLabelResults;
	
	public GameObject popupCalibrate;
	public UILabel gameStartTimer;
	
	public GameObject popupHeadStart;
	
	public Tutorials tutorials;
	public GameObject popupTutorial;
	public UILabel labelTutorial;
	
	public GameObject FPS;
	
	#endregion
	
	#region HUB
	
	public Overheat fuelOverheat;
	public Overheat fireOverheat;
	
	public UILabel guiPowerUpTime;
	public UIFilledSprite guiPowerupCountDown;
	
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
	public GameObject prefabUnlikeliumBronze;
	public GameObject prefabUnlikeliumSilver;
	public GameObject prefabUnlikeliumGold;
	
	#endregion
	
	#region Player Positions
	
	public Transform playerLeftFireTransform;
	public Transform playerRightFireTransform;
	public Transform playerAutoFireTransform;
	public Transform playerFlamethrowerTransform;
	
	public Transform[] posPlayerMissle;
	public Transform playerFireballTransform;
	public ParticleSystem ShockWave;
	
	#endregion
	
	
}
