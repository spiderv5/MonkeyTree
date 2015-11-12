using UnityEngine;
using System.Collections;

namespace Completed
{
	public class EnemyManagement: MonoBehaviour {
		public bool couldMove = false;
		public bool couldLay = false;
		public float enemy_move_speed = 2;
		public GameObject EGG;
	
		private Transform target;
		private Transform Enemy_object;
		private float move_direction;
		private Vector3 destination;
		private Vector3 initPosition;
		private float destroyIntervalY = 25;
		private float time = 0;
		private float interpolationTime = 2;
		private GameObject newEgg;

		public Vector3 Destination {
			get {
				return destination;
			}
			set {
				destination = value;
			}
		}

		public float Enemy_move_speed {
			get {
				return enemy_move_speed;
			}
			set {
				enemy_move_speed = value;
			}
		}

		public float DestroyIntervalY {
			get {
				return destroyIntervalY;
			}
			set {
				destroyIntervalY = value;
			}
		}

		// Use this for initialization
		void Start ()
		{	
			initPosition = transform.position;
		}
		void Update(){
			if (CameraController.instance !=null && transform.position.y < CameraController.instance.transform.position.y - destroyIntervalY) {
				Destroy(gameObject);
			}
			if (couldLay) {
				time += Time.deltaTime;
				if(time > interpolationTime){
					if(newEgg != null){
						Destroy (newEgg.gameObject,0);
					}
					newEgg = Instantiate (EGG, transform.position, Quaternion.identity)as GameObject;
					newEgg.transform.SetParent(gameObject.transform.parent);
					time = 0;
				}
			}
		}
		// Update is called once per frame
		void FixedUpdate () {
			if (couldMove) {
				transform.position = Vector3.MoveTowards (transform.position,destination,enemy_move_speed * Time.deltaTime);
			}
			if(couldLay){
				transform.position = Vector3.MoveTowards (transform.position,destination,enemy_move_speed * Time.deltaTime);
				if(Mathf.Abs(transform.position.x - destination.x) < 0.01){
					Vector3 tmp = initPosition;
					initPosition = destination;
					destination = tmp;
				}
			}
		}

	}
}