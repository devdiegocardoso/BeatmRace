using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)  || Input.GetMouseButtonDown(0)){
			SceneManager.LoadScene("CenaTutorial");
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}	
	}
}
