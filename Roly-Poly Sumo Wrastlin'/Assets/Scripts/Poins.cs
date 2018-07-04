using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poins {

	private int poinCount;

	public Poins() {
		this.poinCount = 0;
	}

	public void AddPoins(int poins) {
		this.poinCount += poins;
	}

	public void ResetPoins() {
		this.poinCount = 0;
	}

	public int GetPoinCount(){
		return this.poinCount;
	}
}
