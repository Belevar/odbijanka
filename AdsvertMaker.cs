using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;


public class AdsvertMaker : MonoBehaviour
{

	private LevelManager levelManger;

	void Start ()
	{
		levelManger = GetComponentInParent<LevelManager> ();
        StartCoroutine(checkInternetConnection());
	}

    void Update()
    {
        Advertisement.IsReady();
    }

	public void ShowRewardedAd ()
	{
            if (Advertisement.IsReady())
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("", options);

            }
            else
            {
                levelManger.checkEndGame();
            }

	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
            levelManger.brickCounterOutput.text = "The ad was successfully shown.";
			levelManger.addLive ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
            levelManger.brickCounterOutput.text = "The ad was skipped before reaching the end.";
            levelManger.checkEndGame ();
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
            levelManger.brickCounterOutput.text = "The ad failed to be shown.";
			levelManger.checkEndGame ();
			break;
        default:
            levelManger.brickCounterOutput.text = "Defff.";
            levelManger.checkEndGame();
        break;   
		}
		levelManger.pauseGameWithoutPauseMenu ();
	}

	public bool isShowing ()
	{
		return Advertisement.isShowing;
	}


    private string androidGameID = "1079174";

    private bool unityAdsInitialized = false;//can be used for informing the user about the status

    public IEnumerator checkInternetConnection()
    {
        float timeCheck = 1.0f;//will check google.com every two seconds
        float t1;
        float t2;
        Debug.Log("Start corutine");
        while (!unityAdsInitialized)
        {
            WWW www = new WWW("http://google.com");
            t1 = Time.fixedTime;
            yield return www;
            Debug.Log("Ni ma INTERNETÓW");
            if (www.error == null)
            {
                Debug.Log("Znalazłem internety");
                Advertisement.Initialize(androidGameID); // initialize Unity Ads.

                unityAdsInitialized = true;

                break;//will end the coroutine
            }
            t2 = Time.fixedTime - t1;
            if (t2 < timeCheck)
                yield return new WaitForSeconds(timeCheck - t2);
        }
    }


}
