using UnityEngine;
using System.Collections;

public class Bonus_Speed : Bonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position);
		Ball[] ballsInGame = FindObjectsOfType<Ball> ();
		foreach (Ball ball in ballsInGame) {
			ball.speedUp ();
		}
		Destroy (gameObject);
	}
	
	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}
