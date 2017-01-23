using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyPlayer : MovingObject {

	public int wallDamange = 1;
	public int PointPerFood = 10;
	public int PointPerSoda = 20;
	public float restartLevelDelay = 1f;
	public AudioClip movingSound1;
	public AudioClip movingSound2;
	public AudioClip eatingSound1;
	public AudioClip eatingSound2;
	public AudioClip drinkingSound1;
	public AudioClip drinkingSound2;
	public AudioClip gameoverSound;
	public Text foodText;

	private Animator animator;
	private int food;
	private Vector2 touchOrigin = -Vector2.one;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator>();
		food = GameManager.manager.playerFoodPoints;
		foodText.text = "Food " + food;
		base.Start();
	
	}

	private void OnDisable(){
		GameManager.manager.playerFoodPoints = food;
	}

	// Update is called once per frame
	void Update () {
		if (!GameManager.manager.playerTurn) {
			return;
		}
		int horizontal = 0;
		int vertical = 0;
    #if UNITY_STANDALONE || UNITY_WEBPLAYER
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");
		if (horizontal != 0) {
			vertical = 0;
		}
	#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		if (Input.touchCount > 0){
		    Touch myTouch = Input.touches[0];
		    if (myTouch.phase == TouchPhase.Began){
		        touchOrigin = myTouch.position;
		    }
		    else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0){
		        Vector2 touchEnd = myTouch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin.x = -1;
				if (Mathf.Abs(x) > Mathf.Abs(y))
					horizontal = x > 0 ? 1 : -1;
				else
					vertical = y > 0 ? 1 : -1;
		    }
		}
	#endif
		if (horizontal != 0 || vertical != 0) {
			AttemptMove<MyWall> (horizontal, vertical);
		}				
	}

	protected override void AttemptMove<T>(int xDir, int yDir){
		food--;
		foodText.text = "Food: " + food;
		base.AttemptMove<T> (xDir, yDir);
		RaycastHit2D hit;
		if(Move(xDir, yDir, out hit)){
			MySoundManager.instance.RandomizeSfx (movingSound1, movingSound2);
		}
		CheckIfGameOver ();
		GameManager.manager.playerTurn = false;
	}

	protected override void OnCantMove<T>(T component){
		MyWall hitWall = component as MyWall;
		hitWall.DamageWall (wallDamange);
		animator.SetTrigger ("PlayerChop");
	}

	private void CheckIfGameOver(){
		if (food <= 0) {
			MySoundManager.instance.playSingle (gameoverSound);
			MySoundManager.instance.musicSource.Stop ();
			GameManager.manager.GameOver ();
		}
	}

	private void Restart(){
		Application.LoadLevel (Application.loadedLevel);
	}

	public void LoseFood(int loss){
		animator.SetTrigger ("PlayerHit");
		food -= loss;
		foodText.text = "-" + loss + " Food: " + food;
		CheckIfGameOver ();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += PointPerFood;
			foodText.text = "+" + PointPerFood + " Food: " + food;
			MySoundManager.instance.RandomizeSfx (eatingSound1, eatingSound2);
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			food += PointPerSoda;
			foodText.text = "+" + PointPerSoda + " Food: " + food;
			MySoundManager.instance.RandomizeSfx (drinkingSound1, drinkingSound2);
			other.gameObject.SetActive (false);
		}
	}

}

