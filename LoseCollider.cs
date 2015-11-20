using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour
{
	private LevelManager levelManager;
	private Paddle paddle;
	private Ball ball;

	void Start ()
	{
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "ball") {
			if (Ball.getBallCounter () > 1) {
				print ("Wiekszy");
				trigger.gameObject.GetComponent<Ball> ().destroyBall ();
			}
			if (levelManager.loseLiveAndCheckEndGame ()) {
				levelManager.loadScene ("LoseScreen");
				Brick.bricksCounter = 0;
			} else {
				GameObject.FindObjectOfType<Paddle> ().resetPaddle ();
				GameObject.FindObjectOfType<Ball> ().resetBall ();
				Destroy (trigger.gameObject);
			}
		}
	}
}