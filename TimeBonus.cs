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
}
