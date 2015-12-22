using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour
{
	private int lives;
	private bool paused = false;
	public int maxLives;
	public Text brickCounterOutput;
	public GameObject pauseMenu;
	public Sprite[] hearts;
	public SpriteRenderer livesSprite;
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
		Debug.LogError ("New Level\n Brick Counter = " + Brick.bricksCounter);
		pauseMenu.SetActive (false);
		brickCounterOutput.text = "x " + Brick.bricksCounter;
		Debug.LogError (brickCounterOutput.text);
		wasGiven = false;
		lives = 3;
		livesSprite.sprite = hearts [lives - 1];

	}

	public bool loseLiveAndCheckEndGame ()
	{
		bool endGame = --lives <= 0;
		print (endGame);
		if (endGame) {
			loadScene ("LoseScreen");
		}
		livesSprite.sprite = hearts [lives - 1];
		brickCounterOutput.text = "X " + Brick.bricksCounter;
		return endGame;
	}

	public void addLive ()
	{
		if (lives < maxLives) {
			++lives;
			livesSprite.sprite = hearts [lives - 1];
		}
	}

	public void updateBallCounter ()
	{
		brickCounterOutput.text = "x " + Brick.bricksCounter;
	}
	
	public int getLives ()
	{
		return lives;
	}

	public void loadNextLevel ()
	{
		Ball.resetBallCounter ();
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void quitRequest ()
	{
		Debug.Log ("Quit request!");
		Application.Quit ();
	}
    
	public void loadScene (string name)
	{
		Ball.resetBallCounter ();

		Brick.bricksCounter = 0;
		Application.LoadLevel (name);
	}

	public bool getIfPaused ()
	{
		return paused;
	}

	public void brickDestroyed ()
	{
		brickCounterOutput.text = "x " + Brick.bricksCounter;
		if (Brick.bricksCounter <= 0) {
			loadNextLevel ();
		}
	}

	public Rigidbody2D getBonus ()
	{
		if (bonuses != null) {
			if (!wasGiven) {
				wasGiven = true;
				return bonuses [0].bonusType;
			} else {
				//return bonuses [1].bonusType;
			}
		}
		return null;
	}

	public void pauseGame ()
	{
		if (paused) {
			Time.timeScale = 1;
			paused = false;
			pauseMenu.SetActive (paused);
		} else {
			paused = true;
			Time.timeScale = 0; 
			pauseMenu.SetActive (paused);
		}
//		Camera.main.backgroundColor.a = 50;
	}

}
