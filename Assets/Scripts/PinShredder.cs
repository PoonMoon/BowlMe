using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinShredder : MonoBehaviour {

	private void OnTriggerExit (Collider collider){
		GameObject thingLeft = collider.gameObject;

		if (thingLeft.GetComponent<Pin>()) {
			Destroy (thingLeft);
		}
	}

}
