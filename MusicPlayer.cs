using UnityEngine;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{

	static MusicPlayer instance = null;
	private List<AudioSource> sounds;

	void Awake ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			sounds = new List<AudioSource> ();
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}

	public void changeVolume (float volume)
	{
		GetComponent<AudioSource> ().volume = volume;
	}

	public int registerSound (AudioSource sound)
	{
		if (!sounds.Contains (sound)) {
			Debug.LogError ("Dodajemy szefie");
			sounds.Add (sound);
			return sounds.Count - 1;
		} else {
			Debug.LogError ("JUZ Jest typie");
			return sounds.IndexOf (sound);
		}
	}

	public void changeSoundsVolume (float volume)
	{
		foreach (AudioSource sound in sounds) {
			sound.volume = volume;
		}
	}

	public void playSound (int index)
	{
		sounds [index].Play ();
	}
}
