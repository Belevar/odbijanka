using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class BonusManager : MonoBehaviour
{
    
	public Slider slider;

    public Vector3 sizeOfBonusSprite;
    private LevelManager levelManger;

	Dictionary<TimeBonus, Slider> timeBonuses = new Dictionary<TimeBonus, Slider> ();
    List<Slider> releasedBonuses = new List<Slider>();

    void Start()
    {
        levelManger = FindObjectOfType<LevelManager>();
    }

	IEnumerator bonusTimeCount (TimeBonus currentBonus, Slider slider)
	{
		if (slider == null) {
			Debug.Log ("Panie ni ma slajdera");
		}
		float bonusDuration = currentBonus.getDuration ();

		while (slider.value < bonusDuration) {
            slider.value =  (Mathf.MoveTowards(slider.value, bonusDuration, 1f * Time.deltaTime));
			yield return null;
		}

        levelManger.brickCounterOutput.text = "koniec bonusu" + currentBonus;
		currentBonus.disactivate ();
        timeBonuses.Remove(currentBonus);
        releasedBonuses.Add(slider);
        slider.transform.SetParent(GameObject.Find("Buttons_and_bonuses").transform);
		Vector2 pos = slider.transform.position;
		pos.y += 75f;

		while (Vector3.Distance (slider.transform.position, pos) > 0.5f) {
            slider.transform.position = new Vector3(slider.transform.position.x, Mathf.MoveTowards( slider.transform.position.y, pos.y, 50f * Time.deltaTime), slider.transform.position.z);
			yield return null;
		}
        
		Destroy (slider.gameObject);
        releasedBonuses.Remove(slider);
		//moveBonus(); //NOT WORKING AS EXPEXTED
	}

	public void moveBonus ()
	{
		//SHOULD CHECK IF newPos is on THE LEFT SIDE 
		//Transform newPos = NextFreePosition();
		var currentBonuses = new List<Slider> (timeBonuses.Values);
		foreach (Slider bonus in currentBonuses) {
			Transform newPos = NextFreePosition ();
			//set Parent to Bonus Manager ?? 
			bonus.transform.SetParent (this.transform);
			Debug.Log ("NEXT FREE:" + newPos.transform.localPosition);
			Debug.Log ("Current POS:" + bonus.transform.localPosition);
			if (newPos.transform.localPosition.x < bonus.transform.localPosition.x) {
				Debug.Log ("Current POS in IF:" + bonus.transform.localPosition);
				bonus.transform.SetParent (newPos);
				Debug.Log ("Current POS: in IF 2" + bonus.transform.localPosition);
				while (Vector3.Distance (bonus.transform.localPosition, newPos.transform.localPosition) > 0.05f) {
					bonus.transform.localPosition = Vector3.Lerp (bonus.transform.localPosition, newPos.transform.localPosition, 3f * Time.deltaTime);
				}
			}
		}
	}

	public void disactivateAllBonuses ()
	{
		var buffer = new List<TimeBonus> (timeBonuses.Keys);
		foreach (TimeBonus bonus in buffer) {
			bonus.disactivate ();
		}
        
        StopAllCoroutines();
	}

	public void registerTimeBonus (TimeBonus newBonus)
	{

        if(timeBonuses.ContainsKey(newBonus))
        {
            timeBonuses[newBonus].value = 0f;
        }
		else {
			timeBonuses.Add (newBonus, spawn (newBonus));
		    StartCoroutine(bonusTimeCount (newBonus, timeBonuses [newBonus]));
        }
	}

	public Slider spawn (TimeBonus newBonus)
	{
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			Slider enemy = Instantiate (slider, freePosition.position, Quaternion.identity) as Slider;
			Image background = enemy.GetComponentInChildren<Image> ();
			background.sprite = newBonus.GetComponent<SpriteRenderer> ().sprite;
			background.SetNativeSize();
            
			background.transform.localScale = new Vector3(0.45f,0.45f,1f);
            background.rectTransform.sizeDelta = sizeOfBonusSprite;
			if (enemy == null) {
				Debug.Log ("Nie stworzyłem go :<");
			}
			enemy.transform.SetParent (freePosition);
			return enemy;
		}
		return null;
	}

	Transform NextFreePosition ()
	{
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}

	public void resetAllBonuses ()
	{
		var buffer = new List<Slider> (timeBonuses.Values);
		foreach (Slider slider in buffer) {
            Destroy(slider.gameObject);
		}
        Debug.Log("RESET BONUSES");
       // var releasedBonusSliders = GetComponentsInChildren<Slider>(GameObject.Find("Buttons_and_bonuses").transform);
       // Debug.Log(releasedBonusSliders.Length);
        foreach (Slider slider in releasedBonuses)
        {
            Debug.Log("RESET BONUSES for");
            Destroy(slider.gameObject);
        }
        releasedBonuses.Clear();
        timeBonuses.Clear();
	}
}
