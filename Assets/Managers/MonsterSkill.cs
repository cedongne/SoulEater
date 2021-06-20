using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public GameObject monster;
    public GameObject[] skillEffect;

    GameObject skillUse;
    public void Use()
    {
        int idx = Random.Range(0, skillEffect.Length);

        if (skillEffect[idx].GetComponent<skillFlag>().isRandomSpawn)
        {
            for(int i = 0; i < 5; i++)
            {
                int xRandom = Random.Range(0, 10);
                int zRandom = Random.Range(0, 10);
                Vector3 pos = transform.position;
                pos.x += xRandom - 5;
                pos.y += 2;
                pos.z += zRandom - 5;

                skillUse = Instantiate(skillEffect[idx], pos,
                Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));

                Destroy(skillUse, 2f);
            }
        }
        else
        {
            skillUse = Instantiate(skillEffect[idx],
            new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z),
            Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));

            if (skillUse.GetComponent<skillFlag>().isProjectile)
            {
                Rigidbody SkillRigid = skillUse.GetComponent<Rigidbody>();
                Vector3 playerPos = GameObject.Find("Player").transform.position;
                playerPos.y += 2.0f;
                skillUse.transform.LookAt(playerPos);
                SkillRigid.velocity = skillUse.transform.forward * 75;

                Destroy(skillUse, 2f);
            }

        }
    }
}
