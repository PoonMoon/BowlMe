using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMaster
{



	//this bad boy's job is to give us the known scores for each frame so far.
	public static List<int> ScoreFrames (List<int> pinFalls)
	{
		//TODO take out all the debug stuff

		bool completeFrameAtEnd;
		List<int> listOfPinFalls = new List<int> (); //list to hold the pinfalls passed into SM
		List<int> frameScores = new List<int> ();//list for SM to hold the list of scores, this is the list we will send out - remember that we are scoring frames so only 10 frames / list of ten scores
		listOfPinFalls = pinFalls;//assign the given pinfalls to our local pinfall list
		int numberOfBowls = pinFalls.Count;
		int bowl1PinFall, bowl2PinFall;
		int thisFrameBowlOneIndex, thisFrameBowlTwoIndex;
		int numberOfFramesToScore = Mathf.CeilToInt (numberOfBowls / 2f);//how many frames to score do we have?

		Debug.Log ("Count of pinfalls / bowls given to SM is " + numberOfBowls);
		Debug.Log ("We have " + numberOfBowls + " bowls and " + numberOfFramesToScore + " frames to score.");

		//throw a benny if we have no bowls in the given list
		if (numberOfBowls < 1) {
			throw new UnityException ("Badgers! Scoremaster has been given an empty list of bowls to score!");
		}


		//finds out if we have a complete frame to score at the end of the list of bowls
		if (numberOfBowls % 2 == 0) {
			completeFrameAtEnd = true;
		} else {
			completeFrameAtEnd = false;
		}

		//iterate through the frames to score
		for (int i = 1; i <= numberOfFramesToScore; i++) {
			Debug.Log ("scoring ----> Frame " + i);


			//for each frame we are scoring, get bowl1 and bowl2 and then decide what to do
			if (i == numberOfFramesToScore) {
				//we are on the last frame to score, so check for the complete falg

				if (!completeFrameAtEnd) {
					Debug.Log ("We are an  inncomplete Ending Frame!");
					frameScores.Add (-1);
					return frameScores;
				}
			} //end Last Frame to Score catch for incomplete frame

			//we can score both balls
			Debug.Log ("Complete Frame - scoring both balls");

			thisFrameBowlOneIndex = (i * 2) - 2; //-2 & -1 because the index starts at zero.
			thisFrameBowlTwoIndex = (i * 2) - 1;
			bowl1PinFall = listOfPinFalls [thisFrameBowlOneIndex];
			bowl2PinFall = listOfPinFalls [thisFrameBowlTwoIndex];

			if (i == 10){
				//we need to catch Frame10 and treat that differently
				Debug.Log ("Frame 10 Baby!");
				int pinFall21 = bowl21Score (listOfPinFalls);

				//TODO We do not need this conditional
				if (pinFall21!= -1) {
					//We have a bowl 21
					Debug.Log ("BOWL 21 AWARDED and Found pinfall of " + pinFall21);
					//Grab the other pinfalls, add them - should be job done

				} else {
					Debug.Log ("No pinfall for 21 found " + pinFall21);

				}

				//frame 10 - jump out here as we have all we need - skip all below
				frameScores.Add (pinFall21 + listOfPinFalls [18] + listOfPinFalls [19]);
				return frameScores;
					
			}

			//not frame ten so go through usual scoring patterns
			//check for strike on ball1
			if (bowl1PinFall == 10) {
				//strike! Skip the next ball, it will be a zero. But check for ball after that
				Debug.Log ("Strike on first bowl of iteration " + i);

				int nextFrameFirstBall = GetNextPinFall (listOfPinFalls, thisFrameBowlOneIndex + 2); //  [x][0]  [?][]  [][] finding, -1 indicates no pinfall recorded (shot not yet taken)
				if (nextFrameFirstBall != -1) {
					//Got a result so check for another strike
					Debug.Log ("Next Frame Ball one, with index of " + (thisFrameBowlOneIndex + 2) + " is a scoring ball of " + nextFrameFirstBall);
					if (nextFrameFirstBall == 10) {
						//Another Strike
						//need to check the third ball, index of 4 ahead to see if thats a known score, if it is (even a strike) we can score this one
						//  [x][0]  [x][0]  [?][]
						//so this frame, we need to set the framescore to unknown
						int twoBallsFromStikeScore = GetNextPinFall (listOfPinFalls, thisFrameBowlOneIndex + 4);
						if ( twoBallsFromStikeScore != -1) {
							Debug.Log ("Storing Score of 10 plus two balls beyond strike conditional");
							frameScores.Add (10 + 10 + twoBallsFromStikeScore);
						} else {
							Debug.Log ("Storing Score of unknown at two balls beyond strike conditional");
							frameScores.Add (-1);
						}

					} else { //Next Frame's first ball not a strike, so check nextFrame2ndBall - if that's known we can score now
						//  [x][0]  [5][?]  [][]
						int nextFrame2ndBowl = GetNextPinFall (listOfPinFalls, thisFrameBowlOneIndex + 3);
						if (nextFrame2ndBowl != -1) {
							//we have a result two ahead
							//score for this frame is 10 (strike) plus those next two balls
							Debug.Log ("Storing Score of strike plus two balls " + nextFrameFirstBall + " + " +  nextFrame2ndBowl + " + 10");
							frameScores.Add (nextFrameFirstBall + nextFrame2ndBowl + 10);
						} else {
							//two ahead not known, so dont score this frame yet
							// [x][0] [5][-1] [][]
							Debug.Log ("Storing Score of unknown at two ahead unknown");
							frameScores.Add (-1);
						}
					}
				} //end next frame first ball check
				else //else The first Ball of next frame is unknown
				{
					Debug.Log ("Storing Score of unknown the else catch for first ball next frame");
					frameScores.Add (-1);
				}

			} //end Strike conditionals
			else { //usual scoring, not strike
				Debug.Log ("Scoring index of ListofBowls " + thisFrameBowlOneIndex + " " + thisFrameBowlTwoIndex);
				Debug.Log ("Scorse for those ListofBowls " + bowl1PinFall + " " + bowl2PinFall);

				if ((bowl1PinFall + bowl2PinFall) == 10) {
					//we have a spare 
					//do we know the next bowl yet?
					int nextBowlPinFall = GetNextPinFall (listOfPinFalls, thisFrameBowlTwoIndex + 1);
					
					if (nextBowlPinFall != -1) {
						//calc the score using 10 + next ball score
						frameScores.Add (10 + nextBowlPinFall);
					} else {
						//we dont know the next bowl yet so return unkown for now
						frameScores.Add (-1);
					}
				} else {
					//not a spare so just add the pinfalls and make that the score for this frame
					frameScores.Add (bowl1PinFall + bowl2PinFall);
				}

			}//end else usual scoring

		} //end interation through bowls

		//send back all the known scores
		return frameScores;

		//We have a case in the game that is not yet coded!
		throw new UnityException ("Badgers! ScoreMaster does not know what to do with given scores!");
	}


	public static void PublicListScores (List<int> scores)
	{
		int county = 1;
		foreach (int score in scores) {
			Debug.Log ("Scores " + county + " has a score of " + score);
			county++;
		}
	}

	private static int GetNextPinFall (List<int> listOfPinFalls, int bowlToCheckIndex)
	{
		//takes the list and returns the pinfall of the next ball
		List<int> pinFalls = listOfPinFalls;

		Debug.Log ("GetNext stats : Count of pinfalls is " + pinFalls.Count + " and index I am checking is " + bowlToCheckIndex);

		if (pinFalls.Count > bowlToCheckIndex) {
			Debug.Log ("Ball " + bowlToCheckIndex + " has a score");
			return pinFalls [bowlToCheckIndex];
		} else {
			Debug.Log ("Ball " + bowlToCheckIndex + " has NO score");
			return -1;
		}
	}

	private static int bowl21Score(List<int> listOfPinFalls){
		//counts the pinfalls to see if we have a bowl 21 (index of 20)
		if (listOfPinFalls.Count == 21) {
			return listOfPinFalls[21-1];
		} else {
			return -1;
		}
	}

}
