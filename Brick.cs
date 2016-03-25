using UnityEngine;
using System.Collections;

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

	private int clipIndex;
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
	


	void Update ()
	{
//		if (isMoveable) {
//			if (movingRight) {
//				transform.position += Vector3.right * speed * Time.deltaTime;
//			} else {
//				transform.position += Vector3.left * speed * Time.deltaTime;
//			}
//		
//			float leftEdgeOfFormation = transform.position.x - (0.5f * width);
//			float rightEdgeOfFormation = transform.position.x + (0.5f * width);
//			if (leftEdgeOfFormation < xMin) {
//				movingRight = true;
//			} else if (rightEdgeOfFormation > xMax) {
//				movingRight = false;
//			}
//		}
	}

	// Use this for initialization
	void Start ()
	{
        areIndestructible = false;
		levelManager = FindObjectOfType<LevelManager> ();
		isInvisible = this.tag == "invisible";
		setBreakable (this.tag == "breakable" || isInvisible);
		if (isMoveable) {
//			float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
//			Vector3 leftEdge = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
//			Vector3 rightEdge = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
//			xMax = rightEdge.x;
//			xMin = leftEdge.x;
//			GetComponent<Rigidbody2D> ().velocity = Vector2.right * speed;
		}
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
		Debug.LogError ("Collisoin");
		if (collision.gameObject.tag == "ball" || collision.gameObject.tag == "missle") {
			if (isBreakable) {
				AudioSource.PlayClipAtPoint (destroySound, transform.position, FindObjectOfType<MusicPlayer> ().getVolume ());
				handleHits ();
			}
		} else {
//			if (movingRight) {
//				GetComponent<Rigidbody2D> ().velocity = Vector2.left * speed;
//				movingRight = false;
//			} else {
//				GetComponent<Rigidbody2D> ().velocity = Vector2.right * speed;
//				movingRight = true;
//			}
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
        if (Ball.superBall() || FindObjectOfType<Paddle>().isShooting())
        {
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

    void OnMouseDown()
    {
        if (!levelManager.gameIsPaused() && isBreakable && levelManager.isFingerOfGodActive()) 
        {
            destroyBrick();
        }
       
    }

}
// void setBreakable (bool gac)}
//{  
//	bonus Gac = null }
//:3
