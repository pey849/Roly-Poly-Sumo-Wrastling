using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public bool debugMessages;

	//Controller 1
    SumoInputs rightSideController_1;
	SumoInputs leftSideController_1;

	//Controller 2
	SumoInputs rightSideController_2;
	SumoInputs leftSideController_2;

	//All triggers are released at the beginning
	bool leftTriggerReleased_1 = true;
	bool rightTriggerReleased_1 = true;
	bool leftTriggerReleased_2 = true;
	bool rightTriggerReleased_2 = true;


    void Start() {
		rightSideController_1 = new SumoInputs();
		leftSideController_1 = new SumoInputs();
		rightSideController_2 = new SumoInputs();
		leftSideController_2 = new SumoInputs();
    }

	// Update is called once per frame
	void Update () {

		//All Joystick input from Controller 1
		float horLeftStick_1 = Input.GetAxis ("Horizontal-LeftStick1");
		float horRightStick_1 = Input.GetAxis ("Horizontal-RightStick1");
		float vertLeftStick_1 = Input.GetAxis ("Vertical-LeftStick1");
		float vertRightStick_1 = Input.GetAxis ("Vertical-RightStick1");

		//All Joystick input from Controller 2
		float horLeftStick_2 = Input.GetAxis ("Horizontal-LeftStick2");
		float horRightStick_2 = Input.GetAxis ("Horizontal-RightStick2");
		float vertLeftStick_2 = Input.GetAxis ("Vertical-LeftStick2");
		float vertRightStick_2 = Input.GetAxis ("Vertical-RightStick2");

		//Set controller movement from Controller 1
		rightSideController_1.setMovemt (horRightStick_1, vertRightStick_1);
		leftSideController_1.setMovemt (horLeftStick_1, vertLeftStick_1);

		//Set controller movement from Controller 2
		rightSideController_2.setMovemt (horRightStick_2, vertRightStick_2);
		leftSideController_2.setMovemt (horLeftStick_2, vertLeftStick_2);

		//Check Controller 1 input buttons
		leftSideController_1.setSwappedProperty (Input.GetButtonDown("Bumper-Left1"));
		rightSideController_1.setSwappedProperty (Input.GetButtonDown("Bumper-Right1"));

		//Check Controller 2 input buttons
		leftSideController_2.setSwappedProperty (Input.GetButtonDown("Bumper-Left2"));
		rightSideController_2.setSwappedProperty (Input.GetButtonDown("Bumper-Right2"));


		//Check Controller 1 trigger buttons
		if (Input.GetAxis ("Trigger-Left1") > 0) {
			if(leftTriggerReleased_1){ 
				if (debugMessages) {Debug.Log ("Left Triger pressed");}
				leftTriggerReleased_1 = false; 
			}

		} else {
			if(!leftTriggerReleased_1){if (debugMessages) {Debug.Log ("Left Triger released");}}
			leftTriggerReleased_1 = true;
		}
			
		if (Input.GetAxis ("Trigger-Right1") > 0) {
			if(rightTriggerReleased_1){ 
				if (debugMessages) {Debug.Log ("Right Triger pressed");}
				rightTriggerReleased_1 = false; 
			}

		} else {
			if(!rightTriggerReleased_1){if (debugMessages) {Debug.Log ("Right Triger released");}}
			rightTriggerReleased_1 = true;
		}
			
		//Check Controller 2 trigger buttons
		if (Input.GetAxis ("Trigger-Left2") > 0) {
			if(leftTriggerReleased_2){ 
				if (debugMessages) {Debug.Log ("Left Triger pressed");}
				leftTriggerReleased_2 = false; 
			}

		} else {
			if(!leftTriggerReleased_2){if (debugMessages) {Debug.Log ("Left Triger released");}}
			leftTriggerReleased_2 = true;
		}

		if (Input.GetAxis ("Trigger-Right2") > 0) {
			if(rightTriggerReleased_2){ 
				if (debugMessages) {Debug.Log ("Right Triger pressed");}
				rightTriggerReleased_2 = false; 
			}

		} else {
			if(!rightTriggerReleased_2){if (debugMessages) {Debug.Log ("Right Triger released");}}
			rightTriggerReleased_2 = true;
		}

		EventManager.PassInput(leftSideController_1, rightSideController_1, leftSideController_2, rightSideController_2);
	}
}
