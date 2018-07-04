using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoDudes : MonoBehaviour {

	//the player ID
	private int playerID;

	//the mid point to determine if we need to change sorting order
	public Transform platformMidpoint;

	//the sumo dudes transform
	private Transform tform;

	//the sumo dudes' rigidbody2D
	private Rigidbody2D rBody;

	//the sumo dudes' animator
	private Animator anim;

	//the sumo is dead?
	private bool isSumoDead;

	//array of player colors
	public Color[] playerColors = {};

	//the spawnpoint for this sumo
	public Transform spawnpoint;

	//the spriterenderers for the sumodudes
	public SpriteRenderer head;
	public SpriteRenderer body;
	public SpriteRenderer legs;
	public SpriteRenderer flagA;
	public SpriteRenderer flagB;

	//the polygon collider
	private CircleCollider2D circleCollider;

	//props for movement & animation
	public bool primaryFacingLeft = false;
	public bool primaryIsStruggling = false;
	public float currentSpeed;

	private float lastPrimeMoveX = 0f;
	private Vector2 moveVector = new Vector2(0f,0f);

	public float speedMod = 1f;
	public float primaryMag = 1f;
	public float secondaryMag = 0.45f;

	// Use this for initialization
	void Start () {
		this.tform = GetComponent<Transform>();
		this.rBody = GetComponent<Rigidbody2D>();
		this.anim = GetComponent<Animator>();
		this.isSumoDead = false;
		this.circleCollider = GetComponent<CircleCollider2D>();
	}

	//arraylist o' SumoInputs
	public void ReceiveInputs(ArrayList sumoInputs) {		
		float primeMagnitude = 0;
		for (int i = 0; i < sumoInputs.Count; i++) {
			SumoInputs iSumoInputs = sumoInputs[i] as SumoInputs;
			if (i == 0) {
				//primary
				moveVector = new Vector2(iSumoInputs.getHorizontalMovemt(), iSumoInputs.getVerticalMovemt()) * this.primaryMag * speedMod;
				primeMagnitude = moveVector.magnitude;
				lastPrimeMoveX = iSumoInputs.getHorizontalMovemt();
			} else {
				//secondary
				moveVector += new Vector2(iSumoInputs.getHorizontalMovemt(), iSumoInputs.getVerticalMovemt()) * this.secondaryMag * speedMod;
			}
		}
		//update properties
		currentSpeed = moveVector.magnitude;
		primaryIsStruggling = currentSpeed < (primeMagnitude * 0.3);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//move player with combined vectors
		if(!isSumoDead) {
			//rBody.velocity = moveVector;
			rBody.AddForce(moveVector * 8f);
		}

		//update animation stuff
		anim.SetFloat("speed", currentSpeed * 0.4f);
		anim.SetBool ("struggling", primaryIsStruggling);

		//check desired direction
		if (primaryFacingLeft != lastPrimeMoveX < 0 && lastPrimeMoveX != 0) {
			//direction change. udpate tracking bool and update x scale (flip)
			primaryFacingLeft = lastPrimeMoveX < 0;
			tform.localScale = new Vector3 (tform.localScale.x*-1.0f, tform.localScale.y, tform.localScale.z);
		}
	}

	public void PlayerOffPlatform() {
		if(!this.isSumoDead) {
			if(IsAbovePlatformMidpoint()) {
				TurnOnMaskInteractions();
			}
			//make him fall
			this.rBody.gravityScale = 3.0f;
			this.rBody.drag = 0;
			this.circleCollider.enabled = false;
			this.isSumoDead = true;
			StartCoroutine("WaitForRespawn");
			EventManager.PlayerOffPlatform(playerID);
		}
	}

	public void SetPlayerID(int playerID) {
		this.playerID = playerID;
		//set color for sumo sprites
		head.color = playerColors[playerID-1];
		body.color = playerColors[playerID-1];
		legs.color = playerColors[playerID-1];
	}

	public void SetupSecondaryDisplay (int startingSecondaryID, int otherSecondaryID) {
		//flag A
		if (startingSecondaryID == 0) {
			//hide flag
			flagA.enabled = false;
		} else {
			//show and colour flag
			flagA.enabled = true;
			flagA.color = playerColors[startingSecondaryID-1];
		}
		//flag B
		if (otherSecondaryID == 0) {
			//hide flag
			flagB.enabled = false;
		} else {
			//show and colour flag
			flagB.enabled = true;
			flagB.color = playerColors[otherSecondaryID-1];
		}
	}

	private bool IsAbovePlatformMidpoint () {
		if(this.tform.position.y > this.platformMidpoint.position.y) {
			return true;
		} else {
			return false;
		}
	}

	private void TurnOnMaskInteractions() {
		head.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		body.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		legs.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		flagA.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		flagB.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
	}

	private void TurnOffMaskInteractions() {
		head.maskInteraction = SpriteMaskInteraction.None;
		body.maskInteraction = SpriteMaskInteraction.None;
		legs.maskInteraction = SpriteMaskInteraction.None;
		flagA.maskInteraction = SpriteMaskInteraction.None;
		flagB.maskInteraction = SpriteMaskInteraction.None;
	}

	public void Respawn() {
		this.rBody.gravityScale = 0;
		this.rBody.drag = 4;
		this.rBody.velocity = Vector2.zero;
		this.transform.position = Vector3.zero;
		this.isSumoDead = false;
		this.circleCollider.enabled = true;
		TurnOffMaskInteractions();
	}

	IEnumerator WaitForRespawn () {
		yield return new WaitForSeconds(1f);
		Respawn();
	}

	public void ResetPlayerPosition () {
		this.transform.position = spawnpoint.position;
	}

	public int GetPlayerID() {
		return this.playerID;
	}
}
