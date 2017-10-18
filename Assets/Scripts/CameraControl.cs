using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {


	public Ball myBall;


	private Vector3 offset;
	private Vector3 ballPosition;

	// Use this for initialization
	void Start () {
		offset = new Vector3 (0, 20, -50);
		getBallPos ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (getBallPos ().z <= 1829) {
			this.transform.position = ballPosition + offset;
		}
	}

	Vector3 getBallPos ()
	{
		ballPosition = myBall.transform.position;
		return ballPosition;
	}
}
