using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinCountDisplay : MonoBehaviour {

	private Text pinDisplay;

	public void UpdatePinCountDisplay(int pinCount){
		pinDisplay = GetComponent<Text> ();
		pinDisplay.text = pinCount.ToString ();
		}

	public void Red (){
		pinDisplay.color = Color.red;
	}

	public void Green (){
		pinDisplay.color = Color.green;
	}
}
