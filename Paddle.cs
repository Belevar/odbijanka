using UnityEngine;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax;
}


public class Paddle : MonoBehaviour
{
	public bool autoPlay = false;
	public Rigidbody2D missle;
	public LevelManager levelManger;
	private Ball ball;
	private bool canShoot;
	Vector3 originalPosition;
	Vector3 originalSize;
	float sizeInX;
	bool wasReduced = false;
	bool wasEnlarged = false;
	public Boundary boundary;
	float oldMouseX;

	// Use this for initialization
	void Start ()
	{
		canShoot = false;
		ball = FindObjectOfType<Ball> ();
		originalPosition = gameObject.transform.position;
		oldMouseX = originalPosition.x;
		originalSize = gameObject.transform.localScale;
		sizeInX = gameObject.GetComponent<BoxCollider2D> ().bounds.size.x;
		if (gameObject.GetComponent<Rigidbody2D> () == null) {
			Debug.LogError ("PADDLE::NI MA RIGIDBODY");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!levelManger.getIfPaused ()) {
			if (!autoPlay) {
				moveWithMouse ();
			} else {
				useAutoPlay ();
			}
		}
	}

	void moveWithMouse ()
	{

		float mousePositionInBlocks = Input.mousePosition.x / Screen.width * 16;
		int delta = (int)Mathf.Abs (mousePositionInBlocks - oldMouseX);
		if (delta <= 1) {
			Vector3 paddlePos = this.transform.position;
			paddlePos.x = Mathf.Clamp (mousePositionInBlocks, sizeInX / 2f - 0.1f, 16f - (sizeInX / 2f) - 0.1f);
			oldMouseX = mousePositionInBlocks;
			this.transform.position = paddlePos;
		}
	}

	void useAutoPlay ()
	{
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp (ballPos.x, boundary.xMin, boundary.xMax);
		this.transform.position = paddlePos;
	}
	//to musi się odbijać piłka w piłce nie w Paddle. Dlatego odbijają się dwie na raz, bo ma dostęp do jednej, a druga robi kolizje.
	//wtedy pierwsza dostaje odbicie z tej funkcji, a druga z unity(bledne bo zgodne z katem padania).
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
		InvokeRepeating ("shot", 0.00001f, 1f);
	}

	public void disactivateShooting ()
	{
		canShoot = false;
		CancelInvoke ("shot");
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
			sizeInX = gameObject.GetComponent<BoxCollider2D> ().bounds.size.x;
		} else if (!wasEnlarged && scale > 0) {
			wasReduced = false;
			wasEnlarged = true;
			transform.localScale += new Vector3 (scale, 0, 0);
			sizeInX = gameObject.GetComponent<BoxCollider2D> ().bounds.size.x;
		} else {
			Debug.LogError ("resizePaddle::To nie powinno sie nigdy wyświetlić");
		}
	}

	void shot ()
	{
		Vector3 misslePos = transform.position;
		misslePos.y += 0.5f;
		Vector3 missle2Pos = misslePos;
		misslePos.x += 0.8f;
		missle2Pos.x -= 0.8f;
		missle2Pos.y -= 0.05f;
		Rigidbody2D clone = Instantiate (missle, misslePos, transform.rotation) as Rigidbody2D;
		Rigidbody2D clone1 = Instantiate (missle, missle2Pos, transform.rotation) as Rigidbody2D;
		clone1.velocity = clone.velocity = transform.TransformDirection (Vector2.down * 5);
	}
}
