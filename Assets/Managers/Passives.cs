using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour
{
    Stat stat;

    public Souls curSoul;

    public Souls HedgeSnailSoul;
    public GameObject HedgeSnailPad;

    public Souls BlackCrocoSoul;
    public GameObject BlackCrocoPassive;

    public void Awake()
    {
        stat = GetComponentInParent<Stat>();
    }

    public void turnOnPassive(string soul)
    {
        Debug.Log(soul);
        if (soul == "HedgeSnail")
        {
            HedgeSnail_Pad();
        }
        else if(soul == "Black Crocodile")
        {
            BlackCrocoPassiveOn();
        }
    }

    public void HedgeSnail_Pad()
    {
        Transform player = GameObject.Find("Player").transform;
        GameObject instance = Instantiate(HedgeSnailPad, player.position, player.rotation);
        instance.transform.parent = player;

    }
    public void BlackCrocoPassiveOn()
    {
        Transform player = GameObject.Find("Player").transform;
        GameObject instance = Instantiate(BlackCrocoPassive, player.position, player.rotation);
        instance.transform.parent = player;

        transform.GetComponent<Attacks>().bullet.GetComponent<Damage>().damage += instance.GetComponent<PassiveStat>().increaseDamage;
        stat.maxHp += instance.GetComponent<PassiveStat>().increaseHP;
        stat.hp += instance.GetComponent<PassiveStat>().increaseHP;
    }
}
