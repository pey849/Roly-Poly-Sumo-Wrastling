using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum YellType {WRASTLED, SWAP, WINNER};

public class Yeller : MonoBehaviour {

	public YellType type;

	//array of player colors
	public Color[] playerColors = {};

	//the sumo dudes' animator
	protected Animator anim;
	protected Image yellImage;

	void OnEnable() {
		switch(this.type) {
			case YellType.WRASTLED:
			EventManager.wrastledEvent += Yell;
				break;
			case YellType.SWAP:
			EventManager.swapEvent += Yell;
			break;
		case YellType.WINNER:
			EventManager.displayScoreEvent += Yell;
			break;
		}
	}

	void OnDisable() {
		switch(this.type) {
		case YellType.WRASTLED:
			EventManager.wrastledEvent -= Yell;
			break;
		case YellType.SWAP:
			EventManager.swapEvent -= Yell;
			break;
		case YellType.WINNER:
			EventManager.displayScoreEvent -= Yell;
			break;
		}
	}

	// Use this for initialization
	void Start () {
		//get the stuff
		yellImage = GetComponent<Image>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//yell something at/about a player
	void Yell(int playerID) {
		if (playerID > 0) {
			//colour the image
			yellImage.color = this.playerColors[playerID-1];
			//trigger animation
			anim.SetTrigger("yell");
		}
	}
}
