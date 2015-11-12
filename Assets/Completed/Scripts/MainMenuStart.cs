using UnityEngine;
using System.Collections;

namespace Completed{
	public class MainMenuStart : MonoBehaviour {

		public void changeScene(string scenname){
			Application.LoadLevel (scenname);
		}

		public static void changeToNextScene(int i){
			int sceneIdx = Application.loadedLevel;
			sceneIdx++;
			if (sceneIdx == Application.levelCount)
				sceneIdx = 0;
			Application.LoadLevel (i);
		}

		public void quit(){
			Application.Quit ();
		}
	}
}