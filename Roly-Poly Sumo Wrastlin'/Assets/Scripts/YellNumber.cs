using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellNumber : Yeller {

	//array of player colors
	public Sprite[] playerNumberSprites = {};

	void OnEnable() {		
		EventManager.displayScoreEvent += Yell;
	}

	void OnDisable() {
		EventManager.displayScoreEvent -= Yell;
	}

	//yell something at/about a player
	void Yell(int playerID) {
		if (playerID > 0) {
			//colour the image and set the appropriate image
			yellImage.sprite = this.playerNumberSprites[playerID-1];
			yellImage.color = this.playerColors[playerID-1];
			//trigger animation
			anim.SetTrigger("yell");
		}
	}
}
