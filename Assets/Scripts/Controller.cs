using UnityEngine;
using System.Collections;

public struct GameInfo {
	static public int playerNum;
	static public int mapHeight;
	static public int mapWidth;
	static public int mapSize;
	static public float viewRadius;
	static public float cellWidth;
	static public float cellCenter;
}

public class Controller : MonoBehaviour {
	
	//main variables
	GameObject playersContainer;
	GameObject player;
	GameObject[] players;

	GameObject cellsContainer;
	GameObject[] cells;
	//Different sprites for cell
	Sprite cellHidden = new Sprite();
	//This one will stay empty
	Sprite cellEmpty = new Sprite();
	Sprite cellMeteor = new Sprite();
	Sprite cellPlanet = new Sprite();
	Sprite cellStar = new Sprite();
	
	//Seed for pseudo random number generator
	int seed;
	//Salt
	float goldeRatio = Mathf.Pow (2.0f, 32.0f);
	
	//Generates some nubmer depending on seed
	int PseudoRandom(int seed) {
		float seed_f = (float)seed;
		//Debug.Log ("Seed: " + seed + " Mult: " + seed * Mathf.Pow (2.0f, 32.0f));
		return (int)(seed_f * goldeRatio * Mathf.Sin(seed_f) % 1000);
	}

	//Destroying game objects and clearing memory before restarting game
	public void Clear() {
		for (int i = 0; i < players.Length; i++) {
			Destroy(players[i]);
		}

		for (int i = 0; i < cells.Length; i++) {
			Destroy(cells[i]);
		}

		Destroy (player);
		Destroy (cellsContainer);
		Destroy (playersContainer);
	}

	// Use this for initialization
	void Start () {
		GameInfo.playerNum = 10;
		GameInfo.mapHeight = 20;
		GameInfo.mapWidth = 20;
		GameInfo.viewRadius = 9.0f;
		GameInfo.cellWidth = Camera.main.orthographicSize / 10;
		GameInfo.cellCenter = Camera.main.orthographicSize / 20;
		GameInfo.mapSize = GameInfo.mapWidth * GameInfo.mapHeight;
		
		seed = (int)(Random.Range (0.0f, 100.0f) * 1000);
		Debug.Log ("Seed: " + seed);
		
		//Initializing all players
		playersContainer = new GameObject ();
		playersContainer.name = "Players container";
		players = new GameObject[GameInfo.playerNum];
		for (int i = 0; i < players.Length; i++) {
			players[i] = new GameObject();
			players[i].name = "player" + i;
			players[i].AddComponent<PlayerMovement> ();
			int start = (int)Random.Range (0.0f, 400.0f);
			float startX = (float)((int)(start/GameInfo.mapWidth) * GameInfo.cellWidth) + GameInfo.cellCenter - Camera.main.orthographicSize;
			float startY = (float)((int)(start%GameInfo.mapHeight) * GameInfo.cellWidth) + GameInfo.cellCenter - Camera.main.orthographicSize;
			players[i].transform.position = new Vector3 (startX, startY, 0.0f);
			players[i].AddComponent<SpriteRenderer> ();
			players[i].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("pitrizzo-SpaceShip-gpl3-opengameart-96x96");
			players[i].GetComponent<SpriteRenderer> ().color = new Color(0.2f, 0.7f, 0.7f);
			players[i].transform.localScale = new Vector3 (GameInfo.cellWidth, GameInfo.cellWidth, 1.0f);
			players[i].transform.parent = playersContainer.transform;
		}
		
		//Initializing main player
		player = players [0];
		player.transform.position = new Vector3 (0.0f + GameInfo.cellCenter, 0.0f + GameInfo.cellCenter, 0.0f);
		player.GetComponent<PlayerMovement> ().isMain = true;
		player.GetComponent<SpriteRenderer> ().color = new Color(1.0f, 1.0f, 1.0f);
		
		//Loading sprites from Assets/Resources
		cellHidden = Resources.Load<Sprite>("unseen3");
		cellMeteor = Resources.Load<Sprite>("Asteroid");
		cellPlanet = Resources.Load<Sprite>("pl2");
		cellStar = Resources.Load<Sprite>("star 1");
		
		//Initializing cells
		cellsContainer = new GameObject ();
		cellsContainer.name = "Cells container";
		cells = new GameObject[GameInfo.mapSize];
		for (int i =0; i < cells.Length; i++) {
			cells[i] = new GameObject();
			float startX = GameInfo.cellWidth * (i/GameInfo.mapWidth) - Camera.main.orthographicSize + GameInfo.cellCenter;
			float startY = GameInfo.cellWidth * (i%GameInfo.mapHeight) - Camera.main.orthographicSize + GameInfo.cellCenter;
			cells[i].transform.position = new Vector3(startX, startY, 0.0f);
			cells[i].name = ("cell_" + (i/GameInfo.mapWidth) + "_" + (i%GameInfo.mapHeight));
			cells[i].AddComponent<SpriteRenderer> ();
			cells[i].transform.localScale = new Vector3 (GameInfo.cellWidth, GameInfo.cellWidth, 1.0f);
			cells[i].transform.parent = cellsContainer.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Checking if player is need to be changed;
		int playerIndex = -1;
		string inputValues = Input.inputString;
		if (inputValues.Length > 0 && inputValues [0] >= '0' && inputValues [0] <= '9') {
			playerIndex = (int)char.GetNumericValue(inputValues[0]);
		}

		//Changing main player
		if (playerIndex > -1 && !players [playerIndex].GetComponent<PlayerMovement> ().isMain) {
			player.GetComponent<SpriteRenderer> ().color = new Color(0.2f, 0.7f, 0.7f);
			player.GetComponent<PlayerMovement> ().isMain = false;
			player = players[playerIndex];
			player.GetComponent<SpriteRenderer> ().color = new Color(1.0f, 1.0f, 1.0f);
			player.GetComponent<PlayerMovement> ().isMain = true;
		}

		Vector3 playerPos = player.transform.position;
		//Checking if cell is visible depending on view radius
		for (int i =0; i < cells.Length; i++) {
			Vector3 cellPos = cells[i].transform.position;
			float distance = Mathf.Sqrt(Mathf.Pow(cellPos.x - playerPos.x, 2.0f) + Mathf.Pow(cellPos.y - playerPos.y, 2.0f));
			
			//If it is not visible, setting sprite of invisible cell
			if (distance > (GameInfo.viewRadius * (GameInfo.cellWidth))) {
				cells[i].GetComponent<SpriteRenderer> ().sprite = cellHidden;
			}
			//Elseway setting sprite depending on it's random value
			//which will be the same for specified cell during one game match
			else {
				int contentCode = PseudoRandom(seed + i);
				if (contentCode >= 950)
					cells[i].GetComponent<SpriteRenderer> ().sprite = cellStar;
				else if (contentCode >= 900)
					cells[i].GetComponent<SpriteRenderer> ().sprite = cellPlanet;
				else if (contentCode >= 750)
					cells[i].GetComponent<SpriteRenderer> ().sprite = cellMeteor;
				else
					cells[i].GetComponent<SpriteRenderer> ().sprite = cellEmpty;
			}
		}
		
		//Checking if other players are visible
		for (int i = 0; i < players.Length; i++) {
			//Skipping main player
			if (players[i].GetComponent<PlayerMovement>().isMain)
				continue;
			Vector3 overPlPos = players[i].transform.position;
			float distance = Mathf.Sqrt(Mathf.Pow(overPlPos.x - playerPos.x, 2.0f) + Mathf.Pow(overPlPos.y - playerPos.y, 2.0f));
			if (distance > (GameInfo.viewRadius * (GameInfo.cellWidth))) {
				players[i].GetComponent<SpriteRenderer> ().color = new Color();
			}
			else {
				players[i].GetComponent<SpriteRenderer> ().color = new Color(0.2f, 0.7f, 0.7f);
			}
		}
	}
}
