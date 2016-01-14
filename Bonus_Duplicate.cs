using UnityEngine;
using System.Collections;

public class Bonus_Duplicate : Bonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		Ball[] ballsInGame = FindObjectsOfType<Ball> ();
		foreach (Ball ball in ballsInGame) {
			ball.duplicateBall ();
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

