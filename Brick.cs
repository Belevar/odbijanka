using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
	public static int bricksCounter = 0;

	public Sprite[] sprites;
	public AudioClip destroySound;
	public Rigidbody2D bonus;

	private int maxHits;
	private int timesHit;
	private LevelManager levelManager;
	private bool isBreakable;
	private bool isInvisible;

	// Use this for initialization
	void Start ()
	{
		isInvisible = this.tag == "invisible";
		setBreakable (this.tag == "breakable" || isInvisible);
		if (isBreakable || isInvisible) {
			bricksCounter++;
		}
		maxHits = sprites.Length + 1;
		if (isInvisible) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			maxHits = 2;
		}

		timesHit = 0;
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void loadSprites ()
	{
		int spriteIndex = timesHit - 1;
		if (sprites [spriteIndex]) {
			this.GetComponent<SpriteRenderer> ().sprite = sprites [spriteIndex];
		} else {
			Debug.LogError ("Missing sprite for Brick. Add it!");
		}

	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		AudioSource.PlayClipAtPoint (destroySound, transform.position);
		
		if (collision.gameObject.tag == "ball") {
            if(Ball.superBall())
            {
                handleSuperBallAttack();
            }
            else if (isBreakable) {
			    handleHits ();
		    
            }
       } else {
            handleHits();
       }
            
            
	}

	public void setVisible ()
	{
		timesHit = 1;
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void makeAllBricksIndestructible ()
	{   
		var invisible = GameObject.FindGameObjectsWithTag ("breakable");
		foreach (var brick in invisible) {
			brick.GetComponent<Brick> ().setBreakable (false);
		}
    }   

    void handleSuperBallAttack()
    {
        Debug.LogError("Pewnie ta funkcja bedzie do usuniecia - trzeba wylaczyc kolizje");
    }


	void handleHits ()
	{
		timesHit += Ball.getDamage ();
		if (timesHit == 1 && isInvisible) {
			setVisible ();
		}
		if (timesHit >= maxHits) {
		    destroyBrick();
        } else {
			loadSprites ();
		}
	}

    void destroyBrick()
    {
			bricksCounter--;
			Destroy (gameObject);
			checkForBonus ();
			levelManager.brickDestroyed ();
    }

	void checkForBonus ()
	{
		Rigidbody2D clone = Instantiate (bonus, transform.position, transform.rotation) as Rigidbody2D;
		clone.velocity = transform.TransformDirection (Vector2.down * 5);
	}

	void setBreakable (bool breakable)
	{
		isBreakable = breakable;
	}

}
