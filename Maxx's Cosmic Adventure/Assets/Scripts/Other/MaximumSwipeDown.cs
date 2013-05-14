using UnityEngine;
using System.Collections;

public class MaximumSwipeDown : MonoBehaviour {
	#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
	int maxfingers = 3;
	#endif
	float[] begin = new float[3] {0f,0f,0f};
	float[] end = new float[3] {0f,0f,0f};
	
	void Update ()
	{
		#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
		int len = Mathf.Min(Input.touchCount,maxfingers);
		for(int i=0;i<len;i++)
		{
			var touch = Input.touches[i];
			var y = touch.position.y/Screen.height;
			
			if( touch.phase == TouchPhase.Began )
			{
				begin[i]=end[i]=y;
			}
			if( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
			{
				for(int j=i;j<maxfingers-1;j++)
				{
					begin[j] = begin[j+1];
					end[j] = end[j+1];
				}
				begin[maxfingers-1] = end[maxfingers-1] = 0;
				break;
			}	
			if( touch.phase == TouchPhase.Moved )
			{
				end[i] = y;
			}
		}
		#else
		float y = Input.mousePosition.y/Screen.height;
		if(Input.GetMouseButtonDown(0))
			begin[0]=end[0]=y;
		if(Input.GetMouseButtonUp(0))
			begin[0]=end[0]=0;
		if(Input.GetMouseButton(0))
			end[0]=y;
		#endif
	}
	
	public float Value{
		get{
			#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
			float max=0f;
			for(int i=0;i<maxfingers;i++)
				if(begin[i]!=0)
					max = Mathf.Max(max,begin[i]-end[i]);
			return max;
			#else
			return begin[0]==0?0:Mathf.Max(0f,begin[0]-end[0]);
			#endif
		}
	}
}
