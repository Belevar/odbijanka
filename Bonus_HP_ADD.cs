using UnityEngine;
using System.Collections;

public class Bonus_HP_ADD : Bonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position);
		LevelManager manager = GameObject.FindObjectOfType<LevelManager> ();
		if (manager == null) {
			Debug.LogError ("O Panie kto panu tu tak spier***!");
		} else {
			manager.addLive ();
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}

