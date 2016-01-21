using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
	public Slider slider;
	Bonus bonus;

	void Update ()
	{

		if (bonus != null) {
			slider.value = Mathf.MoveTowards (slider.value, 10f, 1f * Time.deltaTime);
			if (slider.value == 10f) {
				bonus.disactivate ();
			}
		}
	}

	public void registerBonus (Bonus a)
	{
		bonus = a;
		if (bonus == null) {
			Debug.LogError ("Wiedziałeś o tym!");
		}
	}
}
