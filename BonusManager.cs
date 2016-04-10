using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class BonusManager : MonoBehaviour {
    
    public Slider slider;

    Dictionary<TimeBonus, Slider> timeBonuses = new Dictionary<TimeBonus, Slider>();

    IEnumerator bonusTimeCount(TimeBonus currentBonus, Slider slider)
    {
        if(slider == null)
        {
            Debug.Log("Panie ni ma slajdera");
        }
        float bonusDuration = currentBonus.getDuration();
        while (slider.value < bonusDuration)
        {
            currentBonus.setCurrentTime(Mathf.MoveTowards(currentBonus.getCurrentTime(), bonusDuration, 1f * Time.deltaTime));
            slider.value = currentBonus.getCurrentTime();
            
            yield return null;
        }
        print("koniec bonusu");
        currentBonus.disactivate();

        Vector2 pos = slider.transform.position;
        pos.y +=75f;


       while (Vector3.Distance(slider.transform.position, pos) > 0.05f)
        {
            slider.transform.position = Vector3.Lerp(slider.transform.position, pos, 3f * Time.deltaTime);
            yield return null;
        }

        Slider a = timeBonuses[currentBonus];
        timeBonuses.Remove(currentBonus);
        Destroy(a.gameObject);
       //moveBonus(); //NOT WORKING AS EXPEXTED
    }

    public void moveBonus()
    {
        //SHOULD CHECK IF newPos is on THE LEFT SIDE 
       //Transform newPos = NextFreePosition();
        var currentBonuses = new List<Slider>(timeBonuses.Values);
        foreach (Slider bonus in currentBonuses)
        {
            Transform newPos = NextFreePosition();
            //set Parent to Bonus Manager ?? 
            bonus.transform.SetParent(this.transform);
            Debug.Log("NEXT FREE:" + newPos.transform.localPosition);
            Debug.Log("Current POS:" + bonus.transform.localPosition);
            if (newPos.transform.localPosition.x < bonus.transform.localPosition.x)
            {
                Debug.Log("Current POS in IF:" + bonus.transform.localPosition);
                bonus.transform.SetParent(newPos);
                Debug.Log("Current POS: in IF 2" + bonus.transform.localPosition);
                while (Vector3.Distance(bonus.transform.localPosition, newPos.transform.localPosition) > 0.05f)
                {
                    bonus.transform.localPosition = Vector3.Lerp(bonus.transform.localPosition, newPos.transform.localPosition, 3f * Time.deltaTime);
                }
            }
        }
    }

    public void disactivateAllBonuses()
    {
        var buffer = new List<TimeBonus>(timeBonuses.Keys);
        foreach (TimeBonus bonus in buffer)
        {
            bonus.disactivate();
        }
    }

    public void registerTimeBonus(TimeBonus newBonus)
    {
        bool bonusOfThatTypeIsRegistered = false;
        var buffer = new List<TimeBonus>(timeBonuses.Keys);
        foreach (TimeBonus bonus in buffer)
        {
            if (bonus.GetType() == newBonus.GetType()) 
            {
                bonusOfThatTypeIsRegistered = true;
                bonus.setCurrentTime(0f);
            }
        }
        if (!bonusOfThatTypeIsRegistered)
        {
            timeBonuses.Add(newBonus, spawn(newBonus));
            StartCoroutine(bonusTimeCount(newBonus, timeBonuses[newBonus]));
        }
    }

    public Slider spawn(TimeBonus newBonus)
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            Slider enemy = Instantiate(slider, freePosition.position, Quaternion.identity) as Slider;
            Image background = enemy.GetComponentInChildren<Image>();
            background.sprite = newBonus.GetComponent<SpriteRenderer>().sprite;
            if(enemy == null)
            {
                Debug.Log("Nie stworzyłem go :<");
            }
            enemy.transform.SetParent(freePosition);
            return enemy;
        }
        return null;
    }
		
	Transform NextFreePosition(){
		foreach(Transform childPositionGameObject in transform){
			if (childPositionGameObject.childCount == 0){
				return childPositionGameObject;
			}
		}
		return null;
	}

    public void resetAllBonuses()
    {
        var buffer = new List<Slider>(timeBonuses.Values);
        foreach (Slider slider in buffer)
        {
            slider.value = 10f;
        }
    }
}
