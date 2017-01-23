﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count{
		public int min;
		public int max;

		public Count(int min, int max){
			this.min = min;
			this.max = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);
	public GameObject exit;
	public GameObject[] outerWallTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] foodTiles;
	public GameObject[] floorTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3> ();

	void Initialize(){
		gridPositions.Clear ();
		for (int i = 1; i < columns - 1; i++) {
			for (int j = 1; j < rows - 1; j++) {
				gridPositions.Add (new Vector3 (i, j, 0f));
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;
		for (int i = -1; i < columns + 1; i++) {
			for (int j = -1; j < rows + 1; j++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (i == -1 || j == -1 || i == columns || j == rows) {
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)]; 
				}
				GameObject instance = 
					Instantiate (toInstantiate, new Vector3 (i, j, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition(){
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPositions = gridPositions[randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPositions;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max){
		int objectCount = Random.Range (min, max + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void setupScene(int level){
		BoardSetup ();
		Initialize ();
		LayoutObjectAtRandom (wallTiles, wallCount.min, wallCount.max);
		LayoutObjectAtRandom (foodTiles, foodCount.min, foodCount.max);
		int enemyCount =(int) Mathf.Log (level, 2);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}

}
