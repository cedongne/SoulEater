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

    public Souls MimicSoul;
    public GameObject MimicPassive;

    public Souls GolemSoul;
    public GameObject GolemPassive;
    public GameObject bullet;

    public void Awake()
    {
        stat = GetComponentInParent<Stat>();
        bullet.GetComponent<Damage>().damage = 10;
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
        else if(soul == "Mimic")
        {
            MimicPassiveOn();
        }
        else if(soul == "Golem")
        {
            GolemPassiveOn();
        }
    }
    
    public void turnOffPassive(string soul)
    {
        Debug.Log(soul);
        if (soul == "HedgeSnail")
        {
            HedgeSnail_PadOff();
        }
        else if (soul == "Black Crocodile")
        {
            BlackCrocoPassiveOff();
        }
        else if(soul == "Mimic")
        {
            MimicPassiveOff();
        }
        else if (soul == "Golem")
        {
            GolemPassiveOff();
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
    public void MimicPassiveOn()
    {
        Transform player = GameObject.Find("Player").transform;
        GameObject instance = Instantiate(MimicPassive, player.position, player.rotation);
        instance.transform.parent = player;
    }
    public void GolemPassiveOn()
    {
        transform.GetComponent<Attacks>().bullet.GetComponent<Damage>().damage += GolemPassive.GetComponent<PassiveStat>().increaseDamage;
        GolemPassive.GetComponent<Damage>().damage = transform.GetComponent<Attacks>().bullet.GetComponent<Damage>().damage;
        transform.GetComponent<Attacks>().bullet = GolemPassive;
    }



    public void HedgeSnail_PadOff()
    {
        Destroy(GameObject.Find("HedgeSnailPassive(Clone)"));
    }
    public void BlackCrocoPassiveOff()
    {
        GameObject instance = GameObject.Find("BlackCrocoPassive(Clone)").gameObject;

        transform.GetComponent<Attacks>().bullet.GetComponent<Damage>().damage -= instance.GetComponent<PassiveStat>().increaseDamage;
        stat.maxHp -= instance.GetComponent<PassiveStat>().increaseHP;

        Destroy(instance);
    }
    public void MimicPassiveOff()
    {
        GameObject instance = GameObject.Find("MimicPassive(Clone)").gameObject;

        float decCool = instance.GetComponent<PassiveStat>().decreaseCooltimeRate;
        Destroy(instance);
        for (int i = 0; i < stat.skill.Length; i++)
        {
            if (stat.skill[i].isCoolDown)
            {
                stat.skill[i].coolTime *= 100 / (100 - decCool);
                stat.skill[i].isCoolDown = false;
            }
        }
    }
    public void GolemPassiveOff()
    {
        transform.GetComponent<Attacks>().bullet.GetComponent<Damage>().damage -= GolemPassive.GetComponent<PassiveStat>().increaseDamage;
        bullet.GetComponent<Damage>().damage = transform.GetComponent<Attacks>().bullet.GetComponent<Damage>().damage;
        transform.GetComponent<Attacks>().bullet = bullet;
    }
}
