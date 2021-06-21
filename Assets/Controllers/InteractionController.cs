using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    private Canvas canvas;
    private Camera uiCamera;

    private RectTransform rectParent;
    private RectTransform nameTag;

    public Vector3 offset = new Vector3(0, -2.0f, 0f);
    public Transform targetTr;
    Souls targetSoul;

    public GameObject skillGetEffect;
    public Image[] skillIcons;
    public Text text;
    public Text skillGetText;

    public Stat stat;

    public Image SkillChangeBlackout;
    public Image NewSkill;
    public Image NewSkillIcon;

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;

        rectParent = canvas.GetComponent<RectTransform>();
        nameTag = this.gameObject.GetComponent<RectTransform>();

        stat = GameObject.Find("Player").GetComponent<Stat>();
    }

    private void LateUpdate()
    {
        if (gameObject.GetComponent<Image>().enabled == true)
        {
            targetSoul = targetTr.GetComponent<Souls>();
            text.text = targetSoul.name;
            var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
            var localPos = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectParent, screenPos, uiCamera, out localPos);

            nameTag.localPosition = localPos;
            for (int index = 0; index < 3; index++)
            {
                if (skillIcons[index].GetComponent<Image>().sprite == targetSoul.skillIcon)
                {
                    skillGetText.text = "Already have it";

                    return;
                }
                else
                {
                    skillGetText.text = "Press \"E\" to get";
                }
            }
        }
    }
    public Souls SkillGet(int skillCnt)
    {
        if (skillGetText.text == "Already have it")
            return null;
        skillGetText.text = "Press \"E\" to get";

        targetSoul = targetTr.GetComponent<Souls>();

        if (stat.skillNum == 3)
        {
            stat.isSkillGetting = false;
            NewSkill.gameObject.SetActive(true);
            SkillChangeBlackout.gameObject.SetActive(true);
            NewSkillIcon.sprite = targetSoul.skillIcon;
            GetComponentInParent<UIManager>().isSkillChange = true;
            stat.skillNum--;
            Time.timeScale = 0;
        }
        else
        {
            NewSkill.gameObject.SetActive(false);
            SkillChangeBlackout.gameObject.SetActive(false);
            GetComponentInParent<UIManager>().isSkillChange = false;

            stat.skill[skillCnt] = Instantiate<Souls>(targetSoul);
            stat.skill[skillCnt].gameObject.SetActive(false);
            if (targetSoul.type == Souls.Type.PASSIVE)
                GameObject.Find("SkillCanvas").transform.GetChild(4 + skillCnt).GetChild(0).gameObject.SetActive(true);
            if (targetSoul.type == Souls.Type.ACTIVE || targetSoul.type == Souls.Type.PROJECTILE || targetSoul.type == Souls.Type.SWING)
                GameObject.Find("SkillCanvas").transform.GetChild(4 + skillCnt).GetChild(0).gameObject.SetActive(false);
            stat.skill[skillCnt].transform.parent = GameObject.Find("Player").transform;

            skillIcons[skillCnt].GetComponent<Image>().sprite = targetSoul.skillIcon;
            skillIcons[skillCnt].gameObject.SetActive(true);
            stat.skillNum++;
            stat.isSkillGetting = true;
            return targetSoul;
        }
        return null;
    }
}
