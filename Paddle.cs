using UnityEngine;

public class Paddle : MonoBehaviour
{
	public bool autoPlay = false;
	public Rigidbody2D missle;
	private LevelManager levelManger;
	private Ball ball;
	Vector3 originalPosition;
	Vector3 originalSize;
	float sizeInX;
	float oldMouseX;
    float[] paddleSizes = { 0.5f, 0.8f, 1.1f, 1.4f, 1.7f };
    int indexOfCurrentSize = 2;
    bool shooting;

	// Use this for initialization
	void Start ()
	{
        shooting = false;
		ball = FindObjectOfType<Ball> ();
		originalPosition = gameObject.transform.position;
		oldMouseX = originalPosition.x;
		originalSize = gameObject.transform.localScale;
		sizeInX = gameObject.GetComponent<BoxCollider2D> ().bounds.size.x;
        levelManger= FindObjectOfType<LevelManager>();
		if (gameObject.GetComponent<Rigidbody2D> () == null) {
			Debug.LogError ("PADDLE::NI MA RIGIDBODY");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!levelManger.gameIsPaused ()) {
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
		paddlePos.x = Mathf.Clamp (ballPos.x, sizeInX / 2f - 0.1f, 16f - (sizeInX / 2f) - 0.1f);
		this.transform.position = paddlePos;
	}

	public void activateShooting ()
	{
		InvokeRepeating ("shot", 0.00001f, 1f);
        shooting = true;
	}

	public void disactivateShooting ()
	{
		CancelInvoke ("shot");
        shooting = false;
	}

    public bool isShooting()
    {
        return shooting;
    }

	public void resetPaddle ()
	{
		gameObject.transform.position = originalPosition;
		gameObject.transform.localScale = originalSize;
		disactivateShooting ();
        indexOfCurrentSize = 2;
        sizeInX = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
	}

	public Vector3 getPosition ()
	{
		return originalPosition;
	}

	public void makeShorter ()
	{
        if (indexOfCurrentSize > 0)
        {
            --indexOfCurrentSize;
            Vector2 newScale = transform.localScale;
            newScale.x = paddleSizes[indexOfCurrentSize];
            transform.localScale = newScale;
            sizeInX = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
        }
	}

    public void makeLonger()
    {
        if (indexOfCurrentSize < paddleSizes.Length - 1)
        {
            ++indexOfCurrentSize;
            Vector2 newScale = transform.localScale;
            newScale.x = paddleSizes[indexOfCurrentSize];
            transform.localScale = newScale;
            sizeInX = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
        }
    }

	void shot ()
	{
        float halfPaddleLenght = gameObject.GetComponent<BoxCollider2D>().bounds.size.x / 2f;
		Vector2 misslePos = transform.position;
		misslePos.y += 0.5f;
		Vector2 missle2Pos = misslePos;
		misslePos.x += halfPaddleLenght - 0.25f;
		missle2Pos.x -= halfPaddleLenght - 0.25f;
		missle2Pos.y -= 0.05f;
		Rigidbody2D clone = Instantiate (missle, misslePos, transform.rotation) as Rigidbody2D;
		Rigidbody2D clone1 = Instantiate (missle, missle2Pos, transform.rotation) as Rigidbody2D;
		clone1.velocity = clone.velocity = transform.TransformDirection (Vector2.down * 5);
	}
}
