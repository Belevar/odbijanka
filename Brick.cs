using UnityEngine;
using System.Collections;
using System;


[System.Serializable]
public class Brick : MonoBehaviour
{
	public static int bricksCounter = 0;
	public static bool areIndestructible = false;


	public Sprite[] sprites;
	public AudioClip destroySound;
	public Rigidbody2D bonus;
	public Sprite indestructibleSprite;
	public bool isMoveable;
	private Sprite orginalSprite;

	private int maxHits;
	private int timesHit;
	private LevelManager levelManager;
	private bool isBreakable;
	private bool isInvisible;

	public float width = 0.9f;
	public float height = 5f;
	public float speed = 5f;
	bool movingRight = true;
	float xMin;
	float xMax;


	// Use this for initialization

	void Awake ()
	{
		//################SAVE######################
		LevelManager.SaveEvent += SaveBrick;
		//#########################################

		areIndestructible = false;
		levelManager = FindObjectOfType<LevelManager> ();
		isInvisible = this.tag == "invisible";
		setBreakable (this.tag == "breakable" || isInvisible);

		if (isBreakable || isInvisible) {
			bricksCounter++;
			levelManager.updateBrickCounter ();
		}
		orginalSprite = GetComponent<SpriteRenderer> ().sprite;
		maxHits = sprites.Length + 1;
		if (isInvisible) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			maxHits = 2;
		}
		timesHit = 0;
	}

	public void SaveBrick (object sender, EventArgs args)
	{
		SaveBrick brick = new SaveBrick ();
		brick.isInvisible = isInvisible;
		brick.maxHit = maxHits;
		brick.PositionX = transform.position.x;
		brick.PositionY = transform.position.y;
		brick.timesHit = timesHit;
		brick.tag = this.tag;
		levelManager.savedGame.savedBricks.Add (brick);

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
		if (collision.gameObject.tag == "missle" || collision.gameObject.tag == "ball") {
			if (isBreakable) {
				handleHits ();
			}
		}
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "ball") {
			if (isBreakable) {
				destroyBrick ();
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
		if (Ball.superBall () || FindObjectOfType<Paddle> ().isShooting ()) {
			GetComponent<PolygonCollider2D> ().isTrigger = false;
		}
		setBreakable (false);
		GetComponent<SpriteRenderer> ().sprite = indestructibleSprite;
	}

	public void makeBrickIndestructibleEnd ()
	{
		setBreakable (true);
		GetComponent<SpriteRenderer> ().sprite = orginalSprite;
	}


	public void handleHits ()
	{
		AudioSource.PlayClipAtPoint (destroySound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
		timesHit++;
		if (timesHit == 1 && isInvisible) {
			setVisible ();
		}
		if (timesHit >= maxHits) {
			destroyBrick ();
		} else {
			//Invoke ("spawnBricks", 1f);
			loadSprites ();
		}
	}

	public void spawnBricks ()
	{
		const float radius = 0.2f;
		Vector2[] newBricksSpawnPoints = {
			new Vector2 (2f, 0f),
			new Vector2 (-2f, 0f),
			new Vector2 (0f, 0.6f),
			new Vector2 (0f, -0.6f)
		};

		foreach (Vector2 pos in newBricksSpawnPoints) {
			Vector2 spawnPos = transform.position;
			spawnPos = spawnPos + pos;

			if (spawnPos.x + transform.localScale.x > 16f || spawnPos.x - transform.localScale.x < 0f) {
				Debug.Log ("Pozycja: " + spawnPos + ": Poza ekranem");
			} else {
				if (Physics2D.OverlapCircle (spawnPos, radius)) {
					Debug.Log ("Pozycja: " + spawnPos + ":FOUND object - ignore this spawn point");
				} else {
					Debug.Log ("Pozycja: " + spawnPos + ":CREATE");
					Instantiate (gameObject, spawnPos, Quaternion.identity);
				}	
			}
		}
	}

	public int getLives ()
	{
		return timesHit;
	}

	void destroyBrick ()
	{
		bricksCounter--;
		Destroy (gameObject);
		checkForBonus ();
		levelManager.brickDestroyed ();
	}

	public void destroyBrickOnLoad ()
	{
		bricksCounter--;
		Destroy (gameObject);
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

	void OnMouseDown ()
	{
		if (!levelManager.gameIsPaused () && isBreakable && levelManager.isFingerOfGodActive ()) {
			destroyBrick ();
		}
       
	}

	public void setMaxHits (int hits)
	{
		maxHits = hits;
	}

	public void setTimesHit (int hits)
	{
		timesHit = hits;
	}

	void OnDestroy ()
	{
		//delete from serialization
		//################SAVE######################
		LevelManager.SaveEvent -= SaveBrick;
		//#########################################
	}
}


// void setBreakable (bool gac)}
//{
//	bonus Gac = null }
//:3
