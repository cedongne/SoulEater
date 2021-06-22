using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    Stat playerStat;
    Slider HPgaze;

    public GameObject gameover;

    public bool isSkillChange;
    public Image[] SkillCursors;

    public Image NewSkill;
    public Image interaction;

    Passives passives;
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
        passives = GameObject.Find("Weapon").GetComponent<Passives>();
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
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene("SampleScene");
                }
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
                    if(playerStat.skill[skillNum].type == Souls.Type.PASSIVE)
                    {
                        passives.turnOffPassive(playerStat.skill[skillNum].monsterName);
                    }
                    Destroy(GameObject.Find("Player").transform.Find(playerStat.skill[skillNum].name).gameObject);
                    Souls currSoul = interaction.GetComponent<InteractionController>().SkillGet(skillNum);
                    if (currSoul != null && currSoul.type == Souls.Type.PASSIVE)
                    {
                        Debug.Log("Passive On");
                        passives.turnOnPassive(currSoul.monsterName);
                    }
                }
                SkillCursors[skillNum].gameObject.SetActive(false);
                isSkillChange = false;
                Time.timeScale = 1;
            }
        }
    }
}