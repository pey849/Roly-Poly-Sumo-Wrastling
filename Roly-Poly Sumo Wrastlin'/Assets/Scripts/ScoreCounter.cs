using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {

	//which player am I associated with
	public int playerID;

	//set these up
	public Sprite[] imageSprites;

	//the score text
	private Image scoreOnesDigit;
	public Image scoreTensDigit;

	// Use this for initialization
	void Start () {
		scoreOnesDigit = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		this.ScorePoins(0, this.playerID);
		EventManager.startGameEvent += StartGame;
		EventManager.scorePoinsEvent += ScorePoins;
	}

	void OnDisable() {
		EventManager.startGameEvent -= StartGame;
		EventManager.scorePoinsEvent -= ScorePoins;
	}

	public void ScorePoins(int poinsAmount, int playerID) {
		if(playerID == this.playerID) {
			int tens = poinsAmount / 10;
			int ones = poinsAmount % 10;

			//do the classic "99 is the max" thing
			if (tens > 10) {
				tens = 9;
				ones = 9;
			}

			//do tens
			if (scoreTensDigit != null) {
				if (tens == 0) {
					//tens is zero. don't show
					scoreTensDigit.enabled = false;
				} else {
					//tens is enabled and such
					scoreTensDigit.enabled = true;
					scoreTensDigit.sprite = this.imageSprites[tens];
				}
			}
			//do ones
			if (scoreOnesDigit != null) {
				scoreOnesDigit.sprite = this.imageSprites[ones];
			}
		}
	}

	public void StartGame() {
		this.ScorePoins(0, this.playerID);
	}
}
