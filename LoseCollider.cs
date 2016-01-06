using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour
{
	private LevelManager levelManager;

	void Start ()
	{
		levelManager = FindObjectOfType<LevelManager> ();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.gameObject.tag == "ball") {
			if (Ball.getBallCounter () > 1) {
				print ("Wiekszy");
				trigger.gameObject.GetComponent<Ball> ().destroyBall ();
			} else {
				if (!levelManager.loseLiveAndCheckEndGame ()) {
					FindObjectOfType<Paddle> ().resetPaddle ();
					FindObjectOfType<Ball> ().resetBall ();
				}
			}
		} else {
			Destroy (trigger.gameObject);
		}
	}
}