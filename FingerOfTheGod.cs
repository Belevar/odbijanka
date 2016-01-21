using UnityEngine;
using System.Collections;

public class FingerOfTheGod : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		Debug.LogError("Trigger");
		Destroy(gameObject);
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		Debug.LogError("Kolizja");
//		Brick brick = collision as Brick;
//		brick.handleHits();
//		Destroy(gameObject);
	}
}
