using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
	private int lives;

	//temp variables
	bool wasGiven;

	[Serializable]
	public struct Bonuses
	{
		public int quantity;
		public Rigidbody2D bonusType;
	}
	public Bonuses[] bonuses;

	void Start ()
	{
		wasGiven = false;
		lives = 3;
	}

	public bool loseLiveAndCheckEndGame ()
	{
		bool endGame = --lives <= 0;
		print (endGame);
		if (endGame) {
			loadScene ("LoseScreen");
		}
		return endGame;
	}

	public void addLive ()
	{
		++lives;
	}

	public void loadNextLevel ()
	{
		Ball.resetBallCounter ();
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void quitRequest ()
	{
		Debug.Log ("Quit request!");
	}
    
	public void loadScene (string name)
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		Application.LoadLevel (name);
	}

	public void brickDestroyed ()
	{
		if (Brick.bricksCounter <= 0) {
			loadNextLevel ();
		}
	}

	public Rigidbody2D getBonus ()
	{
		if (bonuses.Length != 0) {
			if (!wasGiven) {
				wasGiven = true;
				return bonuses [0].bonusType;
			} else {
				return bonuses [1].bonusType;
			}
		}
		return null;
	}
}
