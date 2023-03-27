using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {

	GameController controller;

	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		Destroy (collider.gameObject);
	}
}
	