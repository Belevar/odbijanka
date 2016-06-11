using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
	private AdsvertMaker adds;


	//SAVE SHIT
	public SaveObjectsList savedGame;

	public delegate void SaveDelegate (object sender, EventArgs args);

	public static event SaveDelegate SaveEvent;

	public Brick invisibleBrick;
	public Brick oneHitBrick;
	public Brick twoHitBrick;
	public Brick threeHitBrick;
	public Brick indestructibleBrick;


	[Serializable]
	public struct Bonuses
	{
		public int quantity;
		public GameObject bonusType;
	}

	public Bonuses[] bonuses;

	void Awake ()
	{
		Debug.Log (SceneManager.GetActiveScene ().name);
		Debug.Log (PlayerPrefsManager.isGameLoaded ());
		if (SceneManager.GetActiveScene ().name == "start" && PlayerPrefsManager.isGameLoaded () == 0) {
			GameObject.Find ("resume game").GetComponent<Button> ().interactable = false;
		}
	}

	void Start ()
	{
		savedGame = new SaveObjectsList ();
		random = new System.Random ();


		if (brickCounterOutput != null) {
			brickCounterOutput.text = "x " + Brick.bricksCounter;
			pauseMenu.SetActive (false);
			Debug.LogError (brickCounterOutput.text);
			adds = GetComponent<AdsvertMaker> ();
		}
		lives = PlayerPrefsManager.getHealthPoint ();
		if (livesSprite != null) {
			livesSprite.sprite = hearts [lives - 1];
		}
		if (PlayerPrefsManager.isGameLoaded () == 1 && SceneManager.GetActiveScene ().buildIndex > 4) {
			loadGame ();  
		}
		bonusesList = BonusTranslator.convertBonusesToList (bonuses);
	}

	void loadGame ()
	{
		Brick[] bricksInGame = GameObject.FindObjectsOfType<Brick> ();
		foreach (Brick brick in bricksInGame) {
			brick.destroyBrickOnLoad ();
		}
		Brick.bricksCounter = 0;
		LoadData ();
		for (int i = 0; i < savedGame.bonusesLeft.Count; ++i) {
			print ("Bonus numer " + i);
			print (" ilosc=" + savedGame.bonusesLeft [i]);
			bonuses [i].quantity = savedGame.bonusesLeft [i];
		}
		loadBricks ();
	}

	public bool loseLiveAndCheckEndGame ()
	{
		bool zeroLives = --lives <= 0;
		PlayerPrefsManager.setHealthPoints (lives);
		FindObjectOfType<BonusManager> ().disactivateAllBonuses ();
		FindObjectOfType<BonusManager> ().resetAllBonuses ();
		cleanSceneAfterDeath ();
		if (zeroLives) {
			showAdd ();
			//checkEndGame ();
		} else {
			livesSprite.sprite = hearts [lives - 1];
		}
		return zeroLives;
	}

	private void showAdd ()
	{
		Debug.Log ("Przed reklamą");
		adds.ShowRewardedAd ();
		Debug.Log ("PO reklamie");
	}

	public void checkEndGame ()
	{
		if (lives <= 0) {
			PlayerPrefsManager.setGameLoaded (0);
			loadScene ("LoseScreen");
		}
	}

	public void cleanSceneAfterDeath ()
	{
		FindObjectOfType<Paddle> ().resetPaddle ();
		FindObjectOfType<Ball> ().resetBall ();
		Bonus[] bonusesToDestroy = GameObject.FindObjectsOfType<Bonus> ();
		TimeBonus[] timeBonusesToDestroy = GameObject.FindObjectsOfType<TimeBonus> ();
		Missle[] misslesToDestroy = GameObject.FindObjectsOfType<Missle> ();
		for (int i = 0; i < bonusesToDestroy.Length; ++i) {
			Destroy (bonusesToDestroy [i].gameObject);
		}
		for (int i = 0; i < misslesToDestroy.Length; ++i) {
			Destroy (misslesToDestroy [i].gameObject);
		}
		for (int i = 0; i < timeBonusesToDestroy.Length; ++i) {
			Destroy (timeBonusesToDestroy [i].gameObject);
		}
	}

	public void addLive ()
	{
		Debug.Log ("ADD LIVE");
		if (lives < maxLives) {
			++lives;
			PlayerPrefsManager.setHealthPoints (lives);
			livesSprite.sprite = hearts [lives - 1];
		}
	}

	public void updateBrickCounter ()
	{
		brickCounterOutput.text = "x " + Brick.bricksCounter;
	}

	public int getLives ()
	{
		return lives;
	}

	public void loadNextLevel ()
	{
		Debug.Log ("loadNEXTLevel start");
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		if (SceneManager.GetActiveScene ().buildIndex + 1 < SceneManager.sceneCountInBuildSettings) {
			Debug.Log ("loadNEXTLevel IF current index" + SceneManager.GetActiveScene ().buildIndex);
			Debug.Log ("loadNEXTLevel IF count scenes" + SceneManager.sceneCountInBuildSettings);
			PlayerPrefsManager.setLevel (PlayerPrefsManager.getLevel () + 1);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);			
		} else {
			SceneManager.LoadScene ("win");
		}
	}

	public void quitRequest ()
	{
		Application.Quit ();
	}

	public void startNewGame ()
	{
		PlayerPrefsManager.setGameLoaded (0);
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		PlayerPrefsManager.setHealthPoints (3);
		PlayerPrefsManager.setLevel (1);
		SceneManager.LoadScene ("level_1");
	}

	public void loadScene (string name)
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		SceneManager.LoadScene (name);
	}

	public void backToMenu ()
	{
		SaveData ();
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		PlayerPrefsManager.setGameLoaded (1);
		SceneManager.LoadScene ("start");
	}

	public void loadScene ()
	{
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		if (PlayerPrefsManager.getHealthPoint () > 0) {
			SceneManager.LoadScene ("level_" + PlayerPrefsManager.getLevel ());
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
			PlayerPrefsManager.setGameLoaded (0);
			loadNextLevel ();
		}
	}

	public Rigidbody2D getBonus ()
	{
		if (bonusesList.Count > 0) {
			if (random.Next (0, Brick.bricksCounter + 1) <= bonusesList.Count) {
				Rigidbody2D bonus = bonusesList.Last ().GetComponent<Rigidbody2D> ();
				for (int i = 0; i < bonuses.Length; ++i) {
					if (bonuses [i].bonusType.name == bonus.name) {
						--bonuses [i].quantity;
					}
				}
				bonusesList.RemoveAt (bonusesList.Count - 1);
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

	public void activateFingerOfGod ()
	{
		fingerOfGod = true;
	}

	public void disactivateFingerOfGod ()
	{
		fingerOfGod = false;
	}

	public bool isFingerOfGodActive ()
	{
		return fingerOfGod;
	}

	void loadBricks ()
	{
		PlayerPrefsManager.setGameLoaded (0);
		brickCounterOutput.text = "x " + 0;
		if (savedGame.savedBricks != null) {
			foreach (SaveBrick brick in savedGame.savedBricks) {
				initializeBrick (brick);
			}
		}
		updateBrickCounter ();
	}

	void initializeBrick (SaveBrick brick)
	{
		Brick newBrick;
		if (brick.tag == "invisible") {
			newBrick = Instantiate (invisibleBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
		} else if (brick.tag == "breakable") {
			if (brick.maxHit == 3) {
				newBrick = Instantiate (threeHitBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
			} else if (brick.maxHit == 2) {
				newBrick = Instantiate (twoHitBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
			} else {
				newBrick = Instantiate (oneHitBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
			}
		} else {
			newBrick = Instantiate (indestructibleBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
		}
		newBrick.setMaxHits (brick.maxHit);
		for (int i = 0; i < brick.timesHit; ++i) {
			Debug.LogError ("INSIDE i = " + i + " NewBrick " + newBrick + "==" + newBrick.getLives ());
			newBrick.handleHits ();
		}
		newBrick.setTimesHit (brick.timesHit);

	}

	public void FireSaveEvent ()
	{
		savedGame.savedBricks = new List<SaveBrick> ();
		savedGame.bonusesLeft = new List<int> ();
		//If we have any functions in the event:
		if (SaveEvent != null) {
			SaveEvent (null, null);
		}
	}

	public void SaveData ()
	{
		string path = Application.persistentDataPath + "/save.binary";
		if (!Directory.Exists (Application.persistentDataPath))
			Directory.CreateDirectory (Application.persistentDataPath);

		FireSaveEvent ();

		for (int i = 0; i < bonuses.Length; ++i) {
			savedGame.bonusesLeft.Add (bonuses [i].quantity);
		}

		BinaryFormatter formatter = new BinaryFormatter ();

		FileStream SaveObjects = File.Create (path);

		formatter.Serialize (SaveObjects, savedGame);

		SaveObjects.Close ();

	}

	public void LoadData ()
	{
		string path = Application.persistentDataPath + "/save.binary";
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveObjects = File.Open (path, FileMode.Open);

		savedGame = (SaveObjectsList)formatter.Deserialize (saveObjects);

		saveObjects.Close ();

	}

	void OnApplicationFocus (bool focusStatus)
	{
		if (SceneManager.GetActiveScene ().buildIndex > 4) {
			PlayerPrefsManager.setGameLoaded (1);
			Debug.Log ("LOST FOCUS - SAVE start");
			SaveData ();
		}
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (SceneManager.GetActiveScene ().buildIndex > 4) {
			PlayerPrefsManager.setGameLoaded (1);
			Debug.Log ("ON Application pause - SAVE start");
			SaveData ();
		}
	}

}
