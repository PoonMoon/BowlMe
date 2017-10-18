using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {



	private Transform ballTrans;
	private Vector3 ballStartTrans, ballIdleVector3;
	private Rigidbody myRB;
	private AudioSource myAudio;

	public bool hasBeenLaunched = false;
	public int touchAttempts = 3;
	public bool inPlay = false;

	void Start () 
	{
		myRB = this.GetComponent<Rigidbody> ();
		myAudio = GetComponent<AudioSource> ();
		myRB.useGravity = false;
		ballStartTrans = this.transform.position;
		ballIdleVector3 = new Vector3 (0, 0, 0);
	}

	public void Launch (Vector3 launchVector)
	{
		myRB.velocity = launchVector;
		myRB.useGravity = true;
		myAudio.Play ();
		hasBeenLaunched = true;
		inPlay = true;
	}

	public void Reset ()
	{
		Debug.Log ("Resetting the Ball");
		inPlay = false;
		myAudio.Stop ();
		this.transform.position = ballStartTrans;
		myRB.useGravity = false;
		myRB.velocity = ballIdleVector3;
		myRB.angularVelocity = ballIdleVector3;
		//straighten up ball so that the arrows dont move it off the parallel
		straightenUpBall ();
		touchAttempts = 3;
	}


	void straightenUpBall ()
	{
		transform.eulerAngles = new Vector3 (0, 0, 0);
		myRB.velocity = Vector3.zero;
		myRB.angularVelocity = Vector3.zero;
	}
}
