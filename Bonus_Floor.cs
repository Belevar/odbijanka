using UnityEngine;
using System.Collections;

public class Bonus_Floor : TimeBonus
{
    public AudioClip bonusSound;

    override public void activateBonus()
    {
        AudioSource.PlayClipAtPoint(bonusSound, transform.position, FindObjectOfType<MusicPlayer>().getVolume());
        GameObject.Find("PlaySpace").transform.FindChild("bonus_floor").gameObject.SetActive(true);
        FindObjectOfType<BonusManager>().registerTimeBonus(this);
        GetComponent<SpriteRenderer>().enabled = false;
        transform.position = new Vector3(-10f, -10f, -10f);
    }

    override public void disactivate()
    {
        GameObject.Find("PlaySpace").transform.FindChild("bonus_floor").gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "paddle")
        {
            activateBonus();
        }
    }
}
