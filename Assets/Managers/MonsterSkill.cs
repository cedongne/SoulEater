using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public GameObject monster;
    public GameObject[] skillList;

    GameObject skillUse;
    GameObject circle;
    int idx;
    Vector3 sSkillPos;
    Vector3[] mSkillPos;

    GameObject instance;

    public void Awake()
    {
        circle = GetComponent<MonsterSkillController>().circle;
        mSkillPos = new Vector3[10];
        idx = Random.Range(0, skillList.Length);
    }
    public void Use(Vector3 playerPos)
    {
        if (skillList[idx].GetComponent<skillFlag>().isRandomSpawn)
        {
            for(int i = 0; i < 10; i++)
            {
                skillUse = Instantiate(skillList[idx], new Vector3(mSkillPos[i].x, 
                    mSkillPos[i].y + 2.0f, mSkillPos[i].z),
                    Quaternion.Euler(transform.rotation.x, 
                    transform.rotation.y, transform.rotation.z));

                Destroy(skillUse, 2f);
            }
        }
        else
        {
            skillUse = Instantiate(skillList[idx],
            new Vector3(sSkillPos.x, sSkillPos.y + 2.0f, sSkillPos.z),
            Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));

            if (skillUse.GetComponent<skillFlag>().isProjectile)
            {
                Rigidbody SkillRigid = skillUse.GetComponent<Rigidbody>();
                playerPos.y += 2.0f;
                skillUse.transform.LookAt(playerPos);
                SkillRigid.velocity = skillUse.transform.forward * 75;
            }
            Destroy(skillUse, 2f);
        }
    }
    public void setMultiSkillPos()
    {
        for(int i = 0; i < 10; i++)
        {
            int xRandom = Random.Range(0, 30);
            int zRandom = Random.Range(0, 30);
            Vector3 pos = transform.position;
            pos.x += xRandom - 15;
            pos.z += zRandom - 15;
            mSkillPos[i] = pos;
        }
    }
    public void setSingleSkillPos()
    {
        sSkillPos = new Vector3(transform.position.x, transform.position.y,
            transform.position.z);
    }
    public void DrawCircle()
    {
        idx = Random.Range(0, skillList.Length);
        if (skillList[idx].GetComponent<skillFlag>().isRandomSpawn)
        {
            setMultiSkillPos();
            for (int i = 0; i < 10; i++)
            {
                instance = Instantiate(circle,
                mSkillPos[i], Quaternion.identity);
                float cirSize = skillList[idx].transform.GetComponent<SkillStat>().size;
                instance.transform.localScale = new Vector3(cirSize, 0.001f, cirSize);

                Destroy(instance, 2f);
            }
        }
        else
        {
            setSingleSkillPos();
            if (!skillList[idx].GetComponent<skillFlag>().isProjectile)
            {
                instance = Instantiate(circle,
                sSkillPos, Quaternion.identity);
                float cirSize = skillList[idx].transform.GetComponent<SkillStat>().size;
                instance.transform.localScale = new Vector3(cirSize, 0.001f, cirSize);

                Destroy(instance, 2f);
            }
        }
    }
}
