using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	//the sprite mask for the platform
	private SpriteMask sprMask;

	//the sprite renderer for the platform
	private SpriteRenderer sprRenderer;

	//the polygon collider2D for the platform
	private PolygonCollider2D polyCollider;

	// Use this for initialization
	void Start () {
		this.sprRenderer = GetComponent<SpriteRenderer>();
		this.polyCollider = GetComponent<PolygonCollider2D>();
		this.sprMask = GetComponent<SpriteMask>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		EventManager.startGameEvent += StartGame;
	}

	void OnDisable() {
		EventManager.startGameEvent -= StartGame;
	}

	void OnTriggerExit2D(Collider2D collider) {
		//send message to player that he is off the platform
		if(collider.CompareTag("Player")) {
			collider.GetComponent<SumoDudes>().PlayerOffPlatform();
		}
	}

	public void StartGame() {
		this.sprRenderer.enabled = true;
		this.polyCollider.enabled = true;
		this.sprMask.enabled = true;
	}
}
