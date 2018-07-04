using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

	public delegate void PassInputAction(SumoInputs leftSide1, SumoInputs rightSide1, SumoInputs leftSide2, SumoInputs rightSide2);
	public static event PassInputAction passInputEvent;

	public delegate void PlayerOffPlatformAction(int playerID);
	public static event PlayerOffPlatformAction playerOffPlatformEvent;

	public delegate void UpdateTimerAction(float time);
	public static event UpdateTimerAction updateTimerEvent;

	public delegate void PlayGameAction();
	public static event PlayGameAction playGameEvent;

	public delegate void StartGameAction();
	public static event StartGameAction startGameEvent;

	public delegate void GameDoneAction();
	public static event GameDoneAction gameDoneEvent;

	public delegate void ScorePoinsAction(int poinAmount, int playerID);
	public static event ScorePoinsAction scorePoinsEvent;

	public delegate void WrastledAction(int playerID);
	public static event WrastledAction wrastledEvent;

	public delegate void SwapAction(int playerID);
	public static event SwapAction swapEvent;

	public delegate void DisplayScoreAction(int playerID);
	public static event DisplayScoreAction displayScoreEvent;

	public delegate void PlayAgainAction();
	public static event PlayAgainAction playAgainEvent;

	public static void PassInput(SumoInputs leftSide1, SumoInputs rightSide1, SumoInputs leftSide2, SumoInputs rightSide2) {
		passInputEvent(leftSide1, rightSide1, leftSide2, rightSide2);
	}

	public static void PlayerOffPlatform(int playerID) {
		playerOffPlatformEvent(playerID);
	}

	public static void UpdateTimer(float time) {
		updateTimerEvent(time);
	}

	public static void PlayGame() {
		playGameEvent();
	}

	public static void StartGame() {
		startGameEvent();
	}

	public static void GameDone() {
		gameDoneEvent();
	}

	public static void ScorePoins(int poinAmount, int playerID) {
		scorePoinsEvent(poinAmount, playerID);
	}

	public static void Wrastled(int playerID) {
		wrastledEvent(playerID);
	}

	public static void Swap(int playerID) {
		swapEvent(playerID);
	}

	public static void DisplayScore(int playerID){
		displayScoreEvent(playerID);
	}

	public static void PlayAgain() {
		playGameEvent();
	}
}
