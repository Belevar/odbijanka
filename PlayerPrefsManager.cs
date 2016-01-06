using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour
{

	const string HEALTH_POINTS = "HP";
	const string LEVEL = "level_";


	public static void setHealthPoints (int hp)
	{
		PlayerPrefs.SetInt (HEALTH_POINTS, hp);
	}

	public static int getHealthPoint ()
	{
		return PlayerPrefs.GetInt (HEALTH_POINTS);
	}

	public static void setLevel (int levelNumber)
	{
		if (levelNumber <= Application.levelCount) {
			PlayerPrefs.SetInt (LEVEL, levelNumber);
		}
	}
	
	public static int getLevel ()
	{
		return PlayerPrefs.GetInt (LEVEL);
	}
}
