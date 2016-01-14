using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsController : MonoBehaviour
{
	public Slider soundsSlider;
	public Slider musicSlider;
	public LevelManager levelManager;
	private MusicPlayer musicPlayer;
	// Use this for initialization
	void Start ()
	{
		musicPlayer = GameObject.FindObjectOfType<MusicPlayer> ();
		musicSlider.value = PlayerPrefsManager.getMusicVolume ();
		soundsSlider.value = PlayerPrefsManager.getSoundsVolume ();
	}

	void Update ()
	{
		musicPlayer.changeVolume (musicSlider.value);
		musicPlayer.changeSoundsVolume (soundsSlider.value);
	}

	public void saveAndExit ()
	{
		PlayerPrefsManager.setMusicVolume (musicSlider.value);
		PlayerPrefsManager.setSoundsVolume (soundsSlider.value);
		levelManager.loadScene ("start");
	}
}
