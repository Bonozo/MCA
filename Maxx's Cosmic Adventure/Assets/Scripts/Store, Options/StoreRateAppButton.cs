using UnityEngine;
using System.Collections;

public class StoreRateAppButton : MonoBehaviour {

	public int reward;
	
	private bool opened = false;
	
	void Awake()
	{
		opened = PlayerPrefs.GetInt("rateapp",0)==1;
		this.GetComponent<UIButton>().isEnabled = !opened;
		
		var sprite = transform.Find("Icon").GetComponent<UISprite>();
		switch(GlobalPersistentParamters.targetPlatform)
		{
		case GlobalPersistentParamters.BuildPlatform.Android:
			sprite.spriteName = "icon_play";
			break;
		case GlobalPersistentParamters.BuildPlatform.iOS:
			sprite.spriteName = "icon_itunes";
			break;
		case GlobalPersistentParamters.BuildPlatform.Amazon:
			sprite.spriteName = "icon_amazon";
			break;
		default:
			Debug.LogError("There is no icon of current build platform for rate app button in store");
			break;
		}
	}
	
	void OnPress()
	{
		if(!opened)
		{
			opened = true;
			Application.OpenURL(GlobalPersistentParamters.AppWebLink());
			Store.Instance.AddUnlikeliums(reward);
			PlayerPrefs.SetInt("rateapp",opened?1:0);
			this.GetComponent<UIButton>().isEnabled = false;
		}
	}
}
