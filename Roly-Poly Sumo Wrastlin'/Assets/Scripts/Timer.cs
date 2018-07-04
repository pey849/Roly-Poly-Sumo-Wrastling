using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	//the default time remaining
	private readonly float defaultTimeRemaining = 180f;

	//The time remaining in the game
	private float timeRemaining;

	//is the game playing?
	private bool isGamePlaying;

	// Use this for initialization
	void Start () {
		this.timeRemaining = defaultTimeRemaining;
		this.isGamePlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.isGamePlaying == true) {
			this.timeRemaining -= Time.deltaTime;

			//is the game over?
			if (Mathf.Approximately (this.timeRemaining, 0) || this.timeRemaining < 0) {
				EventManager.GameDone ();
				this.timeRemaining = 0;
				this.isGamePlaying = false;
			} else {
				EventManager.UpdateTimer (this.timeRemaining);
			}
		}
	}

	void OnEnable() {
		EventManager.startGameEvent += StartGame;
	}

	void OnDisable() {
		EventManager.startGameEvent -= StartGame;
	}

	public void StartGame() {
		this.isGamePlaying = true;
		this.timeRemaining = defaultTimeRemaining;
	}
}
