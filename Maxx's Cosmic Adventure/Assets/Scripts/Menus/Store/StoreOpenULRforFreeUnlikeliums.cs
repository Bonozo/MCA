using UnityEngine;
using System.Collections;

public class StoreOpenULRforFreeUnlikeliums : MonoBehaviour {
	
	public int reward;
	public string url;
	
	private bool opened = false;
	
	void Awake()
	{
		opened = PlayerPrefs.GetInt("openurl_"+url,0)==1;
		this.GetComponent<UIButton>().isEnabled = !opened;
		//gameObject.SetActive(!opened);
	}
	
	void OnPress()
	{
		if(!opened)
		{
			opened = true;
			Application.OpenURL(url);
			Store.Instance.AddUnlikeliums(reward);
			PlayerPrefs.SetInt("openurl_"+url,opened?1:0);
			this.GetComponent<UIButton>().isEnabled = false;
			//gameObject.SetActive(false);
		}
	}
}
