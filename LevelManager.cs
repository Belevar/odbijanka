using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	private int lives;

	void Start ()
	{
		lives = 3;
	}

	public bool loseLiveAndCheckEndGame ()
	{
		print (lives);
		return (--lives) <= 0;
	}

	public void addLive ()
	{
		++lives;
	}

	public void loadNextLevel ()
	{
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void quitRequest ()
	{
		Debug.Log ("Quit request!");
	}
    
	public void loadScene (string name)
	{
		Application.LoadLevel (name);
	}

	public void brickDestroyed ()
	{
		if (Brick.bricksCounter <= 0) {
			loadNextLevel ();
		}
	}
}
