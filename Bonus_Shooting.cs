using UnityEngine;
using System.Collections;

public class Bonus_Shooting : Bonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position);
		Paddle paddle = GameObject.FindObjectOfType<Paddle> ();
		if (paddle == null) {
			Debug.LogError ("O Panie kto panu tu tak spier***!");
		}
		paddle.activateShooting ();
		Destroy (gameObject);
	}
	
	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}

