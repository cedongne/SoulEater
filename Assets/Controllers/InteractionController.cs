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
    Text text;
    public Text skillGetText;

    Stat stat;
    // Start is called before the first frame update
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;

        rectParent = canvas.GetComponent<RectTransform>();
        nameTag = this.gameObject.GetComponent<RectTransform>();

        text = nameTag.GetComponentInChildren<Text>();
        stat = GameObject.Find("Player").GetComponent<Stat>();
    }

    private void LateUpdate()
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

    public void SkillGet()
    {
        targetSoul = targetTr.GetComponent<Souls>();
        if (stat.skillNum == 3)
        {

        }
        else
        {
            if (skillGetText.text == "Already have it")
                return;
            skillGetText.text = "Press \"E\" to get";

            stat.skill[stat.skillNum] = Instantiate<Souls>(targetSoul);
            stat.skill[stat.skillNum].gameObject.SetActive(false);
            if (targetSoul.type == Souls.Type.PASSIVE)
                GameObject.Find("SkillCanvas").transform.GetChild(3 + stat.skillNum).GetChild(0).gameObject.SetActive(true);
            if (targetSoul.type == Souls.Type.ACTIVE)
                GameObject.Find("SkillCanvas").transform.GetChild(3 + stat.skillNum).GetChild(0).gameObject.SetActive(false);
            stat.skill[stat.skillNum].transform.parent = GameObject.Find("Player").transform;

            skillIcons[stat.skillNum].GetComponent<Image>().sprite = targetSoul.skillIcon;
            skillIcons[stat.skillNum].gameObject.SetActive(true);
            stat.skillNum++;
        }
    }

    void disableAlreadyText()
    {
    }
}
