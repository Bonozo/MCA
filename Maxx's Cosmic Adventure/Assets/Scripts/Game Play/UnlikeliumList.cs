using UnityEngine;
using System.Collections;

public class UnlikeliumList : MonoBehaviour {
	
	private bool reported=false;
	void Update()
	{
		if(!reported&&transform.childCount==0)
		{
			// reporting
			Stats.Instance.ReportCollectedFullUnlikeliumList();
			reported=true;
		}
	}
}
