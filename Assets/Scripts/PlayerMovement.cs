using UnityEngine;
using System.Collections;

enum States { choosingDestination = 0, goingToDestination };

public class PlayerMovement : MonoBehaviour {
	
	States aiState = States.choosingDestination;
	//PlayerFlag
	public bool isMain = false;
	//This is for changing position
	float shift;
	//This is for changing direction (just a small cosmetic feature)
	float borderCheck;
	//Destination for AI status
	Vector3 destination = new Vector3();
	//Delat of choosing destination
	float thinkDelayMax = 5.0f;
	float stepDelay = 0.5f;
	float checkTime;
	float checkDelay;
	

	void Start () {
		//Initializing update variables
		shift = Camera.main.orthographicSize / 10;
		borderCheck = Camera.main.orthographicSize - (shift / 2);
		destination = transform.position;

		//Initializing spaceship
		int start = (int)Random.Range (0.0f, 400.0f);
		float startX = (float)((int)(start/GameInfo.mapWidth) * GameInfo.cellWidth) + GameInfo.cellCenter - Camera.main.orthographicSize;
		float startY = (float)((int)(start%GameInfo.mapHeight) * GameInfo.cellWidth) + GameInfo.cellCenter - Camera.main.orthographicSize;
		transform.position = new Vector3 (startX, startY, 0.0f);

		gameObject.AddComponent<SpriteRenderer> ();
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("pitrizzo-SpaceShip-gpl3-opengameart-96x96");
		GetComponent<SpriteRenderer> ().color = new Color(0.2f, 0.7f, 0.7f);
		transform.localScale = new Vector3 (GameInfo.cellWidth, GameInfo.cellWidth, 1.0f);
		//Debug.Log ("Started!");
	}
	
	void ReadMovement(){
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		float z = transform.rotation.eulerAngles.z;
		//Getting input values and preparing an update variables
		if (Input.GetKeyDown (KeyCode.W) && pos.y < borderCheck) {
			pos.y += shift;
			z = 0.0f;
		} else if (Input.GetKeyDown (KeyCode.S) && pos.y > -borderCheck) {
			pos.y -= shift;
			z = 180.0f;
		}
		
		if (Input.GetKeyDown (KeyCode.D) && pos.x < borderCheck) {
			pos.x += shift;
			z = -90.0f;
		}
		else if (Input.GetKeyDown (KeyCode.A) && pos.x > -borderCheck) {
			pos.x -= shift;
			z = 90.0f;
		}
		rot = Quaternion.Euler (0, 0, z);
		//Updating position and angle
		transform.position = pos;
		transform.rotation = rot;
	}
	
	void HoldOnBehavior(){
		//Checking for delay
		if (checkDelay != 0 && (Time.time - checkTime < checkDelay))
			return;
		checkDelay = 0.0f;
		
		switch (aiState) {
		case States.choosingDestination :
			//Generating new destination spot
			int start = (int)Random.Range (0.0f, 400.0f);
			float startX = (float)((int)(start/GameInfo.mapWidth) * GameInfo.cellWidth) + GameInfo.cellCenter - Camera.main.orthographicSize;
			float startY = (float)((int)(start%GameInfo.mapHeight) * GameInfo.cellWidth) + GameInfo.cellCenter - Camera.main.orthographicSize;
			destination = new Vector3(startX, startY, 0.0f);
			
			//Counting thinking delay
			checkTime = Time.time;
			checkDelay = Random.Range(0.0f, thinkDelayMax);
			
			//Obviously, new state
			aiState = States.goingToDestination;
			break;
		case States.goingToDestination:
			Vector3 pos = transform.position;
			//Condition for changing state
			if (pos == destination){
				aiState = States.choosingDestination;
				break;
			}
			
			//Adding step delay
			checkTime = Time.time;
			checkDelay = stepDelay;
			
			//Counting next step
			Quaternion rot = transform.rotation;
			float z = transform.rotation.eulerAngles.z;
			
			if (destination.y > pos.y){
				pos.y += shift;
				z = 0.0f;
			}
			else if (destination.y < pos.y){
				pos.y -= shift;
				z = 180.0f;
			}
			else if (destination.x > pos.x){
				pos.x += shift;
				z = -90.0f;
			}
			else if (destination.x < pos.x){
				pos.x -= shift;
				z = 90.0f;
			}
			
			//Updating position and angle
			rot = Quaternion.Euler (0, 0, z);
			transform.position = pos;
			transform.rotation = rot;
			break;
		default:
			Debug.Log("Unexpected AI state");
			break;
		}
	}
	
	//Update is called once per frame
	void Update () {
		if (isMain)
			ReadMovement ();
		else
			HoldOnBehavior ();
	}
}
