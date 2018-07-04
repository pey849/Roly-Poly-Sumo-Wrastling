using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

	//the first sumo dude
	public SumoDudes[] sumos = new SumoDudes[2];

	//the poins for all the players
	private Poins[] poins;

	//the sumo associations
	private SumoAssociations[] sumoAsses;

	//did sumo one swap?
	private bool swappedOne = false;

	//did sumo two swap?
	private bool swappedTwo = false;

	//is the game started?
	private bool isGameStarted = false;

	// Use this for initialization
	void Start () {
		poins = new Poins[4];
		for(int i = 0; i < poins.Length; i++) {
			poins[i] = new Poins();
		}
		sumoAsses = new SumoAssociations[4];
		for(int i = 0; i < sumoAsses.Length; i++) {
			SetupSumo(i);
		}

	}

	private void SetupSumo(int index) {
		SumoRole role;
		int sumoNumber;
		//if we are the right side of the controller
		if(index % 2 != 0) {
			//determine the role based on the first player role picked for that controller
			if(sumoAsses[index - 1].role == SumoRole.SUMODUDE) {
				role = SumoRole.SECONDARY;
				if(index < 2) {
					sumos[0].SetupSecondaryDisplay(2, 0);
				} else {
					sumos[1].SetupSecondaryDisplay(4, 0);
				}
			} else {
				role = SumoRole.SUMODUDE;
				if(index < 2) {
					sumos[0].SetPlayerID(2);
				} else {
					sumos[1].SetPlayerID(4);
				}
			}
		} else {
			int rand = Random.Range(1, 3);
			if(rand == 1) {
				role = SumoRole.SUMODUDE;
				if(index < 2) {
					sumos[0].SetPlayerID(1);
				} else {
					sumos[1].SetPlayerID(3);
				}
			} else {
				role = SumoRole.SECONDARY;
				if(index < 2) {
					sumos[0].SetupSecondaryDisplay(1, 0);
				} else {
					sumos[1].SetupSecondaryDisplay(3, 0);
				}
			}
		}
		//if we are the second controller, put us on the second sumo
		if(index > 1) {
			sumoNumber = 2;
		} else {
			sumoNumber = 1;
		}
		sumoAsses[index] = new SumoAssociations(role, index + 1, sumoNumber);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate () {
		
	}

	void OnEnable() {
		EventManager.startGameEvent += StartGame;
		EventManager.playerOffPlatformEvent += PlayerOffPlatform;
		EventManager.passInputEvent += ReceiveInputs;
		EventManager.gameDoneEvent += GameDone;
		EventManager.playGameEvent += PlayAgain;
	}

	void OnDisable() {
		EventManager.startGameEvent -= StartGame;
		EventManager.playerOffPlatformEvent -= PlayerOffPlatform;
		EventManager.passInputEvent -= ReceiveInputs;
		EventManager.gameDoneEvent -= GameDone;
		EventManager.playAgainEvent -= PlayAgain;
	}

	//setup the sumos for a new game
	public void SetupSumos() {
		for(int i = 0; i < poins.Length; i++) {
			poins[i].ResetPoins();
		}
		for(int i = 0; i < sumoAsses.Length; i++) {
			SetupSumo(i);
		}

		sumos[0].ResetPlayerPosition();
		sumos[1].ResetPlayerPosition();
		sumos[0].gameObject.SetActive(true);
		sumos[1].gameObject.SetActive(true);
	}

	//when play game is called
	public void StartGame() {
		SetupSumos();
		this.isGameStarted = true;
		Debug.Log("Restarted game");
	}

	//handles the player off platform
	public void PlayerOffPlatform(int playerID) {
		
		int numOfAttachedSecondaries = 0;
		SumoAssociations sumoStayedOn = new SumoAssociations();
		for(int i = 0; i < sumoAsses.Length; i++) {
			if(sumoAsses[i].role == SumoRole.SUMODUDE && sumoAsses[i].playerID != playerID) {
				sumoStayedOn = sumoAsses[i];
			}
		}
		int sumoNumber = sumoStayedOn.sumoDudeNumber;
		for(int i = 0; i < sumoAsses.Length; i++) {
			if(sumoAsses[i].playerID != playerID && sumoAsses[i].playerID != sumoStayedOn.playerID && sumoAsses[i].sumoDudeNumber != sumoNumber) {
				poins[sumoAsses[i].playerID - 1].AddPoins(1);
				numOfAttachedSecondaries++;
				EventManager.ScorePoins(poins[sumoAsses[i].playerID - 1].GetPoinCount(), sumoAsses[i].playerID);
			}
		}
		poins[sumoStayedOn.playerID - 1].AddPoins(3 - numOfAttachedSecondaries);
		//check for a WRASTLED event
		if(3 - numOfAttachedSecondaries == 1) {
			Debug.Log ("player: "+playerID+", sumoStayedOn ID: "+sumoStayedOn.playerID);
			EventManager.Wrastled(sumoStayedOn.playerID);
		}

		StartCoroutine(WaitForSwap(playerID));

		if(playerID < 2 && swappedOne) {
			EventManager.Swap(playerID);
			swappedOne = false;
		} else if(playerID > 1 && swappedTwo) {
			EventManager.Swap(playerID);
			swappedTwo = false;
		}
		EventManager.ScorePoins(poins[sumoStayedOn.playerID - 1].GetPoinCount(), sumoStayedOn.playerID);
	}

	private bool SwapSumos(int playerID) {
		if(playerID == 1 && sumoAsses[1].sumoDudeNumber == 1) {
			//swap 1 with 2
			sumoAsses[0].role = SumoRole.SECONDARY;
			sumoAsses[1].role = SumoRole.SUMODUDE;
			sumos[0].SetPlayerID(2);
			return true;
		} else if(playerID == 2 && sumoAsses[0].sumoDudeNumber == 1) {
			//swap 2 with 1
			sumoAsses[1].role = SumoRole.SECONDARY;
			sumoAsses[0].role = SumoRole.SUMODUDE;
			sumos[0].SetPlayerID(1);
			return true;
		} else if(playerID == 3 && sumoAsses[3].sumoDudeNumber == 2) {
			//swap 3 with 4
			sumoAsses[2].role = SumoRole.SECONDARY;
			sumoAsses[3].role = SumoRole.SUMODUDE;
			sumos[1].SetPlayerID(4);
			return true;
		} else if(playerID == 4 && sumoAsses[2].sumoDudeNumber == 2) {
			//swap 4 with 3
			sumoAsses[3].role = SumoRole.SECONDARY;
			sumoAsses[2].role = SumoRole.SUMODUDE;
			sumos[1].SetPlayerID(3);
			return true;
		} else {
			return false;
		}
	}

	IEnumerator WaitForSwap(int playerID) {
		yield return new WaitForSeconds(1f);
		if(playerID < 2) {
			swappedOne = SwapSumos(playerID);
		} else {
			swappedTwo = SwapSumos(playerID);
		}
	}

	public void ReceiveInputs(SumoInputs leftSide1, SumoInputs rightSide1, SumoInputs leftSide2, SumoInputs rightSide2) {
		ArrayList sumoOneInputs = new ArrayList();
		ArrayList sumoTwoInputs = new ArrayList();
		int startingSecondaryID1 = this.GetStartingSecondaryIDOne();
		int startingSecondaryID2 = this.GetStartingSecondaryIDTwo();
		int otherSecondaryID1 = this.GetOtherSecondaryIDOne();
		int otherSecondaryID2 = this.GetOtherSecondaryIDTwo();
		if(sumoAsses[0].role == SumoRole.SECONDARY) {
			if(sumoAsses[0].sumoDudeNumber == 1) {
				sumoOneInputs.Add(leftSide1);
			} else {
				sumoTwoInputs.Add(leftSide1);
			}
			if(leftSide1.GetSwappedPrimary()) {
				//swap sumo dudes
				if(sumoAsses[0].sumoDudeNumber == 1) {
					sumoAsses[0].sumoDudeNumber = 2;
					otherSecondaryID2 = sumoAsses[0].playerID;
				} else {
					sumoAsses[0].sumoDudeNumber = 1;
					startingSecondaryID1 = sumoAsses[0].playerID;
				}
			}
			sumoOneInputs.Insert(0, rightSide1);
		} else {
			if(sumoAsses[1].sumoDudeNumber == 1) {
				sumoOneInputs.Add(rightSide1);
			} else {
				sumoTwoInputs.Add(rightSide1);
			}
			if(rightSide1.GetSwappedPrimary()) {
				if(sumoAsses[1].sumoDudeNumber == 1) {
					sumoAsses[1].sumoDudeNumber = 2;
					otherSecondaryID2 = sumoAsses[1].playerID;
				} else {
					sumoAsses[1].sumoDudeNumber = 1;
					startingSecondaryID1 = sumoAsses[1].playerID;
				}
			}
			sumoOneInputs.Insert(0, leftSide1);
		}

		if(sumoAsses[2].role == SumoRole.SECONDARY) {
			if(sumoAsses[2].sumoDudeNumber == 1) {
				sumoOneInputs.Add(leftSide2);
			} else {
				sumoTwoInputs.Add(leftSide2);
			}
			if(leftSide2.GetSwappedPrimary()) {
				//swap sumo dudes
				if(sumoAsses[2].sumoDudeNumber == 1) {
					sumoAsses[2].sumoDudeNumber = 2;
					startingSecondaryID2 = sumoAsses[2].playerID;
				} else {
					sumoAsses[2].sumoDudeNumber = 1;
					otherSecondaryID1 = sumoAsses[2].playerID;
				}
			}
			sumoTwoInputs.Insert(0, rightSide2);
		} else {
			if(sumoAsses[3].sumoDudeNumber == 1) {
				sumoOneInputs.Add(rightSide2);
			} else {
				sumoTwoInputs.Add(rightSide2);
			}
			if(rightSide2.GetSwappedPrimary()) {
				if(sumoAsses[3].sumoDudeNumber == 1) {
					sumoAsses[3].sumoDudeNumber = 2;
					startingSecondaryID2 = sumoAsses[3].playerID;
				} else {
					sumoAsses[3].sumoDudeNumber = 1;
					otherSecondaryID1 = sumoAsses[3].playerID;
				}
			}
			sumoTwoInputs.Insert(0, leftSide2);
		}

		sumos[0].SetupSecondaryDisplay(startingSecondaryID1, otherSecondaryID1);
		sumos[1].SetupSecondaryDisplay(startingSecondaryID2, otherSecondaryID2);
		if(isGameStarted) {
			sumos[0].ReceiveInputs(sumoOneInputs);
			sumos[1].ReceiveInputs(sumoTwoInputs);
		}

	}

	private int GetStartingSecondaryIDOne() {
		if(sumoAsses[0].role == SumoRole.SECONDARY && sumoAsses[0].sumoDudeNumber == 1) {
			return sumoAsses[0].playerID;
		} else if(sumoAsses[1].role == SumoRole.SECONDARY && sumoAsses[1].sumoDudeNumber == 1) {
			return sumoAsses[1].playerID;
		} else {
			return 0;
		}
	}

	private int GetStartingSecondaryIDTwo() {
		if(sumoAsses[2].role == SumoRole.SECONDARY && sumoAsses[2].sumoDudeNumber == 2) {
			return sumoAsses[2].playerID;
		} else if(sumoAsses[3].role == SumoRole.SECONDARY && sumoAsses[3].sumoDudeNumber == 2) {
			return sumoAsses[3].playerID;
		} else {
			return 0;
		}
	}

	public int GetOtherSecondaryIDOne() {
		if(sumoAsses[2].role == SumoRole.SECONDARY && sumoAsses[2].sumoDudeNumber == 1) {
			return sumoAsses[2].playerID;
		} else if(sumoAsses[3].role == SumoRole.SECONDARY && sumoAsses[3].sumoDudeNumber == 1) {
			return sumoAsses[3].playerID;
		} else {
			return 0;
		}
	}

	public int GetOtherSecondaryIDTwo() {
		if(sumoAsses[0].role == SumoRole.SECONDARY && sumoAsses[0].sumoDudeNumber == 2) {
			return sumoAsses[0].playerID;
		} else if(sumoAsses[1].role == SumoRole.SECONDARY && sumoAsses[1].sumoDudeNumber == 2) {
			return sumoAsses[1].playerID;
		} else {
			return 0;
		}
	}

	public void GameDone() {
		//determine winner or draw
		int playerID = 0;
		int highestScore = 0;
		for(int i = 0; i < sumoAsses.Length; i++) {
			if(i == 1) {
				highestScore = poins[i].GetPoinCount();
				playerID = 1;
			} else {
				if(highestScore == poins[i].GetPoinCount()) {
					playerID = 0;
				} else if(highestScore < poins[i].GetPoinCount()) {
					playerID = sumoAsses[i].playerID;
					highestScore = poins[i].GetPoinCount();
				}
			}
		}
		sumos[0].Respawn();
		sumos[1].Respawn();
		sumos[0].gameObject.SetActive(false);
		sumos[1].gameObject.SetActive(false);
		isGameStarted = false;
		EventManager.DisplayScore(playerID);
	}

	public void PlayAgain(){

		EventManager.StartGame();
	}
}
