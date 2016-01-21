using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
	public Slider slider;
	TimeBonus bonus;
	Dictionary<TimeBonus, float> timeBonuses = new Dictionary<TimeBonus, float> ();



	void Update ()
	{

//		if (bonus != null) {
////			timeBonuses[0].updateSlider() czy coś tam
//			slider.value = Mathf.MoveTowards (slider.value, 10f, 1f * Time.deltaTime);
//			if (slider.value >= 10f) {
//				bonus.disactivate ();
//			}
		//		}
		var buffer = new List<TimeBonus> (timeBonuses.Keys);
		foreach (TimeBonus bonus in buffer) {
			slider.value = timeBonuses [bonus] = Mathf.MoveTowards (timeBonuses [bonus], 10f, 1f * Time.deltaTime);
			if (timeBonuses [bonus] >= 10f) {
				bonus.disactivate ();
			}
		}
	}

	public void disactivateAllBonuses ()
	{
		var buffer = new List<TimeBonus> (timeBonuses.Keys);
		foreach (TimeBonus bonus in buffer) {
			bonus.disactivate ();
		}
	}

	public void registerTimeBonus (TimeBonus newBonus)
	{
		bool bonusOfThatTypeIsRegistered = false;
		var buffer = new List<TimeBonus> (timeBonuses.Keys);
		foreach (TimeBonus bonus in buffer) {
			Debug.LogError ("DUPA");
			if (bonus.GetType () == newBonus.GetType ()) {
				bonusOfThatTypeIsRegistered = true;
				timeBonuses [bonus] = 0f;
			}
		}
		if (!bonusOfThatTypeIsRegistered) {
			timeBonuses.Add (newBonus, 0f);
		}
//		if (timeBonuses.ForEach ().GetType () == a.GetType ()) {
//			Debug.LogError ("JEST");
//		} else {
//			Debug.LogError ("NI ma Dodaje");
//			timeBonuses.Add (a);
//		}
	}
}
