using UnityEngine;
using System.Collections;


//We needz it
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour 
{
	public KeyCode ForwardK;
	public KeyCode BackwardK;
	public KeyCode LeftK;
	public KeyCode RightK;

	public float Speed;

	Vector3 Force = Vector3.zero;
	bool Forced = false;

	// Update is called once per frame
	void Update () 
	{
		Force = Vector3.zero;

	    if(Input.GetKey(ForwardK))
		{
			Vector3 temp = this.transform.forward;	//get local vector
			//temp = temp + this.transform.position;	//bring to world space
			Force += temp;				//add to the forces
			Forced = true;
		}

		if(Input.GetKey(BackwardK))
		{
			Vector3 temp = -this.transform.forward;	//get local vector
			//temp = temp + this.transform.position;	//bring to world space
			Force += temp;				//add to the forces
			Forced = true;
		}

		if(Input.GetKey(LeftK))
		{
			Vector3 temp = -this.transform.right;	//get local vector
			//temp = temp + this.transform.position;	//bring to world space
			Force += temp;				//add to the forces
			Forced = true;
		}

		if(Input.GetKey(RightK))
		{
			Vector3 temp = this.transform.right;	//get local vector
			//temp = temp + this.transform.position;	//bring to world space
			Force += temp;				//add to the forces
			Forced = true;
		}

		if(Forced)
			Force.Normalize ();

		this.rigidbody.AddForce (Force * Speed * Time.deltaTime);

	}
}
