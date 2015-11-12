using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Enemy2 : MovingObject {
		
//		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
		private int speed = 2;
		// Use this for initialization
		protected override void Start ()
		{
			//Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
			//This allows the GameManager to issue movement commands.
			//GameManager.instance.AddEnemyToList (this);
			
			//Get and store a reference to the attached Animator component.
//			animator = GetComponent<Animator> ();
			
			//Find the Player GameObject using it's tag and store a reference to its transform component.
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			
			//Call the start function of our base class MovingObject.
			base.Start ();
		}
		//Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
		//See comments in MovingObject for more on how base AttemptMove function works.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
			//Check if skipMove is true, if so set it to false and skip this turn.
			if(skipMove)
			{
				skipMove = false;
				return;
				
			}
			
			//Call the AttemptMove function from MovingObject.
			base.AttemptMove <T> (xDir, yDir);
			
			//Now that Enemy has moved, set skipMove to true to skip next move.
			skipMove = true;
		}
		
		
		// Update is called once per frame
		void FixedUpdate () {
			// The step size is equal to speed times frame time.
			MoveEnemy ();
		}
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveEnemy ()
		{
			//Declare variables for X and Y axis move directions, these range from -1 to 1.
			//These values allow us to choose between the cardinal directions: up, down, left and right.
//			float xDir = transform.position.x;
			float yDir = transform.position.y;
			float distance =  transform.position.y-target.position.y ;
			Debug.Log (transform.position.y);
			Debug.Log (target.position.y);
			Debug.Log (distance);
			//If the difference in positions is approximately zero (Epsilon) do the following:
			if (target.position.y - transform.position.y < 200.0f) {
				float step2 = speed * Time.deltaTime*20;
				// Move our position a step closer to the target.
				//transform.position = Vector3.MoveTowards(transform.position, transform.position+Vector3.up, step);
				if (transform.position.y < yDir -9) {
					transform.position = Vector3.MoveTowards (transform.position, transform.position + Vector3.up, step2);
				}
				if (transform.position.y > yDir+9) {
					transform.position = Vector3.MoveTowards (transform.position, transform.position + Vector3.down, step2);
				}
			}
		}
		
		
		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
		//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
		protected override void OnCantMove <T> (T component)
		{
			//Declare hitPlayer and set it to equal the encountered component.
			//Player hitPlayer = component as Player;
			
			//Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
			//hitPlayer.LoseFood (playerDamage);
			
			//Set the attack trigger of animator to trigger Enemy attack animation.
			//animator.SetTrigger ("enemyAttack");
			
			//Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
			
		}
	}
}