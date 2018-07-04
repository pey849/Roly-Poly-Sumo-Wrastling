using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour {
	
	//set these up
	public Sprite[] imageSprites;

	//the score text
	public Image onesDigit;
	public Image tensDigit;
	public Image hundredsDigit;

	void Start() {
		
	}

	void OnEnable() {
		EventManager.updateTimerEvent += UpdateTime;
	}

	void OnDisable() {
		EventManager.updateTimerEvent -= UpdateTime;
	}

	public void UpdateTime(float time) {
		int hundreds = ((int)time) / 100;
		int tens = ((int)time) % 100 / 10;
		int ones = ((int)time) % 10;

		//do hundreds
		if (hundredsDigit != null) {
			hundredsDigit.sprite = this.imageSprites[hundreds];
		}
		//do tens
		if (tensDigit != null) {
			tensDigit.sprite = this.imageSprites[tens];
		}
		//do ones
		if (onesDigit != null) {
			onesDigit.sprite = this.imageSprites[ones];
		}
	}

}
