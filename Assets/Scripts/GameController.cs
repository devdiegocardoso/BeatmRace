using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	private float StartTime = 3.0f;
	private bool canStart = false;
	private bool endGame = false;

	public Image semaphore;
	public Image winnerImg;
	private Sprite winner;
	public Text winnerText;
	public GameObject playAgain;

	private float elapsedTime = 0;

	public Scrollbar p1Scroll;
	public Scrollbar p2Scroll;

	float gameTime = 60f;
	int count = 0;
	private int MAX_STEPS = 50;

	float player1Time;
	float player2Time;

	public GameObject P1Button;
	public GameObject P2Button;

	private float p1NextSpawn;
	private float p2NextSpawn;

	public GameObject P1Spawn;
	public GameObject P2Spawn;

	public GameObject CheckPointP1;
	public GameObject CheckPointP2;

	private Queue<GameObject> p1queue;
	private Queue<GameObject> p2queue;

	private Queue<float> p1timer;
	private Queue<float> p2timer;

	public Animator animatorP1;
	public Animator animatorP2;

	public Animator carroVermelho;
	public Animator carroVerde;

	public AudioSource gameSound;
	public AudioSource gogogo;

	public Queue<GameObject> getQueue(){
		return p1queue;
	}

	// Use this for initialization
	void Start () {
		winnerImg.gameObject.SetActive (false);
		winnerText.gameObject.SetActive (false);
		playAgain.gameObject.SetActive (false);
		p1queue = new Queue<GameObject>();
		p2queue = new Queue<GameObject>();

		p1timer = new Queue<float> ();
		p2timer = new Queue<float> ();

		p1NextSpawn = 2f;
		p2NextSpawn = 2f;

		player1Time = 0;
		player2Time = 0;

		P1Spawn = GameObject.FindGameObjectWithTag ("P1Spawn");
		P2Spawn = GameObject.FindGameObjectWithTag ("P2Spawn");

		CheckPointP1 = GameObject.FindGameObjectWithTag ("P1Check");
		CheckPointP2 = GameObject.FindGameObjectWithTag ("P2Check");

		P1Button = Resources.Load ("Sprites/TriangleButton") as GameObject;
		P2Button = Resources.Load ("Sprites/TriangleButton2") as GameObject;

		for (int i = 0; i < MAX_STEPS; i++) {
			float timer = (Random.value * 2.5f) + 0.5f;
			p1timer.Enqueue (timer);
			p2timer.Enqueue (timer);
		}
			
	}

	void NewP1Command()
	{
		var newButtonP1 = GameObject.Instantiate(P1Button,P1Spawn.gameObject.transform);
		p1queue.Enqueue (newButtonP1);
	}

	void NewP2Command()
	{
		var newButtonP2 = GameObject.Instantiate(P2Button,P2Spawn.gameObject.transform);
		p2queue.Enqueue (newButtonP2);
	}

	private void p1Commands(){
		if (p1queue.Count > 0) {
			GameObject p1CurrentItem = p1queue.Peek ();
			if (p1CurrentItem == null) {
				p1queue.Dequeue ();
				if (animatorP1.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
					animatorP1.SetTrigger ("ActiveBadAnimation");
			} else if (Input.GetKeyDown (KeyCode.A)) {
				p1CurrentItem = p1queue.Peek ();
				Vector2 v1, v2;
				v1 = new Vector2 (p1CurrentItem.transform.position.x, p1CurrentItem.transform.position.y);
				v2 = new Vector2 (CheckPointP1.transform.position.x, CheckPointP1.transform.position.y);
				float distance = Vector2.Distance (v1,v2);
				if (distance < 2f) {
					Destroy (p1queue.Dequeue ());
					count++;
					Debug.Log (count);
					if (animatorP1.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
						animatorP1.SetTrigger ("ActiveGoodAnimation");
					if (p1timer.Count > 0) {
						p1NextSpawn = p1timer.Dequeue ();
						player1Time += p1NextSpawn;
						p1Scroll.value = player1Time / gameTime;
						Debug.Log (p1NextSpawn);
						CancelInvoke ("NewP1Command");
						InvokeRepeating ("NewP1Command", 0, p1NextSpawn);
					}
				} else {
					Destroy (p1queue.Dequeue ());
					if (animatorP1.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
						animatorP1.SetTrigger ("ActiveBadAnimation");
					Debug.Log (count);
				}
			}
		}
	}

	private void p2Commands(){
		if (p2queue.Count > 0) {
			GameObject p2CurrentItem = p2queue.Peek ();
			if (p2CurrentItem == null) {
				p2queue.Dequeue ();
				if (animatorP2.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
					animatorP2.SetTrigger ("ActiveBadAnimation");
			} else if (Input.GetKeyDown (KeyCode.L)) {
				p2CurrentItem = p2queue.Peek ();
				Vector2 v1, v2;
				v1 = new Vector2 (p2CurrentItem.transform.position.x, p2CurrentItem.transform.position.y);
				v2 = new Vector2 (CheckPointP2.transform.position.x, CheckPointP2.transform.position.y);
				float distance = Vector2.Distance (v1, v2);
				if (distance < 2f) {
					Destroy (p2queue.Dequeue ());
					count++;
					Debug.Log (count);
					if (animatorP2.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
						animatorP2.SetTrigger ("ActiveGoodAnimation");
					if (p2timer.Count > 0) {
						p2NextSpawn = p2timer.Dequeue ();
						player2Time += p2NextSpawn;
						p2Scroll.value = player2Time / gameTime;
						Debug.Log (p2NextSpawn);
						CancelInvoke ("NewP2Command");
						InvokeRepeating ("NewP2Command", 0, p2NextSpawn);
					}
				} else {
					Destroy (p2queue.Dequeue ());
					if (animatorP2.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
						animatorP2.SetTrigger ("ActiveBadAnimation");
					Debug.Log (count);
				}
			}
		}
	}

	IEnumerator deactiveSemaphore(){
		gogogo.Play ();
		yield return new WaitForSeconds (0.5f);
		semaphore.gameObject.SetActive (false);
		gameSound.Play ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (!canStart) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > StartTime) {
				canStart = true;
				InvokeRepeating ("NewP1Command", 0, p1NextSpawn);
				InvokeRepeating ("NewP2Command", 0, p2NextSpawn);
				StartCoroutine(deactiveSemaphore());
			}
		}

		if (canStart && !endGame) {
			if (p1Scroll.value != p2Scroll.value) {
				if (p1Scroll.value > p2Scroll.value) {
					carroVermelho.SetBool ("Vencendo", true);	
					carroVerde.SetBool ("Vencendo", false);
				} else {
					carroVermelho.SetBool ("Vencendo", false);	
					carroVerde.SetBool ("Vencendo", true);
				}
			} else {
				carroVermelho.SetBool ("Vencendo", false);	
				carroVerde.SetBool ("Vencendo", false);
			}
			if (p1Scroll.value >= 1.0f || p2Scroll.value >= 1.0f) {
				endGame = true;
				CancelInvoke ();
				winnerImg.gameObject.SetActive (true);
				winnerText.gameObject.SetActive (true);
				playAgain.gameObject.SetActive (true);
				if (p1Scroll.value > p2Scroll.value)
					winnerText.text = "Player 1";
				else
					winnerText.text = "Player 2";
			} else {
				p1Commands ();
				p2Commands ();
			}
		}
	}
}
