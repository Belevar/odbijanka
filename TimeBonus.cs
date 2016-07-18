using UnityEngine;
using System.Collections;

public abstract class TimeBonus : MonoBehaviour
{
    private float duration = 10f;
    private float currentTime;

	abstract  public void activateBonus ();
	abstract  public void disactivate () ;
    
    public void setCurrentTime(float time)
    {
        currentTime = time;
    }

    public float getCurrentTime()
    {
        return currentTime;
    }

    public float getDuration()
    {
        return duration;
    }

    public override int GetHashCode()
    {
        return gameObject.name.GetHashCode();
    }

    public override bool Equals(object o)
    {
        TimeBonus timeBonus = o as TimeBonus;
        return gameObject.name == timeBonus.gameObject.name;
    }

}
