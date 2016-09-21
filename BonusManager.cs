using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class BonusManager : MonoBehaviour
{
    
	public Slider slider;

    public Vector3 sizeOfBonusSprite;

    public struct BonusSlider
    {
        public Slider slider;
        public Vector3 orginalPosition;

        public BonusSlider(Slider slider, Vector3 position)
        {
            this.slider = slider;
            orginalPosition = position;
        }

    }

    Dictionary<TimeBonus, BonusSlider> timeBonuses = new Dictionary<TimeBonus, BonusSlider>();
    List<BonusSlider> releasedBonuses = new List<BonusSlider>();

    IEnumerator bonusTimeCount(TimeBonus currentBonus, BonusSlider bonusSlider)
	{
		if (bonusSlider.slider == null) {
			Debug.Log ("Panie ni ma slajdera");
		}
		float bonusDuration = currentBonus.getDuration ();
        Vector3 orginalPos = bonusSlider.slider.transform.position;

		while (bonusSlider.slider.value < bonusDuration) {
            bonusSlider.slider.value =  (Mathf.MoveTowards(bonusSlider.slider.value, bonusDuration, 1f * Time.deltaTime));
			yield return null;
		}

		currentBonus.disactivate ();
        timeBonuses.Remove(currentBonus);
        releasedBonuses.Add(bonusSlider);
		Vector2 pos = bonusSlider.slider.transform.position;
		pos.y += 75f;

		while (Vector3.Distance (bonusSlider.slider.transform.position, pos) > 0.5f) {
            bonusSlider.slider.transform.position = new Vector3(bonusSlider.slider.transform.position.x, Mathf.MoveTowards( bonusSlider.slider.transform.position.y, pos.y, 50f * Time.deltaTime), bonusSlider.slider.transform.position.z);
			yield return null;
		}

        bonusSlider.slider.transform.position = orginalPos;
        bonusSlider.slider.GetComponentInChildren<Image>().sprite = null;
        bonusSlider.slider.GetComponentInChildren<Image>().color = Color.white;
        bonusSlider.slider.value = 0.0f;
        
        releasedBonuses.Remove(bonusSlider);
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
            timeBonuses[newBonus].slider.value = 0f;
        }
		else {
			timeBonuses.Add (newBonus, spawn (newBonus));
		    StartCoroutine(bonusTimeCount (newBonus, timeBonuses [newBonus]));
        }
	}

	public BonusSlider spawn (TimeBonus newBonus)
	{
		Slider freeSlider = NextFreePosition ();
        if (freeSlider)
        {
            Image background = freeSlider.GetComponentInChildren<Image>();
			background.sprite = newBonus.GetComponent<SpriteRenderer> ().sprite;
            background.color = Color.black;
            
            if (freeSlider == null)
            {
				Debug.Log ("Nie stworzyłem go :<");
			}
            return new BonusSlider(freeSlider, freeSlider.transform.position);
		}
        return new BonusSlider(); // do something with that
	}

	Slider NextFreePosition ()
	{
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.gameObject.GetComponent<Slider>()) { // not working as expected tu treeba wszystko przerobić : <
                if(childPositionGameObject.gameObject.GetComponent<Slider>().GetComponentInChildren<Image>().sprite == null) // działa teraz przypisac to do slidera , ta funkcja musi slider zwracać i gitara
                {
                    Debug.Log("Bla bla bla ");
                    return childPositionGameObject.gameObject.GetComponent<Slider>();
                }
                else
                {
                    Debug.Log("NIE MA ");
                }
				
			}
		}
		return null;
	}

	public void resetAllBonuses ()
	{
        var buffer = new List<BonusSlider>(timeBonuses.Values);
        foreach (BonusSlider bonusSlider in buffer)
        {
            bonusSlider.slider.transform.position = bonusSlider.orginalPosition;
            bonusSlider.slider.GetComponentInChildren<Image>().sprite = null;
            bonusSlider.slider.GetComponentInChildren<Image>().color = Color.white;
            bonusSlider.slider.value = 0.0f;
		}
        foreach (BonusSlider bonusSlider in releasedBonuses)
        {
            bonusSlider.slider.transform.position = bonusSlider.orginalPosition;
            bonusSlider.slider.GetComponentInChildren<Image>().sprite = null;
            bonusSlider.slider.GetComponentInChildren<Image>().color = Color.white;
            bonusSlider.slider.value = 0.0f;
        }
        releasedBonuses.Clear();
        timeBonuses.Clear();
	}
}
