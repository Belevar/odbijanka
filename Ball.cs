using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	private Paddle paddle;
	private Vector3 paddleToBallVector;
    private LevelManager levelManager;
    const int speedChangeCounter = 2;
	
    static private bool hasStarted = false;
	static private int ballCounter = 0;
	static private bool isGlued = true;
	static private bool isSuperBall = false;
    static private int damage = 1;
    static private int acctualSpeedChange = 0;
    static private Vector3 originalPosition = new Vector3();

    public Sprite[] sprites;
	static public Vector2 currentSpeed = new Vector2 (2f, 13f);

    public enum BALL_MODE
    {
        NORMAL,
        SUPER
    };

	// Use this for initialization
	void Start ()
	{
        levelManager = FindObjectOfType<LevelManager>();
		if (ballCounter == 0) {
			originalPosition = gameObject.transform.position;
			isGlued = true;
			hasStarted = false;
		}
		paddle = FindObjectOfType<Paddle> ();
		paddleToBallVector = this.transform.position - paddle.transform.position;
		ballCounter += 1;
//		stickToThePaddle ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!hasStarted) {
			if (isGlued && !levelManager.gameIsPaused()) {
                int distanceBetweenBallandPaddle = moveBallWithPaddle(); //that int only for multiple balls 

                if (Input.GetMouseButtonUp(0) && distanceBetweenBallandPaddle <= 1)
                { 
					startFromThePaddle (paddleToBallVector.x);
				}
			}
		} else {
//			TODO cos z boringLoop trze zrobic
//			breakBoringLoop ();
		}
	}

    int moveBallWithPaddle()
    {
		Vector3 fingerPos = Input.mousePosition;
		fingerPos = Camera.main.ScreenToWorldPoint (fingerPos);
		float mousePositionInBlocksY = Input.mousePosition.y / Screen.height * 16;
		float mousePositionInBlocksX = Input.mousePosition.x / Screen.height * 9;
		int delta = (int)Mathf.Abs (mousePositionInBlocksY - paddle.transform.position.y);
		transform.position = paddle.transform.position + paddleToBallVector;
        return delta;
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
		Vector2 newVelocity = new Vector2 (input * 2, currentSpeed.y);
		GetComponent<Rigidbody2D> ().velocity = newVelocity;
        hasStarted = true;
		isGlued = false;
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
            else
            {
                bounceFromPaddle(collision.gameObject.transform.position.x);
            }
		}
		if (hasStarted) {
			GetComponent<AudioSource> ().volume = FindObjectOfType<MusicPlayer> ().getVolume ();
			GetComponent<AudioSource> ().Play ();
		}
	}

    void bounceFromPaddle(float posXofPaddle)
    {
         float magnitude = GetComponent<Rigidbody2D>().velocity.magnitude;
         float distanceToMiddle = this.transform.position.x - posXofPaddle;
         GetComponent<Rigidbody2D>().velocity = new Vector2(distanceToMiddle, 1f).normalized * magnitude; 
    }

	public void resetBall ()
	{
		gameObject.transform.position = originalPosition;
		changeBallMode (BALL_MODE.NORMAL);
		stickToThePaddle ();
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
        if (acctualSpeedChange < speedChangeCounter)
        {
            ++acctualSpeedChange;
            Vector2 speed = GetComponent<Rigidbody2D>().velocity;
           // speed *= 0.25f;
            speed *= 1.25f;
          //  GetComponent<Rigidbody2D>().AddForce(speed, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().velocity = speed;
        }
	}

    public void slowDown()
    {
        if (acctualSpeedChange > -speedChangeCounter)
        {
            --acctualSpeedChange;
            Vector2 speed = GetComponent<Rigidbody2D>().velocity;
            //speed *= 0.75f;
            speed *= 0.75f;
            GetComponent<Rigidbody2D>().velocity = speed;
        }
    }

	public void duplicateBall ()
	{
		Vector2 duplicateBallSpeed = currentSpeed;
		duplicateBallSpeed.x *= -1.0f;
		Rigidbody2D clone = Instantiate (GetComponent<Rigidbody2D> (), transform.position, transform.rotation) as Rigidbody2D;
		clone.velocity = duplicateBallSpeed;
		if (clone == null) {
			Debug.LogError ("Duplicate Ball::Nie bangla");
		}
	}
    
}
