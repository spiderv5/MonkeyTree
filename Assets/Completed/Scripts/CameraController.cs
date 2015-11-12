using UnityEngine;
using System.Collections;

namespace Completed{
	public class CameraController : MonoBehaviour {
		public float cameraSpeed;
		private Vector3 initialPosition;
		public static CameraController instance = null;
		public bool enable = true;
		void Awake(){
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}
			DontDestroyOnLoad(gameObject);
			initialPosition = transform.position;
		}

		// Use this for initialization
		public void CreateLevel () {
			transform.position = initialPosition;
		}
		
		// Update is called once per frame
		void Update () {
			if (enable) {
				cameraSpeed = PlayerManagement.instance != null ? PlayerManagement.instance.moveUpSpeed : 0;
				transform.position = Vector3.MoveTowards (transform.position, transform.position + Vector3.up, cameraSpeed * Time.deltaTime);
			}
		}
	}
}

