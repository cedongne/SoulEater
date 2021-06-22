using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Stat playerStat;
    Slider HPgaze;

    public GameObject gameover;

    public bool isSkillChange;
    public Image[] SkillCursors;

    public Image NewSkill;
    public Image interaction;

    int skillNum = 0;

    Text GameOverText;
    public GameObject GameOver;
    public bool isGameClear;
    // Start is called before the first frame update
    void Start()
    {
        GameOverText = GameOver.GetComponent<Text>();
        playerStat = GameObject.Find("Player").GetComponent<Stat>();
        HPgaze = GameObject.Find("HPGaze").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HPgaze.value = (float)playerStat.hp / (float)playerStat.maxHp;
        if (playerStat.hp <= 0 || isGameClear)
        {
            if (isGameClear)
            {
                GameOverText.text = "Clear!";
            }
            Image gameoverImage = gameover.GetComponent<Image>();
            gameover.SetActive(true);
            if (gameoverImage.color.a < 0.7)
            {
                gameoverImage.color = new Color(gameoverImage.color.r, gameoverImage.color.g, gameoverImage.color.b, gameoverImage.color.a + (Time.deltaTime / 2f));
            }
        }
        if (isSkillChange)
        {
            SkillCursors[skillNum % 3].gameObject.SetActive(true);
            SkillCursors[(skillNum + 1) % 3].gameObject.SetActive(false);
            SkillCursors[(skillNum + 2) % 3].gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.A))
                skillNum = (skillNum + 2) % 3;
            if (Input.GetKeyDown(KeyCode.D))
                skillNum = (skillNum + 1) % 3;
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interaction.GetComponent<InteractionController>().SkillGet(skillNum);

                }
                SkillCursors[skillNum].gameObject.SetActive(false);
                isSkillChange = false;
                Time.timeScale = 1;
            }
        }
    }
}