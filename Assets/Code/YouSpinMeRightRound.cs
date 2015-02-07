using UnityEngine;
using System.Collections;

public class YouSpinMeRightRound : MonoBehaviour 
{
	//Y only
	public float[] AngleSelection;

	// Use this for initialization
	void Start () 
	{
		if(AngleSelection.Length == 0)
			return;

		int temp = Random.Range(0, AngleSelection.Length);

		this.transform.RotateAround(this.transform.position,this.transform.up,AngleSelection[temp]);	
	}
}
