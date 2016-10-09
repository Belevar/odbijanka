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
	public GameObject loseMesseage;
    public GameObject bricksGraphic;
    public AudioClip loseSound;


	//SAVE SHIT
	public SaveObjectsList savedGame;

	public delegate void SaveDelegate (object sender, EventArgs args);

	public static event SaveDelegate SaveEvent;

	public Brick invisibleBrick;
	public Brick oneHitBrick;
	public Brick twoHitBrick;
	public Brick threeHitBrick;
	public Brick indestructibleBrick;
    public Brick builderBrick;
    public Brick explodingBrick;

    public Transform bricksHolder;

	[Serializable]
	public struct Bonuses
	{
		public int quantity;
		public GameObject bonusType;
	}

	public Bonuses[] bonuses;

    public enum PAUSE_GAME
    {
        PAUSE,
        UNPAUSE
    }

	void Awake ()
	{
		Debug.Log (SceneManager.GetActiveScene ().name);
		Debug.Log (PlayerPrefsManager.isGameLoaded ());
		if (SceneManager.GetActiveScene ().name == "start" && PlayerPrefsManager.isGameLoaded () == 0) {
			GameObject.Find ("resume game").GetComponent<Button> ().interactable = false;
            GameObject.Find("resume game").GetComponent<Text>().color = new Color(255f,255f,255f,0.2f);
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
        AudioSource.PlayClipAtPoint(loseSound, transform.position, FindObjectOfType<MusicPlayer>().getVolume());
		bool zeroLives = --lives <= 0;
		PlayerPrefsManager.setHealthPoints (lives);
		FindObjectOfType<BonusManager> ().disactivateAllBonuses ();
		FindObjectOfType<BonusManager> ().resetAllBonuses ();
		cleanSceneAfterDeath (); //

		if (zeroLives) {
			livesSprite.enabled = false;
			showLoseMessage ();
		} else {
			livesSprite.sprite = hearts [lives - 1];
		}
		return zeroLives;
	}

	void showLoseMessage ()
	{
		loseMesseage.SetActive (true);
        bricksGraphic.SetActive(false);
		pauseGameWithoutPauseMenu (PAUSE_GAME.PAUSE);
	}

    public GameObject noInternet;

    public void showAdd()
    {
        Debug.Log("Przed reklamą");
        if (adds.areAddsInitialized())
        {
            adds.ShowRewardedAd();
            loseMesseage.SetActive(false);
            bricksGraphic.SetActive(true);
            Debug.Log("PO reklamie");
        }
        else
        {
           // GameObject.Find("No internet connection").GetComponent<Animator>().SetTrigger("No internet");
            Animator animator = noInternet.GetComponent<Animator>();
            if(animator == null)
            {
                Debug.Log("NIE MA ANIMATORA");
            }else
            {
               // pauseGameWithoutPauseMenu(PAUSE_GAME.UNPAUSE);
                Debug.Log("Jest Animator");
                animator.Play("slow disappear");
               // pauseGameWithoutPauseMenu(PAUSE_GAME.PAUSE);

            }
            Debug.LogError("NO INTERNET CONNECTION");
        }
    }

	public void checkEndGame ()
	{
		if (lives <= 0) {
			PlayerPrefsManager.setGameLoaded (0);
			pauseGameWithoutPauseMenu (PAUSE_GAME.UNPAUSE);
			loadScene ("start");
		}
	}

	public void cleanSceneAfterDeath ()
	{
        StartCoroutine(waitForRessurect());
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
        FindObjectOfType<Paddle>().resetPaddle();
        FindObjectOfType<Ball>().resetBall();
    }

	public void addLive ()
	{
		Debug.Log ("ADD LIVE");
		if (lives < maxLives) {
			++lives;
			PlayerPrefsManager.setHealthPoints (lives);
			livesSprite.enabled = true;
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
		Ball.resetBallCounter ();
		Brick.bricksCounter = 0;
		if (SceneManager.GetActiveScene ().buildIndex + 1 < SceneManager.sceneCountInBuildSettings) {
			PlayerPrefsManager.setLevel (PlayerPrefsManager.getLevel () + 1);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);			
		} else {
			SceneManager.LoadScene ("start");
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
		if (!loseMesseage.activeSelf) {
			SaveData ();
			Ball.resetBallCounter ();
			Brick.bricksCounter = 0;
			PlayerPrefsManager.setGameLoaded (1);
			SceneManager.LoadScene ("start");
		}
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
		if (!loseMesseage.activeSelf) {
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

    public void pauseGameWithoutPauseMenu(PAUSE_GAME state)
	{
	    if (state == PAUSE_GAME.UNPAUSE) {
			Time.timeScale = 1;
			paused = false;
		} else if(state == PAUSE_GAME.PAUSE){
			paused = true;
			Time.timeScale = 0; 
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
		Brick newBrick = null;

        switch (brick.brickType)
        {
            case Brick.BRICK_TYPE.INVISIBLE:
                newBrick = Instantiate(invisibleBrick, new Vector3(brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
                break;
            case Brick.BRICK_TYPE.INDESTRUCTIBLE:
                newBrick = Instantiate(indestructibleBrick, new Vector3(brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
                break;
            case Brick.BRICK_TYPE.NORMAL_1_HIT:
                newBrick = Instantiate (oneHitBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
                break;
            case Brick.BRICK_TYPE.NORMAL_2_HIT:
                newBrick = Instantiate (twoHitBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
                break;
            case Brick.BRICK_TYPE.NORMAL_3_HIT:
                newBrick = Instantiate (threeHitBrick, new Vector3 (brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
                break;
            case Brick.BRICK_TYPE.BUILDER:
                newBrick = Instantiate(builderBrick, new Vector3(brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
                break;
            case Brick.BRICK_TYPE.EXPLODING:
                newBrick = Instantiate(explodingBrick, new Vector3(brick.PositionX, brick.PositionY, 0f), Quaternion.identity) as Brick;
                break;
        }

		newBrick.setMaxHits (brick.maxHit);
		for (int i = 0; i < brick.timesHit; ++i) {
			Debug.LogError ("INSIDE i = " + i + " NewBrick " + newBrick + "==" + newBrick.getLives ());
			newBrick.handleHits ();
		}
        newBrick.transform.SetParent(bricksHolder);
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

    IEnumerator waitForRessurect()
    {
        paused = true;
        print(Time.time);
        yield return new WaitForSeconds(2);
        print(Time.time);
        paused = false;
    }

}
