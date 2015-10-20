using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	GameObject game;

	// Use this for initialization
	void Start () {
		game = new GameObject ();
		game.name = "Game controller";
		game.AddComponent<Controller> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)){
			game.GetComponent<Controller>().Clear();
			Destroy (game);
			game = new GameObject();
			game.AddComponent<Controller>();
		}
	}
}
