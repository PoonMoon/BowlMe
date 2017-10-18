using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlCountDisplay : MonoBehaviour {

	private Text bowlDisplay;

	public void UpdateBowlCountDisplay(int bowlCount){
		bowlDisplay = GetComponent<Text> ();
		bowlDisplay.text = bowlCount.ToString ();
		}

}
