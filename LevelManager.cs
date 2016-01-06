using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour
{
	private int lives;
	private bool paused = false;
	private List<GameObject> bonusesList;
	public int maxLives;
	public Text brickCounterOutput;
	public GameObject pauseMenu;
	public Sprite[] hearts;
	public SpriteRenderer livesSprite;

	[Serializable]
	public struct Bonuses
	{
		public int quantity;
		public GameObject bonusType;
	}
	public Bonuses[] bonuses;

	void Start ()
	{
		pauseMenu.SetActive (false);
		brickCounterOutput.text = "x " + Brick.bricksCounter;
		Debug.LogError (brickCounterOutput.text);
		lives = PlayerPrefsManager.getHealthPoint ();
		livesSprite.sprite = hearts [lives - 1];
		bonusesList = BonusTranslator.convertBonusesToList (bonuses);
	}

	public bool loseLiveAndCheckEndGame ()
	{
		bool endGame = --lives <= 0;
		PlayerPrefsManager.setHealthPoints (lives);
		if (endGame) {
			loadScene ("LoseScreen");
		}
		livesSprite.sprite = hearts [lives - 1];
		return endGame;
	}

	public void addLive ()
	{
		if (lives < maxLives) {
			++lives;
			PlayerPrefsManager.setHealthPoints (lives);
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
		Brick.bricksCounter = 0;
		PlayerPrefsManager.setLevel (PlayerPrefsManager.getLevel () + 1);
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void quitRequest ()
	{
		Application.Quit ();
	}

	public void startNewGame ()
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		PlayerPrefsManager.setHealthPoints (3);
		PlayerPrefsManager.setLevel (1);
		Application.LoadLevel ("level_1");
	}
    
	public void loadScene (string name)
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		Application.LoadLevel (name);
	}

	public void loadScene ()
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		Application.LoadLevel ("level_" + PlayerPrefsManager.getLevel ());
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
		if (bonusesList.Count > 0) {
			Rigidbody2D bonus = bonusesList.Last ().GetComponent<Rigidbody2D> ();
			bonusesList.RemoveAt (bonusesList.Count - 1);
			Debug.Log (bonusesList.Count);
			return bonus;
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
	}
}
