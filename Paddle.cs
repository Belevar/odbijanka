using UnityEngine;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}


public class Paddle : MonoBehaviour
{
	public SimpleTouchPad touchPad;
	public bool autoPlay = false;
	public Rigidbody2D missle;
	private Ball ball;
	private bool canShoot;
	Vector3 originalPosition;
	Vector3 originalSize;
	bool wasReduced = false;
	bool wasEnlarged = false;
	public float speed;
	public Boundary boundary;
	private Rigidbody2D rigidbody;
	int oldMouseX;


	// Use this for initialization
	void Start ()
	{
		oldMouseX = 0;
		canShoot = false;
		ball = GameObject.FindObjectOfType<Ball> ();
		originalPosition = gameObject.transform.position;
		originalSize = gameObject.transform.localScale;
		rigidbody = gameObject.GetComponent<Rigidbody2D> ();
		if (rigidbody == null) {
			Debug.LogError ("NAAAAAAAAAAAAAAAAa");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!autoPlay) {
			moveWithMouse ();
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (canShoot) {
					shot ();
				}
			}

		} else {
			useAutoPlay ();
			Debug.LogError ("AUTO PLAY");
		}
	}

	//if palec != paddlePos +- halfSize
	void moveWithMouse ()
	{
		float mousePositionInBlocks = Input.mousePosition.x / Screen.width * 16;
		int deska = (int)this.transform.position.x;
		int mysz = (int)mousePositionInBlocks;
		if (mysz != deska) {
			Vector3 paddlePos;
			Vector2 velocity = Vector2.right * speed;
			if (deska < mysz) {
				paddlePos = this.GetComponent<Rigidbody2D> ().position + velocity * Time.deltaTime;
			} else {
				paddlePos = this.GetComponent<Rigidbody2D> ().position + velocity * Time.deltaTime * (-1);
			}
			paddlePos.x = Mathf.Clamp (paddlePos.x, 0.5f, 15.5f);
			this.transform.position = paddlePos;
		}
	}

	void useAutoPlay ()
	{
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp (ballPos.x, 0.5f, 15.5f);
		this.transform.position = paddlePos;
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		foreach (ContactPoint2D placeOnPaddle in collision.contacts) {
			//Dostosować do różnych wielkości deski 
			float bounceAngle = (this.GetComponent<Collider2D> ().transform.position.x - placeOnPaddle.point.x) * 1.7f;
			ball.bounceFromThePaddle (-bounceAngle);
		}
	}

	public void activateShooting ()
	{
		canShoot = true;
	}

	public void disactivateShooting ()
	{
		canShoot = false;
	}

	public void resetPaddle ()
	{
		gameObject.transform.position = originalPosition;
		gameObject.transform.localScale = originalSize;
		disactivateShooting ();
		wasReduced = wasEnlarged = false;
	}

	public Vector3 getPosition ()
	{
		return originalPosition;
	}

	public void resizePaddle (float scale)
	{
		if (!wasReduced && scale < 0) {
			wasReduced = true;
			wasEnlarged = false;
			transform.localScale += new Vector3 (scale, 0, 0);
		} else if (!wasEnlarged && scale > 0) {
			wasReduced = false;
			wasEnlarged = true;
			transform.localScale += new Vector3 (scale, 0, 0);
		} else {
			Debug.LogError ("To nie powinno sie nigdy wyświetlić");
		}
	}

	void shot ()
	{
		Vector3 test = transform.position;
		test.y += 0.5f;
		Vector3 test2 = test;
		test.x += 0.8f;
		test2.x -= 0.8f;
		Rigidbody2D clone = Instantiate (missle, test, transform.rotation) as Rigidbody2D;
		Rigidbody2D clone1 = Instantiate (missle, test2, transform.rotation) as Rigidbody2D;
		clone1.velocity = clone.velocity = transform.TransformDirection (Vector2.down * 5);
	}
}
