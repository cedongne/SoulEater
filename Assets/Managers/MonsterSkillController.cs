using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSkillController : MonoBehaviour
{
    NavMeshAgent nvAgent;
    Animator animator;
    Stat stat;

    MonsterSkill skill;
    public GameObject circle;

    public float skillRange;

    Stopwatch watch;
    public float castingTime;
    void Awake()
    {
        watch = new Stopwatch();
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        nvAgent.enabled = true;
        animator = gameObject.GetComponent<Animator>();

        stat = gameObject.GetComponent<Stat>();
        skill = GetComponent<MonsterSkill>();
        watch.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if (watch.ElapsedMilliseconds > 5000 && stat.hp > 0)
        {
            StopCoroutine("ActivateSkill");
            watch.Restart();
            StartCoroutine("ActivateSkill");
        }
    }

    IEnumerator ActivateSkill()
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        float dist = Vector3.Distance(playerPos, transform.position);
        if ( dist < skillRange)
        {
            GetComponent<MonsterFlag>().isCasting = true;
            nvAgent.isStopped = true;
            animator.SetTrigger("Skill");
            skill.DrawCircle();
            yield return new WaitForSeconds(castingTime);
            skill.Use(playerPos);
            GetComponent<MonsterFlag>().isCasting = false;
        }
    }
}
