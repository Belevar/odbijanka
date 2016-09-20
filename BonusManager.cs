using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class BonusManager : MonoBehaviour
{
    
	public Slider slider;

    public Vector3 sizeOfBonusSprite;

	Dictionary<TimeBonus, Slider> timeBonuses = new Dictionary<TimeBonus, Slider> ();
    List<Slider> releasedBonuses = new List<Slider>();

	IEnumerator bonusTimeCount (TimeBonus currentBonus, Slider slider)
	{
		if (slider == null) {
			Debug.Log ("Panie ni ma slajdera");
		}
		float bonusDuration = currentBonus.getDuration ();
        Vector3 orginalPos = slider.transform.position;

		while (slider.value < bonusDuration) {
            slider.value =  (Mathf.MoveTowards(slider.value, bonusDuration, 1f * Time.deltaTime));
			yield return null;
		}

		currentBonus.disactivate ();
        timeBonuses.Remove(currentBonus);
        releasedBonuses.Add(slider);
       // slider.transform.SetParent(GameObject.Find("Buttons_and_bonuses").transform);
		Vector2 pos = slider.transform.position;
		pos.y += 75f;

		while (Vector3.Distance (slider.transform.position, pos) > 0.5f) {
            slider.transform.position = new Vector3(slider.transform.position.x, Mathf.MoveTowards( slider.transform.position.y, pos.y, 50f * Time.deltaTime), slider.transform.position.z);
			yield return null;
		}

        slider.transform.position = orginalPos;
        slider.GetComponentInChildren<Image>().sprite = null;
        slider.GetComponentInChildren<Image>().color = Color.white;
        //slider.GetComponent<SpriteRenderer>().color = Color.white;
        slider.value = 0.0f;
        
		//Destroy (slider.gameObject);
        releasedBonuses.Remove(slider);
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
		Slider freeSlider = NextFreePosition ();
        if (freeSlider)
        {
			//Slider enemy = Instantiate (slider, freePosition.position, Quaternion.identity) as Slider;
            Image background = freeSlider.GetComponentInChildren<Image>();
			background.sprite = newBonus.GetComponent<SpriteRenderer> ().sprite;
            background.color = Color.black;
			//background.SetNativeSize();
            
			//background.transform.localScale = new Vector3(0.45f,0.45f,1f);
            //background.rectTransform.sizeDelta = sizeOfBonusSprite;
            if (freeSlider == null)
            {
				Debug.Log ("Nie stworzyłem go :<");
			}
            //freeSlider.transform.SetParent(freePosition);
            return freeSlider;
		}
		return null;
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
		var buffer = new List<Slider> (timeBonuses.Values);
		foreach (Slider slider in buffer) {
            Vector2 pos = slider.transform.localPosition;
           // slider.transform.
           // Debug.Log("Sliders local" + pos);
            Debug.Log("Sliders normal" + slider.transform.position);
            pos.y = -42.1f;
            slider.transform.localPosition = pos;
            slider.GetComponentInChildren<Image>().sprite = null;
            slider.GetComponentInChildren<Image>().color = Color.white;
            slider.value = 0.0f;
		}
        foreach (Slider slider in releasedBonuses)
        {
            Vector2 pos = slider.transform.localPosition;

            Debug.Log("Sliders released" + pos);
            pos.y = -42.1f;
            slider.transform.localPosition = pos;
            slider.GetComponentInChildren<Image>().sprite = null;
            slider.GetComponentInChildren<Image>().color = Color.white;
            slider.value = 0.0f;
        }
        releasedBonuses.Clear();
        timeBonuses.Clear();
	}
}
