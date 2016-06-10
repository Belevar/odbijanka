using UnityEngine;
using System.Collections;

public class Bonus_NoWalls : TimeBonus
{
	public AudioClip bonusSound;

	override public void activateBonus ()
	{
		Debug.Log ("Activate Bonus_NoWalls start");
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		GameObject.Find ("LeftWall").GetComponent<BoxCollider2D> ().isTrigger = true;
		GameObject.Find ("RightWall").GetComponent<BoxCollider2D> ().isTrigger = true;
		FindObjectOfType<BonusManager> ().registerTimeBonus (this);
		Ball.wallsArePresent = false;
		GetComponent<SpriteRenderer> ().enabled = false;
		transform.position = new Vector3 (-10f, -10f, -10f);
		Debug.Log ("Activate Bonus_NoWalls end");
	}

	override public void disactivate ()
	{
		Debug.Log ("Disactivate Bonus_NoWalls start");
		Ball.wallsArePresent = true;
		GameObject.Find ("LeftWall").GetComponent<BoxCollider2D> ().isTrigger = false;
		GameObject.Find ("RightWall").GetComponent<BoxCollider2D> ().isTrigger = false;
		Debug.Log ("Disactivate Bonus_NoWalls end");
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}
