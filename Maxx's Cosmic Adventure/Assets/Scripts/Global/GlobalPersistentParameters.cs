using UnityEngine;
using System.Collections;

public class GlobalPersistentParamters : MonoBehaviour {
	
	#region Enums
	
	public enum BuildPlatform
	{
		Android,
		iOS,
		Amazon
	}
	
	#endregion
	
	#region Global Parameters
	
	public static BuildPlatform targetPlatform = BuildPlatform.Android;
	
	public static string appName = "Maxx's Cosmic Adventure";
	public static string appID = "com.bonozo.MCA";
	
	#endregion
	
	#region Methods
	
	public static string AppWebLink()
	{
		switch(GlobalPersistentParamters.targetPlatform)
		{
		case GlobalPersistentParamters.BuildPlatform.Android:
			return @"https://play.google.com/store/apps/details?id="+appID;
		case GlobalPersistentParamters.BuildPlatform.iOS:
			Debug.LogError("Itunes link is not determined correctly");
			return @"http://itunes.com/apps/maxxscosmicadventure";
		case GlobalPersistentParamters.BuildPlatform.Amazon:
			return @"http://www.amazon.com/gp/mas/dl/android?p="+appID;
		default:
			Debug.LogError("MCA local: App web link is not determined for current target platform");
			return "";
		}
	}
	
	#endregion
}
