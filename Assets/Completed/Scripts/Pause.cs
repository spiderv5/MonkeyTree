using UnityEngine;
using System.Collections;

namespace Completed{
	public class Pause : MonoBehaviour {

		// Has the game been paused?
		private bool isPaused;
		// Has the game gone through it's pause transition?
		private bool hasPaused = false;
		// Use this for initialization

		public void Start () {
			// Default to not paused.
			this.isPaused = false;

		}
		public void OnGUI()
		{   
			changePauseState ();
			// If you're paused
			if(this.isPaused) {
				// And nothing has initialized
				if(!this.hasPaused) {
					// Pause the game
					this.pauseGame();
					this.hasPaused = true;
				}
				// GUI changes.
			} else {
				if(this.hasPaused) {
					this.resumeGame();
					this.hasPaused = false;
				}
			}
			
			// If you click the button,
	//		if(GUI.Button(new Rect(100,100,100,100), "PAUSE")) {
	//			// Toggle the games pause state.
	//			this.changePauseState();
	//		}
			
		}
		
		public void pauseGame()
		{
			// Pause the game
			Time.timeScale = 0;
			// Any other logic
			//GUI.Button (new Rect (100, 100, 100, 100), "PAUSE");
			GameObject GM= GameObject.Find("GameManager");
			GameObject panel=GM.GetComponent<GameManager> ().pausepanel;
			panel.SetActive (true);
		}
		
		public void resumeGame()
		{
			// Resume the game
			Time.timeScale = 1;
			// Any other logic
			GameObject GM= GameObject.Find("GameManager");
			GameObject panel=GM.GetComponent<GameManager> ().pausepanel;
			panel.SetActive (false);
		}
		
		public void changePauseState()
		{
			// Alternate bool value per button click
			this.isPaused = !this.isPaused;
		}

	}
}
