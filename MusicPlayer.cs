using UnityEngine;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{

	static MusicPlayer instance = null;
	private float soundVolume;

	void Awake ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			soundVolume = PlayerPrefsManager.getSoundsVolume ();
			changeVolume (PlayerPrefsManager.getMusicVolume ());
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}

	public void changeVolume (float volume)
	{
		GetComponent<AudioSource> ().volume = volume;
	}

	public void changeSoundsVolume (float volume)
	{
		soundVolume = volume;
	}

	public float getVolume ()
	{
		return soundVolume;
	}
}
