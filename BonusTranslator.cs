using System.Collections.Generic;
using System;
using UnityEngine;

public static class BonusTranslator
{
	public static List<GameObject> convertBonusesToList (LevelManager.Bonuses[] input)
	{
		List<GameObject> bonuses = new List<GameObject> ();

		foreach (var bonusTypes in input) {
			for (int i = 0; i < bonusTypes.quantity; ++i) {
				bonuses.Add (bonusTypes.bonusType);
			}
		}
		bonuses.Shuffle ();
		return bonuses;
	}

	private static System.Random rng = new System.Random ();  
	
	public static void Shuffle<T> (this IList<T> list)
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next (n + 1);  
			T value = list [k];  
			list [k] = list [n];  
			list [n] = value;  
		}  
	}
}
