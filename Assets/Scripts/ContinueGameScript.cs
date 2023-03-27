using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ContinueGameScript : MonoBehaviour {

	public void YesClick(){
		SceneManager.LoadScene ("CenaJogo");
	}

	public void NoClick(){
		Application.Quit ();
	}
}
