using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[Serializable]
public class SaveObjectsList {

    public List<SaveBrick> savedBricks;
    public List<int> bonusesLeft;

    public SaveObjectsList()
    {
        savedBricks = new List<SaveBrick>();
        bonusesLeft = new List<int>();
    }
}
