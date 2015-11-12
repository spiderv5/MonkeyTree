using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

namespace Completed
	
{
	public class BoardManager : MonoBehaviour
	{
		[Serializable]
		public class Count
		{
			public int minimum;             //Minimum value for our Count class.
			public int maximum;             //Maximum value for our Count class.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
		
		public int columns = 9;
		public int rows = 97;
		public int gridLength = 3;
		
		public GameObject[] leftTreeTiles;
		public GameObject[] rightTreeTiles;
		public GameObject treeCrownTile;
		GameObject treeCrown;

		public GameObject[] enemyTiles;
		public GameObject[] fruitTiles;  
		public Count fruitCount = new Count (1190, 1200);   
		
		private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
		private Transform fruitsRoot;
		private Transform enemiesRoot;
		private List <Vector3> fruitPositions = new List <Vector3> ();   //Positions for fruits
		private Vector3 crownInitialPosition;
		
		private int curLevel = 1;
		
		private float[] TREE_POS_X = {-32,-11,11,32};
		private float[] ENEMY_POS_X = {-28,-15,-7,7,15,28};
		
		private float lastCameraPos_Y_staticEnemy = 0;
		private float lastCameraPos_Y_kinectEnemy = 10;
		private float lastCameraPos_Y_layEnemy = 20;
		void Awake()
		{
			boardHolder = new GameObject ("Board").transform;
			boardHolder.parent = transform;
			enemiesRoot = new GameObject ("Enemies").transform;
			enemiesRoot.parent = transform;
			fruitsRoot = new GameObject ("Fruits").transform;
			fruitsRoot.parent = transform;
		
			crownInitialPosition = new Vector3 (0, 18, 0);
			drawTree (420);
		}

		public void ClearScene()
		{
			ClearFruits ();
			ClearEnemies ();
		}
		public void SetupScene (int level)
		{
			curLevel = level;
			treeCrown.transform.position = crownInitialPosition;
			InitializeFruitPositions ();
			LayoutFruitAtRandom (fruitTiles, fruitCount.minimum, fruitCount.maximum, level);
			initEnemiesSetting ();
		}

		void initEnemiesSetting(){
			lastCameraPos_Y_staticEnemy = 0;
			lastCameraPos_Y_kinectEnemy = 0;
			lastCameraPos_Y_layEnemy = 0;
		}

		private float crownSpeed;
		void Update(){
			//move Corwn Up 
			if (CameraController.instance.enable) {
				crownSpeed = CameraController.instance.cameraSpeed;
				treeCrown.transform.position = Vector3.MoveTowards (treeCrown.transform.position, treeCrown.transform.position + Vector3.up, crownSpeed * Time.deltaTime);
			}
			// Generate Enemy
			Vector3 cameraPos = CameraController.instance.transform.position; 
			if (curLevel == 1) {
				//Level 1 : place static bird as enemy
				placeSeveralStaticEnemy (cameraPos, 20);
			} else if (curLevel == 2) {
				//Level 2 : + place kinectic bird as enemy
				placeSeveralStaticEnemy (cameraPos, 20);
				placeSeveralKinecticEnemy (cameraPos, 20, 35);
			} else if (curLevel == 3) {
				//Level 3: + birds laying egg as enemy
				placeSeveralStaticEnemy (cameraPos, 20);
				placeSeveralKinecticEnemy (cameraPos, 20, 25);
				placeSeveralLayingEnemy(cameraPos,35,36);
			}
		}

		private void placeSeveralStaticEnemy(Vector3 cameraPos,float interval){
//			print (cameraPos.y +"|"+ lastCameraPos_Y_staticEnemy + interval);
			if (cameraPos.y > lastCameraPos_Y_staticEnemy + interval) {
				placeSingleStaticBird (new Vector3 (ENEMY_POS_X [Random.Range (0,6)], cameraPos.y + interval, 0));
				placeSingleStaticBird (new Vector3 (ENEMY_POS_X [Random.Range (0,6)], cameraPos.y + interval, 0));
				//placeSingleStaticBird (new Vector3 (ENEMY_POS_X [Random.Range (0,6)], cameraPos.y + interval, 0));
				lastCameraPos_Y_staticEnemy = cameraPos.y;
			}
		}
		private void placeSeveralKinecticEnemy(Vector3 cameraPos,float interval,float destoryIntervalY){
//			print (cameraPos.y +"|"+ lastCameraPos_Y_kinectEnemy + interval);
			if (cameraPos.y > lastCameraPos_Y_kinectEnemy + interval) {
				int startPos = Random.Range(0,6);
				placeSingleKinecticBird(new Vector3 (ENEMY_POS_X [startPos], cameraPos.y + interval, 0),
				                  new Vector3 (ENEMY_POS_X [startPos], cameraPos.y - interval, 0),
				                  8,destoryIntervalY);
				startPos = Random.Range(0,6);
				placeSingleKinecticBird(new Vector3 (ENEMY_POS_X [startPos], cameraPos.y + interval, 0),
				                        new Vector3 (ENEMY_POS_X [startPos], cameraPos.y - interval, 0),
				                        8,destoryIntervalY);
				lastCameraPos_Y_kinectEnemy = cameraPos.y;
			}
		}
		private void placeSeveralLayingEnemy(Vector3 cameraPos,float interval,float destoryIntervalY){
			if (cameraPos.y > lastCameraPos_Y_layEnemy + interval) {
				int startPos = Random.Range(0,6);
				int nextPos = (startPos & 1) == 1? startPos - 1 : startPos + 1;
				placeSingleLayingBird(new Vector3 (ENEMY_POS_X [startPos], cameraPos.y + interval, 0),
				                        new Vector3 (ENEMY_POS_X [nextPos], cameraPos.y + interval , 0),
				                        4,destoryIntervalY);
//				placeSingleLayingBird(new Vector3 (ENEMY_POS_X [startPos + 2], cameraPos.y + interval, 0),
//				                        new Vector3 (ENEMY_POS_X [startPos + 2 + nextPos], cameraPos.y + interval, 0),
//				                        4,destoryIntervalY);
//				placeSingleLayingBird(new Vector3 (ENEMY_POS_X [startPos + 4], cameraPos.y + interval, 0),
//				                        new Vector3 (ENEMY_POS_X [startPos + 4 + nextPos], cameraPos.y + interval, 0),
//				                        4,destoryIntervalY);
				lastCameraPos_Y_layEnemy = cameraPos.y;
			}
		}
		 
		private void placeSingleKinecticBird(Vector3 position, Vector3 detination, float speed , float destoryIntervalY){
			GameObject bird = Instantiate (enemyTiles[1], position, Quaternion.identity) as GameObject;
			EnemyManagement controller = bird.GetComponent<EnemyManagement> ();
			controller.Destination = detination;
			controller.Enemy_move_speed = speed;
			controller.DestroyIntervalY = destoryIntervalY;
			bird.transform.SetParent (enemiesRoot);
		}

		private void placeSingleStaticBird(Vector3 position){
			GameObject bird = Instantiate (enemyTiles[0], position, Quaternion.identity) as GameObject;
			bird.transform.SetParent (enemiesRoot);
		}

		private void placeSingleLayingBird(Vector3 position, Vector3 detination, float speed , float destoryIntervalY){
			GameObject bird = Instantiate (enemyTiles[2], position, Quaternion.identity) as GameObject;
			EnemyManagement controller = bird.GetComponent<EnemyManagement> ();
			controller.Destination = detination;
			controller.Enemy_move_speed = speed;
			controller.DestroyIntervalY = destoryIntervalY;
			bird.transform.SetParent (enemiesRoot);
		}

		private void drawTree(int height){
			 treeCrown = Instantiate(treeCrownTile,crownInitialPosition,Quaternion.identity) as GameObject;
			 treeCrown.transform.SetParent(boardHolder);

			for (int i=-50; i<height; i=i+7) {
				GameObject leftTreeElement = leftTreeTiles[Random.Range (0,leftTreeTiles.Length)];
				GameObject rightTreeElement = rightTreeTiles[Random.Range (0,rightTreeTiles.Length)];

				GameObject mostLeftTree =
					Instantiate (leftTreeElement, new Vector3 (TREE_POS_X[0],i, 0f), Quaternion.identity) as GameObject;
				GameObject LeftTree =
					Instantiate (leftTreeElement, new Vector3 (TREE_POS_X[1],i, 0f), Quaternion.identity) as GameObject;
				GameObject rightTree =
					Instantiate (rightTreeElement, new Vector3 (TREE_POS_X[2],i, 0f), Quaternion.identity) as GameObject;
				GameObject mostRightTree =
					Instantiate (rightTreeElement, new Vector3 (TREE_POS_X[3],i, 0f), Quaternion.identity) as GameObject;
					
				mostRightTree.transform.SetParent (boardHolder);
				mostLeftTree.transform.SetParent (boardHolder);
				LeftTree.transform.SetParent (boardHolder);
				rightTree.transform.SetParent (boardHolder);
			}
		}


		private void InitializeFruitPositions ()
		{
			fruitPositions.Clear ();
			for (int y = 3; y < rows; y ++) {
				for(int x = -7; x <= 7; x++){
					if(x == -3 || x == -2 || x == 2 || x == 3){
						continue;
					}
					fruitPositions.Add (new Vector3(x*gridLength, y*gridLength, 0f));
				}
			}
		}

		
		private Vector3 RandomFruitPosition ()
		{
			int randomIndex = Random.Range (0, fruitPositions.Count);
			Vector3 randomPosition = fruitPositions[randomIndex];
			fruitPositions.RemoveAt (randomIndex);
			return randomPosition;
		}
		
		private void LayoutFruitAtRandom (GameObject[] fruitArray, int minimum, int maximum, int level)
		{
			int objectCount = Random.Range (minimum, maximum+1)  + 5 * level;
			for(int i = 0; i < objectCount; i++)
			{
				Vector3 randomPosition = RandomFruitPosition();
				GameObject fruit = fruitArray[Random.Range (0, fruitArray.Length)];
				GameObject newFruit = Instantiate(fruit, randomPosition, Quaternion.identity) as GameObject;
				newFruit.transform.parent = fruitsRoot;
			}
		}

		private void ClearFruits()
		{
			fruitPositions.Clear ();
			Destroy (fruitsRoot.gameObject);
			fruitsRoot = new GameObject ("Fruits").transform;
			fruitsRoot.parent = transform;
			/*
			while (fruitsRoot.childCount > 0) {
				Destroy (fruitsRoot.GetChild(0).gameObject);
			}
			*/
		}
		private void ClearEnemies(){
			Destroy (enemiesRoot.gameObject );
			enemiesRoot = new GameObject ("Enemies").transform;
			enemiesRoot.parent = transform;

			/*while (enemiesRoot.childCount > 0) {
				Destroy (enemiesRoot.GetChild (0).gameObject);
			}*/
		}
	}
}