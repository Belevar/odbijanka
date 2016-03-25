using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bonus_Indestructible : TimeBonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		Brick[] bricksInGame = FindObjectsOfType<Brick> ();
        Brick.areIndestructible = true;
		foreach (Brick brick in bricksInGame) {
			if (brick.tag == "breakable") {
				brick.makeBrickIndestructible ();
			}
		}
		FindObjectOfType<BonusManager> ().registerTimeBonus (this);
		transform.position = new Vector3 (-10f, -10f, -10f);
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	override public	void disactivate ()
	{
        Brick.areIndestructible = false;
		Brick[] bricksInGame = FindObjectsOfType<Brick> ();
		foreach (Brick brick in bricksInGame) {
			if (brick.tag == "breakable") {
				brick.makeBrickIndestructibleEnd ();
			}
		}
	}
	
	
	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}

