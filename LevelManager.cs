using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	private int lives;
	private bool paused = false;
	private List<GameObject> bonusesList;
	bool fingerOfGod = false;
    System.Random random; 
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
		random = new System.Random ();
		if (brickCounterOutput != null) {
			brickCounterOutput.text = "x " + Brick.bricksCounter;
			pauseMenu.SetActive (false);
			Debug.LogError (brickCounterOutput.text);
		}
		lives = PlayerPrefsManager.getHealthPoint ();
		if (livesSprite != null) {
			livesSprite.sprite = hearts [lives - 1];
		}
		bonusesList = BonusTranslator.convertBonusesToList (bonuses);
	}

	public bool loseLiveAndCheckEndGame ()
	{
		bool endGame = --lives <= 0;
		PlayerPrefsManager.setHealthPoints (lives);
		FindObjectOfType<BonusManager> ().disactivateAllBonuses ();
        FindObjectOfType<BonusManager>().resetAllBonuses() ;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        SceneManager.LoadScene("level_1");
	}

	IEnumerator Example ()
	{
		print (Time.time);
		yield return new WaitForSeconds (5);
		print (Time.time);
	}
	
	public void loadScene (string name)
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		SceneManager.LoadScene (name);


	}

	public void loadScene ()
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
        if (PlayerPrefsManager.getHealthPoint() > 0)
        {
            SceneManager.LoadScene("level_" + PlayerPrefsManager.getLevel());
        }
	}


	public bool gameIsPaused ()
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
			if (random.Next (0, Brick.bricksCounter + 1) <= bonusesList.Count) {
				Rigidbody2D bonus = bonusesList.Last ().GetComponent<Rigidbody2D> ();
				bonusesList.RemoveAt (bonusesList.Count - 1);
				Debug.Log (bonusesList.Count);
				return bonus;
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
	}

    public void activateFingerOfGod()
    {
        fingerOfGod = true;
    }

    public void disactivateFingerOfGod()
    {
        fingerOfGod = false;
    }

    public bool isFingerOfGodActive()
    {
        return fingerOfGod;
    }
}
