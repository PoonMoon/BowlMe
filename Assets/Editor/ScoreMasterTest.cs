using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


[TestFixture]
public class ScoreMasterTest {

	private List<int> frameScoreList;
	private List<int> accumulativeScoreByFrame;
	private List<int> listOfBowls;

	[SetUp]
	public void Setup()
	{
		//list to hold the list of scores returned from SM
		frameScoreList = new List<int>();

		//list to hold the pinFall of bowls we send to SM
		listOfBowls = new List<int>();

		//stores the totals so far
		accumulativeScoreByFrame = new List<int> (){0,0,0,0,0,0,0,0,0,0};

	}

	[Test]
	public void T00PassingTest () {
		Assert.AreEqual (1,1);

	}

	[Test]
	public void T01FirstBowlReturnsScoreUnknown () {
		//First ball cannot return a score yet
		listOfBowls.Clear();
		listOfBowls.Add(3);
		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (-1, frameScore(1));
	}

	[Test]
	public void T02TwoBowlsReturnSumIfNotSpare () {
		
		listOfBowls.Clear();
		listOfBowls.Add(3);
		listOfBowls.Add(4);
		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (7, frameScore(1));
	}

	[Test]
	public void T03AnyIncompleteFrameReturnsUnknownOnLastFrameScored () {

		listOfBowls.Clear();
		//frame1
		listOfBowls.Add(3);
		listOfBowls.Add(4);
		//frame2
		listOfBowls.Add(3);
		listOfBowls.Add(4);
		//frame3
		listOfBowls.Add(3);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (-1, frameScore(3));
	}

	[Test]
	public void T04CompleteNonStrikeNonSpareFrameKnowsLastFrameScore () {
		//check that if we have a non-spare non strike complete frame, we get a valid score back for that and the last frame
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(4);
		listOfBowls.Add(4);
		//frame2
		listOfBowls.Add(2);
		listOfBowls.Add(2);
		//frame3
		listOfBowls.Add(3);
		listOfBowls.Add(3);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);

		Assert.AreEqual (4, frameScore(2));
		Assert.AreEqual (6, frameScore(3));

	}

	[Test]
	public void T05SpareOnFrameReturnsUnknownScore () {
		//if we get a spare on a frame, that frame's score should be unknown at that point
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(4);
		listOfBowls.Add(6);
		//spare

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		Assert.AreEqual (-1, frameScore(1));

	}

	[Test]
	public void T06NonStrikeOnFrameAfterSpareReturnsScoreForPrevious () {
		//if we get a spare on a frame, that frame's score should be unknown at that point
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(4);
		listOfBowls.Add(6);
		//spare

		//frame2
		listOfBowls.Add(5);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		Assert.AreEqual (15, frameScore(1));

	}

	[Test]
	public void T07SpareAndNotClearSecondFrameReturnsScores () {
		//if we get a spare on a frame, that frame's score should be unknown at that point
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(4);
		listOfBowls.Add(6);
		//spare

		//frame2
		listOfBowls.Add(5);
		listOfBowls.Add(1);

		//frame3
		listOfBowls.Add(5);
		listOfBowls.Add(2);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (15, frameScore(1));
		Assert.AreEqual (6, frameScore(2));
		Assert.AreEqual (7, frameScore(3));

	}

	[Test]
	public void T07ZeroReturnsNoScoreNotUnknown () {
		//if we get a spare on a frame, that frame's score should be unknown at that point
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(0);
		listOfBowls.Add(0);
		//spare

		//frame2
		listOfBowls.Add(0);
		listOfBowls.Add(0);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (0, frameScore(2));
	}

	[Test]
	public void T08StrikeReturnsUnknown () {
		//if we get a spare on a frame, that frame's score should be unknown at that point
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		//strike

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (-1, frameScore(1));
	}

	[Test]
	public void T09NonSpareAfterStrikeCalcsScoreForThatFrame () {
		//if we get a spare on a frame, that frame's score should be unknown at that point
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//TODO when we have a strike, Game Manager needs to fill the next PinFall
		//strike

		//frame2
		listOfBowls.Add(2);
		listOfBowls.Add(2);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (4, frameScore(2));
	}

	[Test]
	public void T10StrikeShouldNotScoreOnNextBall() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in.
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//TODO when we have a strike, Game Manager needs to fill the next PinFall
		//strike

		//frame2
		listOfBowls.Add(2);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (-1, frameScore(1));
	}

	[Test]
	public void T11StrikeShouldNotScoreOnNext2Ball() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in.
		//first case
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//TODO when we have a strike, Game Manager needs to fill the next PinFall
		//strike

		//frame2
		listOfBowls.Add(2);
		listOfBowls.Add(3);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		Assert.AreEqual (15, frameScore(1));
		Assert.AreEqual (5, frameScore(2));
	}

	[Test]
	public void T12StrikeThenSpareScores20() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in. Two Balls done, so score the previous frame (one)
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//TODO when we have a strike, Game Manager needs to fill the next PinFall
		//strike

		//frame2
		listOfBowls.Add(2);
		listOfBowls.Add(8);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (20, frameScore(1));
	}

	[Test]
	public void T14StrikeThenSpareScoresReturnsUnknown() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in. Here we are checking frame 2 score is unknown
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//strike

		//frame2
		listOfBowls.Add(2);
		listOfBowls.Add(8);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (20, frameScore(1));
		Assert.AreEqual (-1, frameScore(2));
	}

	[Test]
	public void T15DoubleStrikeReturnsUnknownOnFirstFrame() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in.
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//strike

		//frame2
		listOfBowls.Add(10);
		listOfBowls.Add(0);
		//strike

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		Assert.AreEqual (-1, frameScore(1));
	}

	[Test]
	public void T16DoubleStrikeReturnsUnknownOnSecondFrame() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in so this should return no score
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//strike

		//frame2
		listOfBowls.Add(10);
		listOfBowls.Add(0);
		//strike

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		Assert.AreEqual (-1, frameScore(2));
	}

	[Test]
	public void T17TwoStrikesThenPinfallReturnsFirstStrikeScoring() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in, so this should now return a score.
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//strike

		//frame2
		listOfBowls.Add(10);
		listOfBowls.Add(0);
		//strike

		//frame3
		listOfBowls.Add(2);
	
		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		Assert.AreEqual (22, frameScore(1));
	}

	[Test]
	public void T18ThreeStrikesReturnScoreOnFirst() {
		//After a strike, we need two more balls before we calc the score for the frame the strike was in, so this should return a score
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(10);
		listOfBowls.Add (0);
		//strike

		//frame2
		listOfBowls.Add(10);
		listOfBowls.Add(0);
		//strike

		//frame3
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		Assert.AreEqual (30, frameScore(1));
	}

	[Test]
	public void T19YouTubeExampleScoreCardUpToFrame8() {
		//using the youtube example, check up to frame 8 is correct (ignores the frame 10 exception cases)
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(8);
		listOfBowls.Add (2);
		//strike

		//frame2
		listOfBowls.Add(7);
		listOfBowls.Add(3);
		//strike

		//frame3
		listOfBowls.Add(3);
		listOfBowls.Add(4);

		//frame4
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame5
		listOfBowls.Add(2);
		listOfBowls.Add(8);

		//frame6
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame7
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame8
		listOfBowls.Add(8);
		listOfBowls.Add(0);

		//frame9
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		//listScores ();
		addUpScores ();
		Assert.AreEqual (131, accumulativeScoreByFrame[8-1]);
	}

	[Test]
	public void T20HandleFrame10WithNoBall21Awarded() {
		//using the YouTube example with the last frame (10) altered so that only 2 bowls were made. i.e. no ball 21
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(8);
		listOfBowls.Add (2);
		//strike

		//frame2
		listOfBowls.Add(7);
		listOfBowls.Add(3);
		//strike

		//frame3
		listOfBowls.Add(3);
		listOfBowls.Add(4);

		//frame4
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame5
		listOfBowls.Add(2);
		listOfBowls.Add(8);

		//frame6
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame7
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame8
		listOfBowls.Add(8);
		listOfBowls.Add(0);

		//frame9
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame10
		listOfBowls.Add(8);
		listOfBowls.Add(1); //nudged down so that frame 10 does not award ball 21
	
		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		addUpScores ();
		Assert.AreEqual (150, accumulativeScoreByFrame[9-1]);
	}

	[Test]
	public void T21HandleFrame10WithBall21Awarded() {
		//the full youtube example
		listOfBowls.Clear();

		//frame1
		listOfBowls.Add(8);
		listOfBowls.Add (2);
		//strike

		//frame2
		listOfBowls.Add(7);
		listOfBowls.Add(3);
		//strike

		//frame3
		listOfBowls.Add(3);
		listOfBowls.Add(4);

		//frame4
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame5
		listOfBowls.Add(2);
		listOfBowls.Add(8);

		//frame6
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame7
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame8
		listOfBowls.Add(8);
		listOfBowls.Add(0);

		//frame9
		listOfBowls.Add(10);
		listOfBowls.Add(0);

		//frame10
		listOfBowls.Add(8);
		listOfBowls.Add(2); 
		listOfBowls.Add(9);

		frameScoreList = ScoreMaster.ScoreFrames (listOfBowls);
		listScores ();
		addUpScores ();
		Assert.AreEqual (170, accumulativeScoreByFrame[10-1]);
	}


	private void listScores (){
		int county = 1;
		foreach (int score in frameScoreList) {
			Debug.Log ("SMTest says frame score " + county + " scored " + score);
			county++;
		}
	}



	private void addUpScores (){
		int county = 1;
		int runningScore = 0;
		foreach (int score in frameScoreList) {
			
			runningScore += frameScore (county);
			accumulativeScoreByFrame[county-1] = runningScore;
			county++;
		}
		listAccumScores ();

	}

	private void listAccumScores (){
		int county = 1;
		foreach (int score in accumulativeScoreByFrame) {
			Debug.Log ("SMTest says Accum Scores are " + county + " total is " + score);
			county++;
		}
	}

	private int frameReference (int gameFrameNumber){
		//takes in the frame number and returns the index you need for FrameScoreList
		int indexFrameNumber = gameFrameNumber - 1;
		return indexFrameNumber;
	}

	private int frameScore (int gameFrameNumber){
		//takes in the frame number and returns score for that frame
		return frameScoreList[frameReference(gameFrameNumber)];
	}



}
