using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

namespace Completed
	
{
	public class StartMenuManager : MonoBehaviour
	{
		public GameObject[] enemy;
		
		private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
		private Transform fruitsRoot;
		private Transform enemiesRoot;

		private float lastCameraPos_Y_staticEnemy = 0;
		private float lastCameraPos_Y_kinectEnemy = 10;
		private float lastCameraPos_Y_layEnemy = 20;

		private float birdIntervalY = 1.5f;
		private float birdMaxY = 2;
		void Awake()
		{
			enemiesRoot = new GameObject ("Enemies").transform;
			enemiesRoot.parent = transform;
			for(int i = 0; i < 5 ; i++){
				enemy[i].transform.position =  new Vector3(-10, birdMaxY - i * birdIntervalY,0);
				EnemyManagement controller = enemy[i].GetComponent<EnemyManagement> ();
				controller.Destination = new Vector3(10, birdMaxY - i * birdIntervalY,0);
				controller.Enemy_move_speed = 8;
				controller.DestroyIntervalY = float.MaxValue;
			}
		}

		float timeBird1 = 0;
		float timeBird2 = 3;
		float timeIntervalBird1 = 6;
		float timeIntervalBird2 = 6;
		void Update(){
			timeBird1 += Time.deltaTime;
			timeBird2 += Time.deltaTime;
			if (timeBird1 > timeIntervalBird1) {
				for(int i = 0; i < 5 ; i++){
					enemy[i].transform.position =  new Vector3(-10, birdMaxY - i * birdIntervalY,0);
				}
				timeBird1 = 0;
			}
			if (timeBird2 > timeIntervalBird2) {
				for (int i =5; i < 10; i++) {
					enemy[i].transform.position =  new Vector3(-10, birdMaxY - (i-5) * birdIntervalY,0);
					EnemyManagement controller = enemy[i].GetComponent<EnemyManagement> ();
					controller.Destination = new Vector3(10, birdMaxY - (i-5) * birdIntervalY,0);
					controller.Enemy_move_speed = 8;
					controller.DestroyIntervalY = float.MaxValue;
				}
				timeBird2 = 0;
			}
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