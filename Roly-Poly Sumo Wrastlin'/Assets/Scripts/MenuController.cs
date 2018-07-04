using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	//the in game ui
	public GameObject inGameCanvas;

	//the menus
	public GameObject menusCanvas;
	public GameObject tutorialCanvas;
	public GameObject titleScreenExitCanvas;

	public GameObject playButton;
	public GameObject playAgainButton;
	public GameObject backButtonTutorial;
	public GameObject exitButton;
	public GameObject titleScreenExitButton;

	public Text playButton_Text;
	public Text playAgainButton_Text;
	public Text backButton_Text;
	public Text exitButton_Text;


	//Start Button
	public GameObject startButton;

	//Keep track of the menu we are on
	protected GameObject curMenu;
	protected GameObject storeSelectedButton;

	private Color selectedColor = new Color(124,252,0);
	private Color unSelectedColor = new Color(255,255,255);

	// Use this for initialization
	void Start () {
		curMenu = menusCanvas;
		switchCurrentButton (startButton);

		storeSelectedButton.GetComponent<Button> ().image.color = selectedColor;

	}
	
	// Update is called once per frame
	void Update () {
		menuSelection ();
	}

	void OnEnable() {
		EventManager.gameDoneEvent += GameDone;
	}

	void OnDisable() {
		EventManager.gameDoneEvent -= GameDone;
	}

	void GameDone() {
		switchCurrentMenu(titleScreenExitCanvas);
	}

	//click to play the game
	public void OnClickPlay() {
		switchCurrentMenu (inGameCanvas);
		EventManager.StartGame();
	}

	//click to go to next menu
	public void onClick_Next(){
		if (curMenu == menusCanvas) {
			switchCurrentMenu (tutorialCanvas);
			Debug.Log ("Changing to tutorial");
		}
	
	}

	//click to go to next menu
	public void onClick_Exit(){

		//UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();

	}

	//Make old menu inactive and the new one active
	public void switchCurrentMenu(GameObject menu){
		curMenu.SetActive (false);
		curMenu = menu;
		curMenu.SetActive (true);
	}

	public void menuSelection(){

		float analog = Input.GetAxis ("Horizontal");

		if (curMenu == menusCanvas) {
			switchAnalogButton (analog, startButton, exitButton);
		}
		else if(curMenu == tutorialCanvas){
			switchAnalogButton (analog, backButtonTutorial, playButton);
		}
		else if (curMenu == titleScreenExitCanvas){
			switchAnalogButton (analog, playAgainButton, titleScreenExitButton);
		}


		if (Input.GetButtonDown ("Submit")) {

			if (curMenu == inGameCanvas) {
				//do not allow presses
			}
			else {
				if (storeSelectedButton == startButton) {
					Debug.Log ("Start button Was Pressed");
					switchCurrentButton (playButton);
					onClick_Next ();
				} 
				else if (storeSelectedButton == playButton) {
					Debug.Log ("Play Button was Pressed");
					switchCurrentButton (playAgainButton);
					OnClickPlay ();
				} 
				else if (storeSelectedButton == playAgainButton) {
					Debug.Log ("Play again was pressed");
					//event to play again
					EventManager.PlayAgain();
					switchCurrentMenu(inGameCanvas);
				} 
				else if (storeSelectedButton == backButtonTutorial || storeSelectedButton == titleScreenExitButton) {
					Debug.Log ("back button tutorial or main menu was pressed");
					switchCurrentButton (startButton);
					switchCurrentMenu (menusCanvas);
				} 
				else if (storeSelectedButton == exitButton) {
					onClick_Exit ();
					Debug.Log ("Exit was pressed");
				}
			}
		}
	}

	//Make old menu inactive and the new one active
	public void switchCurrentButton(GameObject button){
		this.storeSelectedButton = button;
	}

	public void switchAnalogButton(float leftOrRightValue, GameObject leftButton, GameObject rightButton){

		if (leftOrRightValue == -1) {
			if (storeSelectedButton == rightButton) {
				Debug.Log ("Went Left");
				switchCurrentButton (leftButton);
				rightButton.GetComponent<Button> ().image.color = unSelectedColor;
			}
		} else if (leftOrRightValue == 1) {
			if (storeSelectedButton == leftButton) {
				Debug.Log ("Went Right");
				switchCurrentButton (rightButton);
				leftButton.GetComponent<Button> ().image.color = unSelectedColor;
			}
		}

		this.storeSelectedButton.GetComponent<Button> ().image.color = selectedColor;
	}
		
}
