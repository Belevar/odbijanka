using UnityEngine;
using System.Collections;

public class Bonus_SuperBall : TimeBonus
{
	
	public AudioClip bonusSound;
	public float bonusSoundMultiplayer = 1.2f;
	
    override public	void activateBonus ()
	{
		AudioSource.PlayClipAtPoint (bonusSound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
        Debug.Log("VOLUME: " + FindObjectOfType<MusicPlayer>().getVolume());
        Brick[] bricksInGame = FindObjectsOfType<Brick> ();
        if (!Brick.areIndestructible)
        {
            foreach (Brick brick in bricksInGame)
            {
                if(brick.tag == "breakable" || brick.tag == "invisible")
                {
                    brick.GetComponent<PolygonCollider2D>().isTrigger = true;
                }
            }
        }
		Ball[] ballsInGame = FindObjectsOfType<Ball> ();
		foreach (Ball ball in ballsInGame) {
			ball.changeBallMode (Ball.BALL_MODE.SUPER);
		}
		FindObjectOfType<BonusManager> ().registerTimeBonus (this);
		transform.position = new Vector3 (-10f, -10f, -10f);
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	override public	void disactivate ()
	{
        Ball[] ballsInGame = FindObjectsOfType<Ball>();
        foreach (Ball ball in ballsInGame)
        {
            ball.changeBallMode(Ball.BALL_MODE.NORMAL);
        }
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			activateBonus ();
		}
	}
}

