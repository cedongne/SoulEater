using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Stat playerStat;
    Slider HPgaze;

    public GameObject gameover;
    bool isDead = false;
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
        if (playerStat.hp <= 0)
        {
            isDead = true;
            Image gameoverImage = gameover.GetComponent<Image>();
            gameover.SetActive(true);
            if (gameoverImage.color.a < 0.7)
            {
                gameoverImage.color = new Color(gameoverImage.color.r, gameoverImage.color.g, gameoverImage.color.b, gameoverImage.color.a + (Time.deltaTime / 2f));
            }
        }
    }
}

