using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

	private Paddle paddle;
	private Vector3 paddleToBallVector;
	static private bool hasStarted = false;
	static private int ballCounter = 0;
	static private bool isGlued = true;
	static private bool isSuperBall = false;
	static private int damage = 1;
	private Vector3 originalPosition = new Vector3 ();
	private Vector2 originalSpeed = new Vector2 (2f, 10f);
	public enum BALL_MODE
	{
		NORMAL,
		SUPER}
	;
	public Sprite[] sprites;

	// Use this for initialization
	void Start ()
	{
		if (ballCounter == 0) {
			originalPosition = gameObject.transform.position;
		}
		ballCounter += 1;
		print ("Start " + ballCounter);
		paddle = GameObject.FindObjectOfType<Paddle> ();
		paddleToBallVector = this.transform.position - paddle.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!hasStarted || isGlued) {
			this.transform.position = paddle.transform.position + paddleToBallVector;
			if (Input.GetMouseButtonDown (0)) {
				this.GetComponent<Rigidbody2D> ().velocity = originalSpeed; 
				hasStarted = true;
				isGlued = false;
			}
		
		}
	}

	public void bounceFromThePaddle (float input)
	{
		Vector2 test = originalSpeed;
		test.x *= input;
		this.GetComponent<Rigidbody2D> ().velocity = test;
	}

	public void stickToThePaddle ()
	{
		hasStarted = false;
		isGlued = true;
		paddleToBallVector = this.transform.position - paddle.transform.position;
		Vector3 v = this.GetComponent<Rigidbody2D> ().velocity;
		v.y = 0.0f;
		v.x = 0.0f;
		this.GetComponent<Rigidbody2D> ().velocity = v;
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.collider.tag == "paddle") {
			print ("Glue to pancakes!");
//			stickToThePaddle ();
		}
		if (hasStarted) {
			Vector2 tweak = new Vector2 (Random.Range (0f, 0.2f), Random.Range (0f, 0.2f));
			GetComponent<AudioSource> ().Play ();
			this.GetComponent<Rigidbody2D> ().velocity += tweak;
		}
	}

	public void resetBall ()
	{
		gameObject.transform.position = originalPosition;
		changeBallMode (BALL_MODE.NORMAL);
		stickToThePaddle ();
	}

	static private void setDamage ()
	{
		damage = isSuperBall ? 10 : 1;
	}

	static public int getDamage ()
	{
		return damage;
	}
    
	static public bool superBall ()
	{
		return isSuperBall; 
	}

	static public int getBallCounter ()
	{
		return ballCounter;
	}

	static public void resetBallCounter ()
	{
		ballCounter = 0;
	}


	public void changeBallMode (BALL_MODE mode)
	{
		if (mode == BALL_MODE.SUPER) {
//			Physics2D.IgnoreLayerCollision (gameObject.layer, 10, true);
			isSuperBall = true;
			setDamage ();
			this.GetComponent<SpriteRenderer> ().sprite = sprites [1];
		} else if (mode == BALL_MODE.NORMAL) {
			Physics2D.IgnoreLayerCollision (gameObject.layer, 10, false);
			isSuperBall = false;
			setDamage ();
			this.GetComponent<SpriteRenderer> ().sprite = sprites [0];
		}
	}

	public int destroyBall ()
	{
		ballCounter--;
		print ("destroy " + ballCounter);
		Destroy (gameObject);
		return ballCounter;
	}

	public void speedUp ()
	{
//        transform.Translate(Vector2.right * 2.0f * Time.deltaTime);
//        transform.Translate(Vector2.up * 2.0f * Time.deltaTime);
//		transform.gameObject.vel
		print ("SpeedUP");
//		this.GetComponent<Rigidbody2D> ().velocity += 2.0f;
	}

	public void slowDown ()
	{
//        transform.Translate(Vector2.right * 0.5f * Time.deltaTime);
//        transform.Translate(Vector2.up * 0.5f * Time.deltaTime);
		print ("lowDown");
//		this.GetComponent<Rigidbody2D> ().velocity -= 2.0f;
	}

	public void duplicateBall ()
	{
		Vector2 test = originalSpeed;
		test.x *= -1.0f;
		Rigidbody2D clone = Instantiate (this.GetComponent<Rigidbody2D> (), transform.position, transform.rotation) as Rigidbody2D;
		clone.velocity = test;
		if (clone == null) {
			Debug.LogError ("Nie bangla");
		}
	}
    
}
