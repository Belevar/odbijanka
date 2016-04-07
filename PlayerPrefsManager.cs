using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour
{

	const string HEALTH_POINTS = "HP";
	const string LEVEL = "level_";
	const string MUSIC_VOLUME = "music_volume";
	const string SOUNDS_VOLUME = "sounds_volume";
    const string GAME_LOADED = "game_loaded";


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

	public static float getMusicVolume ()
	{
		return PlayerPrefs.GetFloat (MUSIC_VOLUME);
	}
	public static void setMusicVolume (float volume)
	{
		PlayerPrefs.SetFloat (MUSIC_VOLUME, volume);
	}

	public static float getSoundsVolume ()
	{
		return PlayerPrefs.GetFloat (SOUNDS_VOLUME);
	}
	public static void setSoundsVolume (float volume)
	{
		PlayerPrefs.SetFloat (SOUNDS_VOLUME, volume);
	}

    public static void setGameLoaded(int isLoaded)
    {
        PlayerPrefs.SetInt(GAME_LOADED, isLoaded);
    }
    public static int isGameLoaded()
    {
        return PlayerPrefs.GetInt(GAME_LOADED);     
    }

}
