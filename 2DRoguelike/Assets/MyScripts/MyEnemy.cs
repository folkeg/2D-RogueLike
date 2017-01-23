using UnityEngine;
using System.Collections;

public class MyEnemy : MovingObject {

	public int playerDanamge;
	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;

	private Transform target;
	private bool skipMove;
	private Animator animator;

	// Use this for initialization
	protected override void Start () {
		GameManager.manager.AddEnemy (this);
		animator = GetComponent<Animator> ();
		target = GameObject.FindWithTag ("Player").transform;
		base.Start ();	
	}

	protected override void AttemptMove<T>(int xDir, int yDir){
		if (skipMove) {
			skipMove = false;
			return;
		}
		base.AttemptMove<T> (xDir, yDir);
		skipMove = true;
	}

	public void MoveEnemy(){
		int xDir = 0;
		int yDir = 0;
		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
			yDir = target.position.y > transform.position.y ? 1 : -1;
		} else {
			xDir = target.position.x > transform.position.x ? 1 : -1;
		}
		AttemptMove<MyPlayer> (xDir, yDir);
	}

	protected override void OnCantMove<T> (T component){
		MyPlayer player = component as MyPlayer;
		animator.SetTrigger ("EnemyAttack");
		MySoundManager.instance.RandomizeSfx (enemyAttack1, enemyAttack2);
		player.LoseFood (playerDanamge);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
