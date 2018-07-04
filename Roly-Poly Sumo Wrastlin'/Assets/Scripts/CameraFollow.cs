using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	//the first target
	public Transform target1;

	//the second target
	public Transform target2;

	//the camera's transform
	private Transform tform;

	//the camera component
	private Camera cameraComponent;

	//is the game started
	private bool isGameStarted;

	private float lastDistance;

	//the music player
	private AudioSource musicPlayer;

	// Use this for initialization
	void Start () {
		this.tform = GetComponent<Transform>();
		this.cameraComponent = GetComponent<Camera>();
		this.isGameStarted = false;
		this.musicPlayer = GetComponent<AudioSource>();
		this.lastDistance = 8f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		EventManager.startGameEvent += StartGame;
		EventManager.gameDoneEvent += EndGame;
	}

	void OnDisable() {
		EventManager.startGameEvent -= StartGame;
		EventManager.gameDoneEvent -= EndGame;
	}

	void LateUpdate () {
		if(isGameStarted) {
			if(!musicPlayer.isPlaying) {
				musicPlayer.Play();
			}
			float avgX;
			float avgY;
			float distance;
			float step = 1f * Time.deltaTime;

			distance = Mathf.Lerp(this.lastDistance, Vector2.Distance(target1.position, target2.position), step);

			avgX = (target2.position.x + target1.position.x)/2;
			avgY = (target2.position.y + target1.position.y)/2;
			tform.position = new Vector3(avgX, avgY, -10f);

			if(distance <= 3f) {
				distance = 3f;
			} else if(distance >= 8f) {
				distance = 8f;
			}
			this.cameraComponent.orthographicSize = distance;
			this.lastDistance = distance;
		}
	}

	public void StartGame() {
		this.isGameStarted = true;
	}

	public void EndGame() {
		this.transform.position = new Vector3(0, 0, -10);
		musicPlayer.Stop();
		this.isGameStarted = false;
		this.cameraComponent.orthographicSize = 10;
	}
}
