using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicPassive : MonoBehaviour
{
    PassiveStat passiveStat;
    Stat stat;

    void Awake()
    {
        passiveStat = GetComponent<PassiveStat>();
        stat = GameObject.Find("Player").GetComponent<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stat.skill.Length; i++)
        {
            if(stat.skill[i] != null)
            {
                if (!stat.skill[i].isCoolDown)
                {
                    stat.skill[i].coolTime *= (100 - passiveStat.decreaseCooltimeRate) / 100;
                    stat.skill[i].isCoolDown = true;
                }
            }
        }
    }
}
