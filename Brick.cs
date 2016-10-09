using UnityEngine;
using System.Collections;
using System;


[System.Serializable]
public class Brick : MonoBehaviour
{
	public static int bricksCounter = 0;
	public static bool areIndestructible = false;


	public Sprite[] sprites;
	public AudioClip hitSound;
    public AudioClip indestructibleHitSound;
	public Rigidbody2D bonus;
	public Sprite indestructibleSprite;
    public bool isExploding = false;
	private Sprite orginalSprite;

	private int maxHits;
	private int timesHit;
	private LevelManager levelManager;
	private bool isBreakable;
    public bool IsBreakable { get{ return isBreakable;}}
	private bool isInvisible;
    public bool IsInvisible { get { return isInvisible; } }
    private bool exploded = false;

    private BRICK_TYPE brickType;

	public float width = 0.9f;
	public float height = 5f;
	public float speed = 5f;
	float xMin;
	float xMax;


    public Animator explosionAnimation;

    public enum BRICK_TYPE
    {
        INVISIBLE,
        EXPLODING,
        BUILDER,
        INDESTRUCTIBLE,
        NORMAL_1_HIT,
        NORMAL_2_HIT,
        NORMAL_3_HIT
    }

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

        if(isExploding)
        {
            GetComponent<Animator>().enabled = false;
        }
		maxHits = sprites.Length + 1;
		if (isInvisible) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			maxHits = 2;
		}
		timesHit = 0;
        assignCorrectBrickType();
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
        brick.brickType = brickType;
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
        Debug.Log("Trigger = " + trigger.name);
		if (trigger.gameObject.tag == "ball") {
			Ball ball = trigger.gameObject.GetComponent<Ball>();
			if (isBreakable) {
				ball.playSuperBallSound();
				destroyBrick ();
			}
		} else if(trigger.gameObject.tag == "missle")
        {
            if(isBreakable)
            {
                handleHits();
                Destroy(trigger.gameObject);
            }
        }
	}

	public void setVisible ()
	{
		timesHit = 1;
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
        isInvisible = false;
        tag = "breakable";//this might cause error when saving/loading bricks
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
        if (Ball.superBall() )//|| FindObjectOfType<Paddle>().isShooting())
        {
            GetComponent<PolygonCollider2D>().isTrigger = true;
        }
		setBreakable (true);
		GetComponent<SpriteRenderer> ().sprite = orginalSprite;
	}


	public void handleHits ()
	{
		timesHit++;
		if (timesHit == 1 && isInvisible) {
			setVisible ();
		}
		if (timesHit >= maxHits) {
			destroyBrick ();
		} else {
			//Invoke ("spawnBricks", 1f);// creating new bricks
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
					GameObject newBrick = Instantiate (gameObject, spawnPos, Quaternion.identity) as GameObject;
                    newBrick.transform.SetParent(GameObject.Find("Bricks").transform);
				}	
			}
		}
	}

	public int getLives ()
	{
		return timesHit;
	}

    //HOT FIX
    public void destroyBrickAfterExplosion()
    {
        Destroy(gameObject);
    }

	public void destroyBrick ()
	{
        
        if (isExploding && !exploded)
        {
            Animator anim = GetComponent<Animator>();
            anim.enabled = true;
            if (anim)
            {
                anim.Play("explode", 0);
            }
            else
            {
                Debug.Log("Ni ma animacji");
            }
           // Destroy(gameObject);
            explode();
            checkForBonus();
            levelManager.brickDestroyed();
        }
        else if(!isExploding)
        {
            Destroy(gameObject);
            if(isBreakable)
            {
                bricksCounter--;
                checkForBonus();
                levelManager.brickDestroyed();
            }
            
        }
	}

    void explode()
    {
        bricksCounter--;
        float explosionRadius = 1;
        exploded = true;
        //if (explosionAnimation)
       // {
            //Instantiate(explosionAnimation, transform.position, transform.rotation);
            //explosionAnimation.Play("explode");
 
            // }
        //else
       // {
        //    Debug.Log("No animation for explosion!");
        //}
            
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        AudioSource.PlayClipAtPoint(getHitSound(), transform.position, FindObjectOfType<MusicPlayer>().getVolume()); // something is wrong here
        foreach (Collider2D col in objectsInRange)
        {
            Brick brick = col.GetComponent<Brick>();
            if (brick != null && brick.transform.position != transform.position && !brick.wasExploded())
            {
                brick.Invoke("destroyBrick", 0.05f);
            }
        }


    }

    public bool wasExploded()
    {
        return exploded;
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

    public AudioClip getHitSound()
    {
        return (isBreakable == true) ? hitSound : indestructibleHitSound;
    }

	void OnDestroy ()
	{
		//delete from serialization
		//################SAVE######################
		LevelManager.SaveEvent -= SaveBrick;
		//#########################################
	}

    void assignCorrectBrickType()
    {
        if(isExploding)
        {
            brickType = BRICK_TYPE.EXPLODING;
        } else if (isInvisible)
        {
            brickType = BRICK_TYPE.INVISIBLE;
        } else if(!IsBreakable)
        {
            brickType = BRICK_TYPE.INDESTRUCTIBLE;
        } else if (maxHits == 3){
            brickType = BRICK_TYPE.NORMAL_3_HIT;
        } else if (maxHits == 2){
            brickType = BRICK_TYPE.NORMAL_2_HIT;
        } else if (maxHits == 1){
            brickType = BRICK_TYPE.NORMAL_1_HIT;
        }
        Debug.Log(brickType);
    }
}


// void setBreakable (bool gac)}
//{
//	bonus Gac = null }
//:3
