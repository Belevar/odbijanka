using UnityEngine;
using System.Collections;

public class Bonus_Shooting : Bonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		Paddle paddle = FindObjectOfType<Paddle> ();
		if (paddle == null) {
			Debug.LogError ("O Panie kto panu tu tak spier***!");
		}
		paddle.activateShooting ();
		Destroy (gameObject);
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

