using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	
	public float smoothing;
	
	private Vector2 origin;
	private Vector2 direction;
	private Vector2 smoothDirection;
	private bool touched;
	private int pointerID;
	public Rigidbody2D paddle;
	private float oldPositionX;

	void Awake ()
	{
		oldPositionX = 0.0f;
		direction = Vector2.zero;
		touched = false;
	}
	
	public void OnPointerDown (PointerEventData data)
	{
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			origin = data.position;
		}
	}
	
	public void OnDrag (PointerEventData data)
	{
		if (data.pointerId == pointerID) {
			Vector2 currentPosition = data.position;

			if (currentPosition.x != oldPositionX) {
				if ((int)currentPosition.x < Screen.width / 2) {
					direction = Vector2.left;
				} else {
					direction = Vector2.right;
				}
				oldPositionX = currentPosition.x;
			} else {
				direction = Vector2.zero;
			}
//			Vector2 directionRaw = currentPosition - origin;
//			Vector2 directionRaw = (currentPosition / Screen.width * 16) - paddle.position;
//			direction = directionRaw.normalized;
		}
	}

	public void OnPointerUp (PointerEventData data)
	{
		if (data.pointerId == pointerID) {
			direction = Vector3.zero;
			touched = false;
		}
	}
	
	public Vector2 GetDirection ()
	{
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}
}