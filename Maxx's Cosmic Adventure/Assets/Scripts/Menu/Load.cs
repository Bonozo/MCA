using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Load")]
public class Load : MonoBehaviour {
	
	public string sceneName = "playgame";
	public UIFilledSprite filled;
	
	// Use this for initialization
	IEnumerator Start () {
		AsyncOperation opr = Application.LoadLevelAsync(sceneName);
		while(!opr.isDone)
		{
			filled.fillAmount = opr.progress;
			yield return null;
		}
		
		Destroy(this.gameObject);
	}
}
