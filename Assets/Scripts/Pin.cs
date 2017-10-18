using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {

	public float pinStandingThreshold;
	public float wierdPinRotation = 270f;
	private Rigidbody myRB;

	public void Start(){
		myRB = GetComponent<Rigidbody>();

	}


	public void Raise(float amountToMovePin){
		if (IsStanding ()) {
			myRB.useGravity = false;
			//transform.Translate(0f, amountToMovePin,0f,Space.World);
			transform.Translate (Vector3.up * amountToMovePin ,Space.World);
			StraightenUpPin ();

		} 
	}


	public void StraightenUpPin ()
	{
		//straighten up the pin
		transform.rotation = Quaternion.Euler (270f,0,0);
		myRB.velocity = Vector3.zero;
		myRB.angularVelocity = Vector3.zero;
	}


	public void Lower(float amountToMovePin){
			//transform.Translate(0f,0f,0f,Space.World);
			StraightenUpPin ();
			myRB.useGravity = true;
	}


	public bool IsStanding (){
		//detects what angle the pin is at and wether it should be classed as standing or not
		//uses Eulor angles shizzle
		//as this is attached to a pin, just return the state of standing for This.Pin
		//we need to look at the euler angles on Z and X, lets say the threshold is 45 degrees
		Vector3 rotationInEuler = this.transform.rotation.eulerAngles;
		float pinAngleX = wierdPinRotation-Mathf.Abs(rotationInEuler.x);
		float pinAngleZ = Mathf.Abs(rotationInEuler.z);

		if (pinAngleX < pinStandingThreshold && pinAngleX > (0 - pinStandingThreshold)) {
			//for a pinthreshold of 45 degrees the acceptable angles are 0 to 45, or 315 to 360
			//X is OK. So check Z, if X is not OK no need to check Z as well....
			//now check Z
			//if (pinAngleZ < pinStandingThreshold && pinAngleZ > (0 - pinStandingThreshold)) {
			//skipping the z check as the eular angles seem all fucked up and only x seems to matter - realted to blender model I think
			//Z is OK too so give a true return
			return true;
			}
		return false;
	}

}

