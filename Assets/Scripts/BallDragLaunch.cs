using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Ball))]
public class BallDragLaunch : MonoBehaviour {

	public float difficulty;

	private Ball myBall;
	private Vector3 startVector, endVector, launchVector;
	private float dragStartTime, swipeTimeTaken;
	private LaneScript bowlingLane;
	private float laneWidth;

	// Use this for initialization
	void Start () {
		myBall = GetComponent<Ball> ();
		bowlingLane = FindObjectOfType <LaneScript> ();
		laneWidth = bowlingLane.transform.localScale.x;
		Debug.Log ("Lane width is " + laneWidth);
	}


	public void DragStart(){
		//capture time and position of drag start
		//get vector of start position
		startVector = Input.mousePosition;
		//Debug.Log ("start drag pos is " + startVector);

		//get time of mouse down
		dragStartTime = Time.timeSinceLevelLoad;
		//Debug.Log ("start time of drag is " + dragStartTime);
	
	}

	public void DragEnd(){

		//get the ending vector
		endVector = Input.mousePosition;

		//work out the launch vector
		launchVector = endVector-startVector;

		//get the ending time
		swipeTimeTaken = Time.timeSinceLevelLoad-dragStartTime;
	
		//work out launch speed, speed = distance travelled diveded by time taken
		float launchSpeedZ = launchVector.y / swipeTimeTaken;
		float launchSpeedX = (launchVector.x / swipeTimeTaken) / difficulty;

		launchVector = new Vector3 (launchSpeedX, 0,launchSpeedZ);

		//launch the ball


		Debug.Log ("Touch is " + myBall.touchAttempts);
		if (myBall.touchAttempts > 0) {
				myBall.Launch (launchVector);
				myBall.touchAttempts--;
			}

	}

	public void MoveStart(float myNudge){
	
		Debug.Log("Nudge my balls " + myNudge);
		float laneWidth = bowlingLane.transform.localScale.x;

		if (!myBall.hasBeenLaunched) {

			//this is how Ben did it
			//myBall.transform.Translate (new Vector3(myNudgeVector,0,0));

			Vector3 currentBallVector = myBall.transform.position;
			float currentBallX = currentBallVector.x;
			Vector3 newBallVector = new Vector3 (Mathf.Clamp(currentBallX + myNudge, 0-(laneWidth/2),laneWidth/2), currentBallVector.y, currentBallVector.z);
			myBall.transform.position = newBallVector;


		}
	
	}

}
