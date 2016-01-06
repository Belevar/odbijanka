using UnityEngine;
using System.Collections;

public class Bonus_ShowAll : Bonus
{

	public AudioClip bonusSound;

	void showInvisible ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position);
		var invisible = GameObject.FindGameObjectsWithTag ("invisible");
		foreach (var brick in invisible) {
			brick.GetComponent<Brick> ().setVisible ();
		}
		Destroy (gameObject);
	}

	override public	void activateBonus ()
	{
		showInvisible ();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}

