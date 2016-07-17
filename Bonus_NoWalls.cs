using UnityEngine;
using System.Collections;

public class Bonus_NoWalls : TimeBonus
{
	public AudioClip bonusSound;

	override public void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		GameObject.Find ("LeftWall").GetComponent<BoxCollider2D> ().isTrigger = true;
		GameObject.Find ("RightWall").GetComponent<BoxCollider2D> ().isTrigger = true;
		FindObjectOfType<BonusManager> ().registerTimeBonus (this);
		Ball.wallsArePresent = false;
		GetComponent<SpriteRenderer> ().enabled = false;
		transform.position = new Vector3 (-10f, -10f, -10f);
	}

	override public void disactivate ()
	{
		Ball.wallsArePresent = true;
		GameObject.Find ("LeftWall").GetComponent<BoxCollider2D> ().isTrigger = false;
		GameObject.Find ("RightWall").GetComponent<BoxCollider2D> ().isTrigger = false;
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}
