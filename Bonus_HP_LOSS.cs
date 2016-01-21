using UnityEngine;
using System.Collections;

public class Bonus_HP_LOSS : Bonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		LevelManager manager = FindObjectOfType<LevelManager> ();
		if (manager == null) {
			Debug.LogError ("O Panie kto panu tu tak spier***!");
		} else {
			Destroy (gameObject);
			manager.loseLiveAndCheckEndGame ();
		}
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

