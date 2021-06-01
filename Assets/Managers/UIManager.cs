using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Stat playerStat;
    Slider HPgaze;

    // Start is called before the first frame update
    void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<Stat>();
        HPgaze = GameObject.Find("HPGaze").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HPgaze.value = (float)playerStat.hp / (float)playerStat.maxHp;
    }

}
