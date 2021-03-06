﻿using UnityEngine;
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
				trigger.gameObject.GetComponent<Ball> ().destroyBall ();
			} else {
				levelManager.loseLiveAndCheckEndGame ();
			}
		} else {
			Destroy (trigger.gameObject);
		}
	}
}