using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;



[TestFixture]
public class ActionMasterTest {

	private ActionMaster.Action endTurn = ActionMaster.Action.EndTurn;
	private ActionMaster.Action tidy = ActionMaster.Action.Tidy;
	private ActionMaster.Action endGame = ActionMaster.Action.EndGame;
	private ActionMaster.Action reset = ActionMaster.Action.Reset;
	private ActionMaster localActionMaster;
	private PinSetter localPinSetter;

	[SetUp]
	public void Setup(){
	}


	[Test]
	public void T00PassingTest () {
		Assert.AreEqual (1,1);
	}

	[Test]
	public void T01Bowl1StrikeReturnsEndFrame (){
		List <int> pinFalls = new List<int> (){ 10};
		Assert.AreEqual (endTurn, ActionMaster.NextAction (pinFalls));
	}

	[Test]
	public void T02Bowl1Score8ReturnsTidy (){
		List <int> pinFalls = new List<int> (){ 8}; 
		Assert.AreEqual (tidy, ActionMaster.NextAction (pinFalls));
	}

	[Test]
	public void T03BowlSpareReturnsEndFrame(){
		List <int> pinFalls = new List<int> (){ 8, 2 };
		Assert.AreEqual (endTurn, ActionMaster.NextAction (pinFalls));
	}

	[Test]
	public void T04BowlNotSpareReturnEndFrame(){
		List <int> pinFalls = new List<int>{ 2 ,3 };
		Assert.AreEqual (endTurn, ActionMaster.NextAction (pinFalls));
	}

	[Test]
	public void T05BowlZeroFirstBallReturnsTidy(){
		List <int> pinFalls = new List<int>{0};
		Assert.AreEqual (tidy, ActionMaster.NextAction (pinFalls));
	}


	[Test]
	public void T07BowlZeroFirstAndSecondBallReturnsEndturn(){
		List <int> pinFalls = new List<int> () { 0, 0 };
		Assert.AreEqual (endTurn,ActionMaster.NextAction (pinFalls));
	}

	[Test]
	public void T08StrikeOn20ReturnsReset(){
		//on ball 19, bowls a strike so gets another two goes, check here we get onto bowl 20 with a reset
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 10};
		Assert.AreEqual (reset, ActionMaster.NextAction (pinFalls));
	}

	[Test]
	public void T09StrikeOn19and20GetsResetOn20(){
		//got a strike on 19 so check we have 2 additional goes
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 10,10};
		Assert.AreEqual (reset, ActionMaster.NextAction (pinFalls));//gets a reset on 20 if a strike on 20
	}

	[Test]
	public void T10Bowl19NotClearedReturnsTidy(){
		//on ball 19, not cleared.
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 8};
		Assert.AreEqual (tidy, ActionMaster.NextAction (pinFalls));
	}

	[Test]
	public void T11GameEndsOn21(){
		//if player gets to 21, always end game
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 8,2,1};
		Assert.AreEqual (endGame, ActionMaster.NextAction (pinFalls)); //any score should return endgame
	}

	[Test]
	public void T12SpareOn20ReturnsReset(){
		//if player gets spare on 20, have another go 
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 8,2};
		Assert.AreEqual (reset,ActionMaster.NextAction (pinFalls)); //spare - return reset
	}

	[Test]
	public void T14GameEndsOn20ifNoSpare(){
		//if player does not clear on 20, game over
		List <int> pinFalls = new List<int> ()  { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 8,1};
		Assert.AreEqual (endGame, ActionMaster.NextAction (pinFalls)); // not clear, end game
	}

	[Test]
	public void T15Strike19GivesTidyon20ifNotClear(){
		//if player gets the 21 award, but does not clear on 20, they need a tidy, not reset.
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 10,1};
		Assert.AreEqual (tidy,ActionMaster.NextAction (pinFalls)); //not clear - return tidy
	}

	[Test]
	public void T16BensBowl20Test(){
		//player has 21 awrded, bowls zero on bowl 20 - should get tidy
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 10,0};
		Assert.AreEqual (tidy, ActionMaster.NextAction (pinFalls)); //not clear - return tidy
	}

	[Test]
	public void T17BensBowl20Test2(){
		//play does not have 21 awrded, bowls zero on bowl 20 - should get end game
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0};
		Assert.AreEqual (endGame, ActionMaster.NextAction (pinFalls)); //not clear - return tidy
	}


	[Test]
	public void T18Strike20GivesReset(){
		//if player gets the 21 award, but does not clear on 20, they need a tidy, not reset.
		List <int> pinFalls = new List<int> () { 0,0 ,0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 10,10};
		Assert.AreEqual (reset,ActionMaster.NextAction (pinFalls)); 
	}

}







