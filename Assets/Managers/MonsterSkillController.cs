using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSkillController : MonoBehaviour
{
    NavMeshAgent nvAgent;
    Animator animator;

    MonsterSkill skill;

    Stopwatch watch;
    public float castingTime;
    void Awake()
    {
        watch = new Stopwatch();
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        nvAgent.enabled = true;
        animator = gameObject.GetComponent<Animator>();

        skill = GetComponent<MonsterSkill>();
        watch.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if (watch.ElapsedMilliseconds > 5000)
        {
            StopCoroutine("ActivateSkill");
            StartCoroutine("ActivateSkill");
        }
    }

    IEnumerator ActivateSkill()
    {
        GetComponent<MonsterFlag>().isCasting = true;
        nvAgent.isStopped = true;
        animator.SetTrigger("Skill");
        watch.Restart();
        yield return new WaitForSeconds(castingTime);
        skill.Use();
        GetComponent<MonsterFlag>().isCasting = false;

    }
}
