using UnityEngine;
using System.Collections;

public class Missle : MonoBehaviour
{

	void Start ()
	{
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 10f);
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		Destroy (gameObject);
	}
}
