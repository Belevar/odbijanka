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
	static public float maxSpeed;
	static public float minSpeed;
	static private Vector3 originalPosition = new Vector3 ();
	static public Vector2 originalSpeed = new Vector2 (2f, 10f);
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
			isGlued = true;
			hasStarted = false;
		}
		paddle = FindObjectOfType<Paddle> ();
		paddleToBallVector = this.transform.position - paddle.transform.position;
		ballCounter += 1;
		Debug.Log ("Start:" + ballCounter);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!hasStarted || isGlued) {
			float mousePositionInBlocks = Input.mousePosition.y / Screen.height * 16;
			int delta = (int)Mathf.Abs (mousePositionInBlocks - paddle.transform.position.y);
			this.transform.position = paddle.transform.position + paddleToBallVector;
			if (Input.GetMouseButtonDown (0) && delta <= 1) {
				GetComponent<Rigidbody2D> ().velocity = originalSpeed; 
				hasStarted = true;
				isGlued = false;
			}
		
		}
	}

	public void bounceFromThePaddle (float input)
	{
		Vector2 newVelocity = originalSpeed;
		newVelocity.x *= input;
		this.GetComponent<Rigidbody2D> ().velocity = newVelocity;
	}

	public void stickToThePaddle ()
	{
		hasStarted = false;
		isGlued = true;
		paddleToBallVector = transform.position - paddle.transform.position;
		Vector3 v = GetComponent<Rigidbody2D> ().velocity;
		v.y = 0.0f;
		v.x = 0.0f;
		GetComponent<Rigidbody2D> ().velocity = v;
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.collider.tag == "paddle") {
			if (isGlued) {
				stickToThePaddle ();
			} else {

			}
		}
		if (hasStarted) {
			GetComponent<AudioSource> ().Play ();
		}
	}

	public void resetBall ()
	{
		gameObject.transform.position = originalPosition;
//		changeBallMode (BALL_MODE.NORMAL);
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
		Debug.Log ("DestroyBall:" + ballCounter);
		if (ballCounter > 0)
			Destroy (gameObject);
		return ballCounter;
	}

	public void speedUp ()
	{
		if (originalSpeed.y < maxSpeed) {
			originalSpeed.y += 10;
			gameObject.GetComponent<Rigidbody2D> ().velocity = originalSpeed;
		}
	}

	public void slowDown ()
	{
		if (originalSpeed.y > maxSpeed) {
			originalSpeed.y -= 10;
			GetComponent<Rigidbody2D> ().velocity = originalSpeed;
		}
	}

	public void duplicateBall ()
	{
		Vector2 test = originalSpeed;
		test.x *= -1.0f;
		Rigidbody2D clone = Instantiate (this.GetComponent<Rigidbody2D> (), transform.position, transform.rotation) as Rigidbody2D;
		clone.velocity = test;
		if (clone == null) {
			Debug.LogError ("Duplicate Ball::Nie bangla");
		}
	}
    
}
