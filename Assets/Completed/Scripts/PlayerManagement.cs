using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace Completed{
	public class PlayerManagement : MonoBehaviour {

		public class AreaRange
		{
			public float leftBoard;            
			public float rightBoard;  
			public AreaRange (float leftBoard, float rightBoard)
			{
				this.leftBoard = leftBoard;
				this.rightBoard = rightBoard;
			}
		}

		public float moveSide2SideSpeed;
		public float moveUpSpeed;

		public float upBorder = 360;

		private float initMoveUpSpeed ;

		private bool onLeft;
		private bool jump;
		private bool shift;
		private bool goUp;

		public GameObject panel; 
		public GameObject failpanel;
		public Text countText; // Showing text for player's Points
		private int PlayerScore; // Player's points
		public int passLevelScore = 200; // points to pass current level

		public AudioClip jumpSound;
		public AudioClip eatFruitSound;
		public AudioClip gameOverSound;
		public AudioClip completeSound;

		public static PlayerManagement instance = null;	
		private static List<AreaRange> AREA_BOARD = new List<AreaRange>();
		private int curAreaIndex;
		private int nextAreaIndex;
		private Animator PlayerAnimator;

		public GameObject scoreTile;
				
		//Count number of banana, bean, pear
		int num_banana, num_bean, num_pear;
		public Text count_banana;
		public Text count_bean;
		public Text count_pear;

		private Vector3 initialPosition = new Vector3(6,-12,0); // Warning: must always keep this same with player position;
		private Quaternion initialRotation;
		private bool enablePlayerBehavor = true;

		//Timer
		public Text timerUI;
		float timeRemaining;
		public float warningTime;

		void Awake(){
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}
			DontDestroyOnLoad(gameObject);

			//Space between trees
			AREA_BOARD.Add (new AreaRange (-27, -16));
			AREA_BOARD.Add (new AreaRange(-6,6));
			AREA_BOARD.Add (new AreaRange(16,27));
			
			PlayerAnimator = GetComponent<Animator> ();
			//initialPosition = transform.position;
			initialRotation = transform.rotation;
			initMoveUpSpeed = moveUpSpeed;
		}

		void Start () {
			CreateLevel ();
		}

		void CreateLevel()
		{
			CameraController.instance.enable = true;
			panel.SetActive (false);
			failpanel.SetActive (false);
			PlayerScore = 0;
			num_banana = 0;
			num_bean = 0;
			num_pear = 0;
			fruit_count_update ();
			countText.text = "Points: " + PlayerScore.ToString () + "/" + passLevelScore.ToString();
			initPlayer ();

			//set timer

			timeRemaining=(upBorder-initialPosition.y)/moveUpSpeed;
			updateTimerUI ();

		}
		void initPlayer(){
			transform.position = initialPosition;
			transform.rotation = initialRotation;
			moveUpSpeed = initMoveUpSpeed + 2;
			goUp = true;
			curAreaIndex = 1;
			nextAreaIndex = -1;
			jump = false;
			shift = false;
			PlayerAnimator.SetTrigger ("LevelStart");
			GetComponent<Collider2D> ().enabled = true;
			enablePlayerBehavor = true;
			GetComponent<Rigidbody2D>().isKinematic=true;
		}
		private void initPlayerMoveStatus (){
			jump = false;
			shift = false;
		}

		public void LoadNextLevel () {
			CreateLevel ();
		}
		
		void FixedUpdate () {
				if (goUp) {
					moveTo (transform.position + Vector3.up, moveUpSpeed);
				}
				if (jump) {
					if (onLeft) {
						jumpFromSideToSide (AREA_BOARD [curAreaIndex].leftBoard, AREA_BOARD [curAreaIndex].rightBoard, AREA_BOARD [curAreaIndex].rightBoard);
					} else {
						jumpFromSideToSide (AREA_BOARD [curAreaIndex].leftBoard, AREA_BOARD [curAreaIndex].rightBoard, AREA_BOARD [curAreaIndex].leftBoard);
					}
				} else if (shift) {
					if (onLeft) {
						shiftToOtherSide (new Vector3 (AREA_BOARD [nextAreaIndex].rightBoard, transform.position.y, transform.position.z), moveSide2SideSpeed, false);
					} else {
						shiftToOtherSide (new Vector3 (AREA_BOARD [nextAreaIndex].leftBoard, transform.position.y, transform.position.z), moveSide2SideSpeed, true);
					}
				}
		}

		void Update(){
			if (!enablePlayerBehavor) {
				return;
			}

			//update timer
			timeRemaining -= Time.deltaTime;
			updateTimerUI ();


			if (transform.position.y > upBorder - 17)
				CameraController.instance.enable = false;
			if (transform.position.y > upBorder) {
				gameOver ();
			}
			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0) {
				//Store the first touch detected.
				Touch myTouch = Input.touches [0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Ended) {
					if (myTouch.position.x < Screen.width / 2) {
						//left movement
						if (!jump && !shift) {
							if (!playerOnLeft ())
								playerJump ();
							else if ((curAreaIndex != 0)) {
								shift = true;
								nextAreaIndex = curAreaIndex - 1;
								onLeft = true;
								PlayerAnimator.SetTrigger ("PlayerShift");
							}
						}
					}
					if (myTouch.position.x > Screen.width / 2) {
						//right movement
						if (!jump && !shift) {
							if (playerOnLeft ()) {
								playerJump ();
							} else if ((curAreaIndex != AREA_BOARD.Count - 1)) {
								shift = true;
								nextAreaIndex = curAreaIndex + 1;
								onLeft = false;
								PlayerAnimator.SetTrigger ("PlayerShift");
							}
						}
					}
				}
			}
			//For test
			if (!jump && !shift) {
				if (Input.GetButtonUp ("right")) {
					if (playerOnLeft ()) {
						playerJump ();
					} else if ((curAreaIndex != AREA_BOARD.Count - 1)) {
						shift = true;
						nextAreaIndex = curAreaIndex + 1;
						onLeft = false;
						PlayerAnimator.SetTrigger ("PlayerShift");
					}
				} else if (Input.GetButtonUp ("left")) {
					if (!playerOnLeft ())
						playerJump ();
					else if ((curAreaIndex != 0)) {
						shift = true;
						nextAreaIndex = curAreaIndex - 1;
						onLeft = true;
						PlayerAnimator.SetTrigger ("PlayerShift");
					}
				}
			}
		}

		void OnTriggerEnter2D (Collider2D other)  
		{	
			if(!enablePlayerBehavor){
				return;
			}
			if (other.tag == "Enemy1" || other.tag == "Enemy2" || other.tag == "Enemy3" || other.tag == "Egg"){
				if(other.tag == "Egg"){
					Destroy(other.gameObject);
				}
				gameOver();
				return;
			}
			if (other.tag == "Fruit1" || other.tag == "Fruit2" ||other.tag == "Fruit3")
			{
				SoundManager.instance.PlaySingle (eatFruitSound);
				Vector3 scorePosition = other.transform.position;
				Destroy(other.gameObject);
				PlayerScore = PlayerScore + 10;
				//Show score
				GameObject score = Instantiate (scoreTile, scorePosition, Quaternion.identity) as GameObject;
				Destroy(score, 1);
				//Fruit Count detail update
				if(other.tag == "Fruit1") num_banana++;
				if(other.tag == "Fruit2") num_bean++;
				if(other.tag == "Fruit3") num_pear++;
				countText.text = "Points: " + PlayerScore.ToString () + "/" + passLevelScore.ToString();
				fruit_count_update ();
			}
			checkIfGameIsOver(PlayerScore);
		}
		
		void checkIfGameIsOver(int PlayerScore) {
			if (PlayerScore >= passLevelScore)
			{
				//MainMenuStart.changeToNextScene(2);
				SoundManager.instance.PlayBackground(completeSound);
				nextLevel();
			}
		}

		void gameOver(){
			SoundManager.instance.PlayBackground (gameOverSound);
			playerFall();
			failpanel.SetActive (true);
			GetComponent<Collider2D> ().enabled = false;
			enablePlayerBehavor = false;
			//enabled = false;
			//GameManager.instance.enabled = false;
		}

		void nextLevel(){
			panel.SetActive (true);
			goUp = false;
			moveUpSpeed = 0;
			enablePlayerBehavor = false;
		}

		private void playerFall(){
			initPlayerMoveStatus ();
			moveUpSpeed = 0;
			GetComponent<Rigidbody2D> ().isKinematic = false;
			PlayerAnimator.SetTrigger("PlayerHit");
			PlayerAnimator.SetBool ("PlayerFall",true);
		}

		private void playerJump(){
			SoundManager.instance.PlaySingle (jumpSound);
			PlayerAnimator.SetTrigger("PlayerJump");
			onLeft = playerOnLeft ();
			jump=true;
			GetComponent<Rigidbody2D>().isKinematic=false; 
			GetComponent<Rigidbody2D>().velocity=Vector3.up*(float)((AREA_BOARD[curAreaIndex].rightBoard - AREA_BOARD[curAreaIndex].leftBoard) / moveSide2SideSpeed * 9.8)/2;
		}

		private void moveTo(Vector3 destination, float speed){
			transform.position = Vector3.MoveTowards (transform.position,destination,speed * Time.deltaTime);
		}
		
		private bool playerOnLeft(){
			return transform.position.x <= AREA_BOARD[curAreaIndex].leftBoard;
		}
		
		private void shiftToOtherSide(Vector3 destination,float moveSpeed, bool toRight){
			moveTo (destination,moveSpeed);
			if (toRight && transform.position.x >= destination.x || !toRight && transform.position.x <= destination.x) {
				shift = false;
				PlayerAnimator.SetTrigger ("PlayerRun");
				transform.Rotate(new Vector3(0,180,0));
				curAreaIndex = nextAreaIndex;
			}
		}
		private void jumpFromSideToSide(float leftBoundry, float rightBoundry, float destination){
			moveTo (new Vector3(destination,transform.position.y,transform.position.z),moveSide2SideSpeed);
			if (transform.position.x >= rightBoundry || transform.position.x <= leftBoundry) {
				jump = false;
				GetComponent<Rigidbody2D>().isKinematic=true;
				PlayerAnimator.SetTrigger("PlayerRun");
				transform.Rotate(new Vector3(0,180,0));
			}
		}

		//This Function is used to update text info of fruit tracking
		private void fruit_count_update()
		{
			count_banana.text = num_banana.ToString ();
			count_bean.text = num_bean.ToString ();
			count_pear.text = num_pear.ToString ();
		}

		private void updateTimerUI (){
			timerUI.text = "Time Remain:" + (int)timeRemaining + "s";
			if (timeRemaining > warningTime) {
				timerUI.color = Color.yellow;
			} else {
				timerUI.color = Color.red;
			}
		}

	}

}
