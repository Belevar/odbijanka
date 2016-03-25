using UnityEngine;
using System.Collections;

public class Bonus_FingerOfTheGod : TimeBonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		FindObjectOfType<BonusManager> ().registerTimeBonus (this);
		transform.position = new Vector3 (-10f, -10f, -10f);
        FindObjectOfType<LevelManager>().activateFingerOfGod();
        GetComponent<SpriteRenderer> ().enabled = false;
	}

	override public	void disactivate ()
	{
        FindObjectOfType<LevelManager>().disactivateFingerOfGod();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}

