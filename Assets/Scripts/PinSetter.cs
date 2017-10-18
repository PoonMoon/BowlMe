using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PinSetter : MonoBehaviour {

	//stuff set through inspector
	public PinCountDisplay pinCountDisplay;
	public BowlCountDisplay bowlCountDisplay;
	public float amountToRaisePins = 30f;
	public GameObject pinSet;

	private int lastStandingCount = -1;
	private float lastChangeTime;
	private int standingCountThisUpdateCycle;
	private int standingCountAtStart = 10;

	private bool ballLeftLaneCollider = false;
	private Ball ball;

	private ActionMaster localActionMaster = new ActionMaster ();

	void Start () {

		ball = GameObject.FindObjectOfType<Ball> ();
		pinCountDisplay.UpdatePinCountDisplay (CountStanding ());
		bowlCountDisplay.UpdateBowlCountDisplay (localActionMaster.returnBowlNumber ());
		lastChangeTime = Time.timeSinceLevelLoad;
	}
	

	void Update () {
		if (ballLeftLaneCollider){
			pinCountDisplay.Red ();
			CheckStanding ();
			}
	}

	public void SetBallOutOfPlay(bool state){
		ballLeftLaneCollider = state;
	}

	public void RaisePins(){
		
		foreach (Pin pin in GameObject.FindObjectsOfType<Pin>()) {
			pin.Raise (amountToRaisePins); 
		}
	}


	public void LowerPins(){
		
		foreach (Pin pin in GameObject.FindObjectsOfType<Pin>()) {
			pin.Lower (-amountToRaisePins); 
		}
	}


	public void RenewPins(){
		
		Instantiate (pinSet,new Vector3(0f,amountToRaisePins,1829),Quaternion.identity);
	}


	private void PinsHaveSettled(){
		Animator myAnimator;
		myAnimator = gameObject.GetComponent<Animator> ();

		int standingCount = CountStanding ();
		int pinFall = standingCountAtStart - standingCount;
		Debug.Log ("Pin fall is " + pinFall + ", StandingCountAtStartWas " + standingCountAtStart + ", and Countstanding is " + standingCount);

		//TODO creating a test List so that we can compile - remove this
		List<int> pinFalls = new List<int>();
		pinFalls.Add(5); //we bowled a five
		ActionMaster.Action action = ActionMaster.NextAction (pinFalls);



		if (action == ActionMaster.Action.Tidy) {
			myAnimator.SetTrigger ("tidy");
			standingCountAtStart=standingCount;

		} else if (action == ActionMaster.Action.Reset) {
			myAnimator.SetTrigger ("reset");
			standingCountAtStart=10;

		} else if (action == ActionMaster.Action.EndTurn) {
			myAnimator.SetTrigger ("reset");
			standingCountAtStart=10;
		}

		ResetBall (standingCountAtStart);
	}


	void ResetBall (int standingCountAtStart){

		ball.Reset ();
		lastStandingCount = -1;
		pinCountDisplay.UpdatePinCountDisplay (standingCountAtStart);
		pinCountDisplay.Green ();
		ballLeftLaneCollider = false;
		bowlCountDisplay.UpdateBowlCountDisplay (localActionMaster.returnBowlNumber ());
	}


	public int StandingCountAtStart(){
		//returns the number of pins standing as the ball enters the trigger zone
		standingCountAtStart =  CountStanding();
		return standingCountAtStart;
	}


	void CheckStanding ()	{

		//Get the standing count this frame
		standingCountThisUpdateCycle = CountStanding ();
		float timeWaited;

		//Has it changed since last frame / start?
		if (standingCountThisUpdateCycle != lastStandingCount) {
			//something has changed, reset the clock & update the display
			pinCountDisplay.UpdatePinCountDisplay (standingCountThisUpdateCycle);
			lastStandingCount = standingCountThisUpdateCycle;
			lastChangeTime = Time.timeSinceLevelLoad;
		}
		else {
			//no change, keep the clock running, check to see if 3 seconds has passed
			timeWaited = Time.timeSinceLevelLoad - lastChangeTime;
			if (timeWaited > 3.0f) {
				//3 seconds are up!
				PinsHaveSettled ();
			}
		}
	}


	private int CountStanding(){
		int standingCount = 0;
		foreach (Pin pin in GameObject.FindObjectsOfType<Pin>()) {
			if (pin.IsStanding ()) {
				standingCount++;
				} 
		}
		return standingCount;
	}



}
