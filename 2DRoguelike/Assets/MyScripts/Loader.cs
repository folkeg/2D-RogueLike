using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameObject gameManager;

	void Awake(){
		if (GameManager.manager == null) {
			Instantiate (gameManager);
		}
	}
}
