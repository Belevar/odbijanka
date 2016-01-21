using UnityEngine;
using System.Collections;

public class Bonus_SuperBall : Bonus
{
	
	public AudioClip bonusSound;
	
	override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		Destroy (gameObject);
		GameObject[] bricksInGame = GameObject.FindGameObjectsWithTag ("breakable");
		foreach (GameObject brick in bricksInGame) {
			brick.GetComponent<PolygonCollider2D> ().isTrigger = true;
		}
		Ball[] ballsInGame = FindObjectsOfType<Ball> ();
		foreach (Ball ball in ballsInGame) {
			ball.changeBallMode (Ball.BALL_MODE.SUPER);
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

