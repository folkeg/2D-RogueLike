using UnityEngine;
using System.Collections;

public class MyWall : MonoBehaviour {

	public AudioClip playerChop1;
	public AudioClip playerChop2;
	public Sprite dmgSprite;
	public int hp = 4;

	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	
	}

	public void DamageWall(int loss){
		MySoundManager.instance.RandomizeSfx (playerChop1, playerChop2);
		spriteRenderer.sprite = dmgSprite;
		hp -= loss;
		if (hp <= 0) {
			gameObject.SetActive (false);
		}
	}
	

}
