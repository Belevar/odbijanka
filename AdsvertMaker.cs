using UnityEngine;
using UnityEngine.Advertisements;

public class AdsvertMaker : MonoBehaviour
{

	private LevelManager levelManger;

	void Start ()
	{
		levelManger = GetComponentInParent<LevelManager> ();
	}

	public void ShowRewardedAd ()
	{
		if (Advertisement.IsReady ()) {
			levelManger.pauseGame ();
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("", options);

		} else {
			Debug.Log ("IS NOT READY");
		}
	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			levelManger.addLive ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
			levelManger.checkEndGame ();
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
			levelManger.checkEndGame ();
			break;
		}
		levelManger.pauseGame ();
	}

	public bool isShowing ()
	{
		return Advertisement.isShowing;
	}
}
