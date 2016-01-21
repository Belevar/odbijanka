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
	public float maxSpeed;
	public float minSpeed;
	static private Vector3 originalPosition = new Vector3 ();
	static public Vector2 originalSpeed = new Vector2 (2f, 13f);
	float actualSpeed = 13f;
	public GameObject fingerOfTheGod;
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
//		stickToThePaddle ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {
			Vector3 fingerPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Instantiate (fingerOfTheGod, fingerPos, Quaternion.identity);

		}
		if (!hasStarted) {
			if (isGlued) {
				Vector3 fingerPos = Input.mousePosition;
				fingerPos = Camera.main.ScreenToWorldPoint (fingerPos);
				float mousePositionInBlocksY = Input.mousePosition.y / Screen.height * 16;
				float mousePositionInBlocksX = Input.mousePosition.x / Screen.height * 9;
				int delta = (int)Mathf.Abs (mousePositionInBlocksY - paddle.transform.position.y);
				transform.position = paddle.transform.position + paddleToBallVector;

				if (Input.GetMouseButtonUp (0) && delta <= 1) { //to chyba musi być w paddle(wystrzeliwanie piłek)
					//i jest to logiczne. Trzeba użyć bounceFromPaddle dla każdej piłki znalezionej i przyklejonej;
					startFromThePaddle (paddleToBallVector.x);
					hasStarted = true;
					isGlued = false;
				}
			}
		} else {
//			TODO cos z boringLoop trze zrobic
//			breakBoringLoop ();
		}
	}

	void breakBoringLoop ()
	{
		Vector2 temp = GetComponent<Rigidbody2D> ().velocity;
		if (temp.y < 1.0f && temp.y > 0f) { //To raczej nie powinno tak być
			Debug.LogError ("Boring Loop? Break IT!");
			temp.y *= 2.0f;
			GetComponent<Rigidbody2D> ().velocity = temp;
		} else if (temp.x < 1.0f && temp.x > 0f) { //To raczej nie powinno tak być
			Debug.LogError ("Boring Loop? Break IT!");
			temp.x *= 2.0f;
			GetComponent<Rigidbody2D> ().velocity = temp;
		}
	}

	public void startFromThePaddle (float input)
	{
		Vector2 newVelocity = new Vector2 (input * 2, actualSpeed);
		GetComponent<Rigidbody2D> ().velocity = newVelocity;
	}

	public void bounceFromThePaddle (float input)
	{
		Vector2 newVelocity = originalSpeed;
		newVelocity.x *= input;
		GetComponent<Rigidbody2D> ().velocity = newVelocity;
	}

	public void stickToThePaddle ()
	{
		hasStarted = false;
		isGlued = true;
		paddleToBallVector = transform.position - paddle.transform.position;
		GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.collider.tag == "paddle") {
			if (isGlued) {
				stickToThePaddle ();
			}
		}
		if (hasStarted) {
			GetComponent<AudioSource> ().volume = FindObjectOfType<MusicPlayer> ().getVolume ();
			GetComponent<AudioSource> ().Play ();
		}
	}

	public void resetBall ()
	{
		gameObject.transform.position = originalPosition;
		changeBallMode (BALL_MODE.NORMAL);
		stickToThePaddle ();
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
			GetComponent<SpriteRenderer> ().sprite = sprites [1];
		} else if (mode == BALL_MODE.NORMAL) {
			isSuperBall = false;
			GetComponent<SpriteRenderer> ().sprite = sprites [0];
			GameObject[] bricksInGame = GameObject.FindGameObjectsWithTag ("breakable");
			foreach (GameObject brick in bricksInGame) {
				brick.GetComponent<PolygonCollider2D> ().isTrigger = false;
			}
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
			originalSpeed.y += 5;
			gameObject.GetComponent<Rigidbody2D> ().velocity = originalSpeed;
		}
	}

	public void slowDown ()
	{
		if (originalSpeed.y > maxSpeed) {
			originalSpeed.y -= 5;
			GetComponent<Rigidbody2D> ().velocity = originalSpeed;
		}
	}

	public void duplicateBall ()
	{
		Vector2 duplicateBallSpeed = originalSpeed;
		duplicateBallSpeed.x *= -1.0f;
		Rigidbody2D clone = Instantiate (GetComponent<Rigidbody2D> (), transform.position, transform.rotation) as Rigidbody2D;
		clone.velocity = duplicateBallSpeed;
		if (clone == null) {
			Debug.LogError ("Duplicate Ball::Nie bangla");
		}
	}
    
}
