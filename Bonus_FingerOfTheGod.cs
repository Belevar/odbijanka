using UnityEngine;
using System.Collections;

public class Bonus_FingerOfTheGod : TimeBonus
{
	
	public AudioClip bonusSound;

	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		/*Paddle paddle = FindObjectOfType<Paddle> ();
		if (paddle == null) {
			Debug.LogError ("O Panie kto panu tu tak spier***!");
		}
		paddle.activateShooting ();*/
		FindObjectOfType<BonusManager> ().registerTimeBonus (this);
		transform.position = new Vector3 (-10f, -10f, -10f);
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	override public	void disactivate ()
	{
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}

