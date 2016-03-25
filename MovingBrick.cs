using UnityEngine;
using System.Collections;

public class MovingBrick : MonoBehaviour
{

	// Use this for initialization
	float speed = 3.0f;
	bool movingRight = true;
	void Start ()
	{
		GetComponent<Rigidbody2D> ().velocity = Vector2.right * speed;
	}
	
	void OnCollisionEnter2D (Collision2D collision)
	{
		Debug.LogError ("aaa");

		if (movingRight) {
			GetComponent<Rigidbody2D> ().velocity = Vector2.left * speed;
			movingRight = false;
		} else {
			GetComponent<Rigidbody2D> ().velocity = Vector2.right * speed;
			movingRight = true;
		}
	}
}
