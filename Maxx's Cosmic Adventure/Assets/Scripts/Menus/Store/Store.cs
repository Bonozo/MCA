using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	
	#region Player Prefabs
	
	private int _unlikeliums;
	public int Unlikeliums{
		get{
			return _unlikeliums;
		}
		set{
			_unlikeliums=value;
			PlayerPrefs.SetInt("unlikeliums",_unlikeliums);
			guiUnlikelium.text = "" + _unlikeliums;
		}
	}
	
	void Awake()
	{
		Application.targetFrameRate = 60;
		
		Unlikeliums = PlayerPrefs.GetInt("unlikeliums",0);
		
		DontDestroyOnLoad(this.gameObject);
		
		FingerGestures.OnDragMove += HandleFingerGesturesOnDragMove;
		
		ShowStore=false;
	}
	
	#endregion
	
	#region References
	
	public GameObject gui;
	public AudioSource audioSource;
	public Camera camera2d;
	public Transform rootWeapons,rootShip;
	public UILabel guiUnlikelium;
	
	public GameObject popupUpgradePowerups;
	public GameObject popupBuyUnlikeliums;
	public GameObject popupBuyUnlikeliumsConfirmation;
	
	public UILabel popupName;
	public UISprite popupIcon;
	public UILabel popupDecsription;
	public UILabel popupCost;
	public UILabel popupBuyText;
	
	#endregion
	
	#region Public Field
	
	private bool _showStore = false;
	public bool ShowStore{
		get{
			return _showStore;
		}
		set{
			_showStore = value;
			gui.SetActive(value);
			if(_showStore)
			{
				tabWeapons = true;
				popupUpgradePowerups.SetActive(false);
				audioSource.volume = Options.Instance.volumeMusic;
				audioSource.Play();
			}
			else
			{
				if(IsPlayGame)
					LevelInfo.Settings.UpdatePurchasedItems();
				DisableAllPopups();
				SetQuitPopupButtonEnabled(true);
				audioSource.Stop();
			}
		}
	}
	
	[System.NonSerializedAttribute]
	public UpdateablePowerup _currentPowerup;
	
	#endregion

	#region Input Events
	
	public GameObject tabButtonWeapons;
	public GameObject tabButtonShip;
	
	private bool _tabWeapons=true;
	public bool tabWeapons{
		get{
			return _tabWeapons;
		}
		set{
			_tabWeapons = value;
			rootWeapons.gameObject.SetActive(value);
			rootShip.gameObject.SetActive(!value);
			var small = new Vector3(0.9f,0.9f,1f);
			var  big  = new Vector3(1.1f,1.1f,1f);
			tabButtonWeapons.transform.localScale = value?big:small;
			tabButtonShip.transform.localScale = value?small:big;
		}
	}
	public float storeWidthWeapons;
	public float storeWidthShip;
	void HandleFingerGesturesOnDragMove (Vector2 fingerPos, Vector2 delta)
	{
		if(!ShowStore||PopupActive) return;
		
		float storeWidth=tabWeapons?storeWidthWeapons:storeWidthShip;
		Transform buttonsDelta = tabWeapons?rootWeapons:rootShip;
		
		fingerPos = camera2d.transform.worldToLocalMatrix * camera2d.ScreenToWorldPoint( fingerPos );

		if( Mathf.Abs(fingerPos.y) <= 250f && storeWidth!=0)
		{
			float scrollmax = storeWidth-camera2d.pixelWidth/camera2d.pixelHeight*800;
			buttonsDelta.localPosition = new Vector3(Mathf.Clamp(buttonsDelta.localPosition.x+4f*delta.x,-scrollmax,5f),buttonsDelta.localPosition.y,buttonsDelta.localPosition.z);
		}
	}
	
	#endregion
	
	#region Powerups
	
	public UpdateablePowerup powerupSureShot;
	public UpdateablePowerup powerupMagned;
	public UpdateablePowerup powerupLighenUp;
	public UpdateablePowerup powerupFreeze;
	public UpdateablePowerup powerupShazam;
	public UpdateablePowerup powerupIntergalactic;
	public UpdateablePowerup powerupBoostFuel;
	public UpdateablePowerup powerupToughGuy;
	public UpdateablePowerup powerupFireHeating;
	public UpdateablePowerup powerupHeadStart;
	public UpdateablePowerup powerupPOW;
	public UpdateablePowerup powerupTripleTrouble;
	public UpdateablePowerup powerupBeastieBoost;
	public UpdateablePowerup powerupKeepFighting;
	public UpdateablePowerup powerupShipEngine;
	
	public readonly int[] costs = {50,250,1000,5000,25000};
	public readonly int costStored = 1000;
	
	public void Activate(UpdateablePowerup powerup)
	{
		_currentPowerup = powerup;
		
		popupName.text = powerup.labelName.text + "!";
		popupDecsription.text = powerup.description;
		popupIcon.spriteName = powerup.transform.FindChild("Icon").GetComponent<UISprite>().spriteName;

		if(_currentPowerup.stored)
		{
			popupCost.text = "YOU HAVE " + powerup.level;
			popupBuyText.text = "ADD BY " + costStored;
		}
		else
		{
			popupCost.text = "LEVEL " + powerup.level;
			popupBuyText.text = powerup.FullyUpdated?"FULLY UPGRADED":"" + costs[powerup.level] + " (LEVEL " + (powerup.level+1) + ")";
		}
		popupUpgradePowerups.SetActive(true);
	}
	
	public bool PopupActive{ get{ return popupUpgradePowerups.activeSelf||popupBuyUnlikeliums.activeSelf; }}
	
		
	public void DisableAllPopups()
	{
		popupUpgradePowerups.SetActive(false);
		popupBuyUnlikeliums.SetActive(false);
		popupBuyUnlikeliumsConfirmation.SetActive(false);
	}
	
	
	public UIButton[] quitPopupButtons;
	public void SetQuitPopupButtonEnabled(bool enable)
	{
		foreach(var button in quitPopupButtons)
			button.isEnabled = enable;
	}
	
	#endregion
	
	#region Buy More Unlikeliums Confiramtion
	
	public UIButton[] buttonsBuyMoreNeedToDisable;
	
	public void SetBuyMoreNeedDisabled(bool state)
	{
		foreach(var b in buttonsBuyMoreNeedToDisable)
			b.isEnabled = state;
	}
	
	public void AddUnlikeliums(int unlk)
	{
		StartCoroutine(AddUnlikeliumsAfter3Seconds(unlk));
	}
	
	private IEnumerator AddUnlikeliumsAfter3Seconds(int unlk)
	{
		yield return new WaitForSeconds(3f);
		Unlikeliums += unlk;
	}
	
	#endregion
	
	#region Properties
	
	public bool IsMainMenu { get { return Application.loadedLevelName=="mainmenu"; }}
	public bool IsPlayGame { get { return Application.loadedLevelName=="playgame"; }}
	
	#endregion
	
	#region  Static Instance
	
	//Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile Store _staticInstance;	
    public static Store Instance {
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(Store)) as Store;
                    if (_staticInstance == null) {
                       Debug.LogError("The Store instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion
}
