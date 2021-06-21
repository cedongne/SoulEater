using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooltime : MonoBehaviour
{
    public GameObject skill1Object;
    public GameObject skill2Object;
    public GameObject skill3Object;

    Image skill1;
    Image skill2;
    Image skill3;

    bool is1Ready = true;
    bool is2Ready = true;
    bool is3Ready = true;

    Stat stat;
    // Start is called before the first frame update
    void Start()
    {
        skill1 = skill1Object.GetComponent<Image>();
        skill2 = skill2Object.GetComponent<Image>();
        skill3 = skill3Object.GetComponent<Image>();
        stat = GameObject.Find("Player").GetComponent<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Skill1") && is1Ready)
        {
            skill1.fillAmount = 1;
            is1Ready = false;
            StartCoroutine(CoolTime1(stat.skill[0].coolTime));
        }
        if (Input.GetButtonDown("Skill2") && is2Ready)
        {
            skill2.fillAmount = 1;
            is2Ready = false;
            StartCoroutine(CoolTime2(stat.skill[1].coolTime));
        }
        if (Input.GetButtonDown("Skill3") && is3Ready)
        {
            skill3.fillAmount = 1;
            is3Ready = false;
            StartCoroutine(CoolTime3(stat.skill[2].coolTime));
        }
    }

    IEnumerator CoolTime1(float coolTime)
    {
        while (skill1.fillAmount > 0)
        {
            skill1.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
            yield return null;
        }
        is1Ready = true;
        yield break;
    }
    IEnumerator CoolTime2(float coolTime)
    {
        while (skill2.fillAmount > 0)
        {
            skill2.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
            yield return null;
        }
        is2Ready = true;
        yield break;
    }
    IEnumerator CoolTime3(float coolTime)
    {
        while (skill3.fillAmount > 0)
        {
            skill3.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
            yield return null;
        }
        is3Ready = true;
        yield break;
    }
}
