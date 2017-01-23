using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public float turnDelay = .1f;
	public static GameManager manager = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playerTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private int level = 1;
	private List<MyEnemy> enemies;
	private bool enemyMoving;
	private bool doingSetup = true;

	void Awake(){
		if (manager == null) {
			manager = this;
		} else if (manager != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
		enemies = new List<MyEnemy> ();
		boardScript = GetComponent<BoardManager> ();
		InitialGame ();
	}

	void OnLevelWasLoaded(int index){
		level++;
		InitialGame ();
	}

	void InitialGame(){
		doingSetup = true;
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideImage", levelStartDelay);
		enemies.Clear ();
		boardScript.setupScene (level);
	}

	void HideImage(){
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver(){
		levelText.text = "After " + level + " days, you starved.";
		levelImage.SetActive (true);
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerTurn || enemyMoving || doingSetup) {
			return;
		}
		StartCoroutine (MovingEnemies ());
	    
	}

	public void AddEnemy(MyEnemy script){
		enemies.Add (script);
	}

	IEnumerator MovingEnemies(){
		enemyMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies[i].movingSpeed);
		}
		playerTurn = true;
		enemyMoving = false;
	}
}
