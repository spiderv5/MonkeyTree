using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Collider : MonoBehaviour {

	public Text countText; // Showing text for player's Points
	public Text winText; //Winning text to show, we may not need it
	private int count; // Player's points

	

	private void OnTriggerEnter2D (Collider2D other)  
	{	
		if (other.tag == "fruit1")
		{
			Destroy(other.gameObject);
			count = count + 10;
		}
		else if (other.tag == "fruit2")
		{
			Destroy(other.gameObject);
			count = count + 20;
		}
		else if (other.tag == "fruit3")
		{
			Destroy(other.gameObject);
			count = count + 30;
		}
		else if (other.tag == "enemy1")
		{
			Destroy(other.gameObject);
			count = count - 10;
		}
		else if (other.tag == "enemy2")
		{
			Destroy(other.gameObject);
			count = count - 20;
		}
		else if (other.tag == "enemy3")
		{
			Destroy(other.gameObject);
			count = count - 30;
		}
	}
	
	void SetCountText ()
	{
		countText.text = "Points: " + count.ToString ();
		if (count >= 10000)
		{
			//Next level
			winText.text = "Congratulations!";
		}
	}


}