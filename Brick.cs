using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
	public static int bricksCounter = 0;

	public Sprite[] sprites;
	public AudioClip destroySound;
	public Rigidbody2D bonus;
	public Sprite indestructibleSprite;
	private Sprite orginalSprite;

	private int clipIndex;
	private int maxHits;
	private int timesHit;
	private LevelManager levelManager;
	private bool isBreakable;
	private bool isInvisible;

	// Use this for initialization
	void Start ()
	{
		levelManager = FindObjectOfType<LevelManager> ();
		isInvisible = this.tag == "invisible";
		setBreakable (this.tag == "breakable" || isInvisible);
		if (isBreakable || isInvisible) {
			bricksCounter++;
			levelManager.updateBallCounter ();
		}
		orginalSprite = GetComponent<SpriteRenderer> ().sprite;
		maxHits = sprites.Length + 1;
		if (isInvisible) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			maxHits = 2;
		}
		timesHit = 0;
	}
	
	void loadSprites ()
	{
		int spriteIndex = timesHit - 1;
		if (sprites [spriteIndex]) {
			orginalSprite = sprites [spriteIndex];
			GetComponent<SpriteRenderer> ().sprite = sprites [spriteIndex];
		} else {
			Debug.LogError ("Missing sprite for Brick. Add it!");
		}

	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.tag == "ball" || collision.gameObject.tag == "finger" || collision.gameObject.tag == "missle") {
			if (isBreakable) {
				AudioSource.PlayClipAtPoint (destroySound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
				handleHits ();
			}
		} 
	}

	public void setVisible ()
	{
		timesHit = 1;
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void makeBrickIndestructible ()
	{   
		//dodać wczytywanie 
//		GetComponent<Brick> ().setBreakable (false);
		setBreakable (false);
		GetComponent<SpriteRenderer> ().sprite = indestructibleSprite;
	}   

	public void makeBrickIndestructibleEnd ()
	{   
//		GetComponent<Brick> ().setBreakable (true);
		setBreakable (true);
		GetComponent<SpriteRenderer> ().sprite = orginalSprite;
	}   
	
	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "ball") {
			if (isBreakable) {
				destroyBrick ();
			}
		} else if (trigger.gameObject.tag == "finger") {
			if (isBreakable) {
				Destroy (trigger);
				handleHits ();
			}
		}
	}

	public void handleHits ()
	{
		timesHit += Ball.getDamage ();
		if (timesHit == 1 && isInvisible) {
			setVisible ();
		}
		if (timesHit >= maxHits) {
			destroyBrick ();
		} else {
			loadSprites ();
		}
	}

	void destroyBrick ()
	{
		bricksCounter--;
		Destroy (gameObject);
		checkForBonus ();
		levelManager.brickDestroyed ();
	}

	void checkForBonus ()
	{
		bonus = levelManager.getBonus ();
		if (bonus != null) {
			Rigidbody2D clone = Instantiate (bonus, transform.position, transform.rotation) as Rigidbody2D;
			clone.velocity = transform.TransformDirection (Vector2.down * 5);
		}
	}

	void setBreakable (bool breakable)
	{
		isBreakable = breakable;
	}

}
// void setBreakable (bool gac)}
//{  
//	bonus Gac = null }
//:3
