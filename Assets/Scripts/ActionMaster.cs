using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMaster {

	// Frames   
	// 1   2   3   4   5     6    7     8     9     10
	// Bowls
	// 1-2 3-4 5-6 7-8 9-10 11-12 13-14 15-16 17-18 19-20-21
	// Index of Pinfalls
	// 0-1 .... 18-19-20


	public enum Action {Reset, Tidy, EndTurn, EndGame, Error}
	private int bowlNumber = 1;
	private bool strikeOnBowl19 = false;


	public static Action NextAction (List<int> pinFalls){
		//takes a list of pinFalls from GameManager and returns the next action
		//I guess then we need to get the latest pinFall from the list

		Action lastActionReturned = new Action();
		ActionMaster staticActionMaster = new ActionMaster ();

		int county = 0;
		foreach (int pinFall in pinFalls) {
			lastActionReturned = staticActionMaster.Bowl (pinFall, pinFalls);
			Debug.Log ("Pinfall index of " + county + " with pinfall "+ pinFall + " gave action of " + lastActionReturned);
			county++;
		}

		return lastActionReturned;
	}


	private  Action Bowl (int pins, List<int> pinFalls){

		if (pins < 0 || pins > 10) {throw new UnityException ("Shitnuts! ActionMaster.Bowl just had a a pin count of less than zero or more than 10!");}

		if (bowlNumber == 19 && pins == 10) {
			//we are on the edgecase of Frame 10, first ball scores a strike so player needs another go and fresh pins, send a reset and award bowl21
			strikeOnBowl19 = true;
			//Debug.Log("Score on bowl " + bowl + " is " + pins + " so Bowl21 Awarded " + bowl21Awarded);
			bowlNumber += 1;
			return Action.Reset;
		} 

		if (bowlNumber == 20) {
			
			if (strikeOnBowl19 && pins == 10) {
				//player had a strike on 19 so gets two more goes.  Send Reset if strike on 20
				bowlNumber += 1;
				return Action.Reset;
			} else if (strikeOnBowl19 && pins < 10) {
				//player had strike on 19, two more goes but needs a tidy here as they have not cleared.
				bowlNumber += 1;
				return Action.Tidy;
			}

			int scoreOf1920 = (pinFalls[18]+pinFalls[19]);//gets the sum of bowl 19 and bowl 20 (index of 18 and 19)
			if (scoreOf1920 == 10){
				//hit a spare on 20 so give another go
				bowlNumber +=1;
				return Action.Reset;
			}

			//all edge cases covered, so endgame on 20 
			return Action.EndGame;
		}


		if (bowlNumber == 21) {
			//always throw endgame on 21
			return Action.EndGame;
		}


		//if first bowl of frame
		if (bowlNumber % 2 != 0) {
			//not end of frame - odd number

			if (pins >= 0 && pins <= 10) {
				if (pins == 10) {
					//strike on first ball, so jump a bowl to first bowl of next frame
					bowlNumber += 2;
					return Action.EndTurn;
				} else {
					bowlNumber += 1;
					return Action.Tidy;
				}
			}

		} else {
			//if 2nd bowl of frame
			bowlNumber += 1;
			return Action.EndTurn;
		}

		//We have a case in the game that is not yet coded!
		throw new UnityException ("Shitnuts! ActionMaster, the bowling score controller thingy doesnt know what to do!");

	}

	public int returnBowlNumber(){
		Debug.Log ("We are on bowl number " + bowlNumber);
		return bowlNumber;
	}


}
