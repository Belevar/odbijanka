using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{

	public AudioClip bonusSound;
	private Paddle paddle;
	private Ball ball;

	enum bonusType
	{
		SPEED,
		SLOW,
		HP_UP,
		HP_LOSE,
		LONGER,
		SHORTER,
		GLUE,
		SHOT,
		SHOW_INVISIBLE,
		INDESTRUCTIBLE,
		SUPER_BALL,
		SPLIT_BALL,
		STRANGE_FLY}
	; 

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void showInvisible ()
	{
		var invisible = GameObject.FindGameObjectsWithTag ("invisible");
		foreach (var brick in invisible) {
			brick.GetComponent<Brick> ().setVisible ();
		}
	}

	void activateBonus ()
	{
		Debug.LogError ("NIc sie nie dzieje, z bonusami");
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "paddle") {
			AudioSource.PlayClipAtPoint (bonusSound, transform.position);
			Destroy (gameObject);
			//GameObject.FindObjectOfType<Paddle> ().resizePaddle (Random.Range (-1F, 1F));
			GameObject.FindObjectOfType<Paddle> ().activateShooting ();
			//Ball.changeBallMode (Ball.BALL_MODE.SUPER);
			showInvisible ();
//			GameObject.FindObjectOfType<Ball> ().speedUp (); 
			GameObject.FindObjectOfType<Ball> ().duplicateBall ();
		}
	}
}
