using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneBox : MonoBehaviour {

	private PinSetter LBpinSetter;

	// Use this for initialization
	void Start () {
		LBpinSetter = FindObjectOfType<PinSetter> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerExit (Collider objLeft){
		if (objLeft.name == "Ball") {
			LBpinSetter.SetBallOutOfPlay(true);

		}
	}

}
