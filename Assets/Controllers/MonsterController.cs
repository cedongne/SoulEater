using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    Stat stat;
    public enum CurrentState { idle, trace, attack, dead };
    public CurrentState curState = CurrentState.idle;

    Transform monsterTransform;
    Transform playerTransform;
    NavMeshAgent nvAgent;
    Animator animator;

    SphereCollider monsterCollider;
    GameObject hitBox;
    MeshRenderer[] meshs;

    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);

    GameObject hpBar;
    public GameObject soul;
    Canvas hpCanvas;
    Slider slider;

    public float attackDist = 4.0f;
    public float moveDist = 10.0f;
    public float wakeDist;
    public float beforeAttackDelay;
    public float afterAttackDelay;
    public float moveSpeedClose;
    public float moveSpeedFar;

    private bool isDead = false;
    private bool isAttack = false;
    private bool isDamage = false;

    private void Awake()
    {
        stat = GetComponent<Stat>();

        monsterTransform = gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        nvAgent.enabled = false;
        animator = gameObject.GetComponent<Animator>();

        monsterCollider = gameObject.GetComponent<SphereCollider>();
        hitBox = monsterTransform.Find("HitBox").gameObject;
        meshs = GetComponentsInChildren<MeshRenderer>();


    }
    // Start is called before the first frame update
    void Start()
    {
        nvAgent.enabled = true;
        nvAgent.speed = stat.moveSpeed;
        animator.SetFloat("AttackSpeed", stat.attackSpeed);

        SetHpBar();
        
        StartCoroutine("CheckState");
    }

    IEnumerator CheckState()
    {

        while (!isDead)
        {
            yield return null;

            float dist = Vector3.Distance(playerTransform.position, transform.position);
            if (isAttack || isDamage || isDead)
                nvAgent.isStopped = true;
            else
            {
                if (dist <= attackDist)
                {
                    nvAgent.isStopped = true;
                    animator.SetTrigger("Attack");
                    isAttack = true;
                    Invoke("Attack", beforeAttackDelay);
                    Invoke("AttackOut", afterAttackDelay);
                }
                else if(dist <= moveDist)
                {
                    nvAgent.speed = moveSpeedClose;
                    nvAgent.destination = playerTransform.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("isTrace", true);
                }
                else if(dist > moveDist && animator.GetBool("isTrace"))
                {
                    nvAgent.speed = moveSpeedFar;
                    nvAgent.destination = playerTransform.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("isTrace", true);
                }
                else if(dist < wakeDist && !animator.GetBool("isTrace"))
                {
                    nvAgent.speed = 0;
                    nvAgent.destination = playerTransform.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("isTrace", true);
                }
                else
                {
                    nvAgent.speed = 0;
                    nvAgent.destination = playerTransform.position;
                    nvAgent.isStopped = true;
                    animator.SetBool("isTrace", false);
                }
            }
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        yield return 0.2f;
    }

    void Attack()
    {
        hitBox.SetActive(true);
    }
    void AttackOut()
    {
        hitBox.SetActive(false);
        isAttack = false;
    }

    void SetHpBar()
    {
        hpCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        hpBar = Instantiate<GameObject>(hpBarPrefab, hpCanvas.transform);

        slider = hpBar.GetComponent<Slider>();
        slider.value = (float)stat.hp / (float)stat.maxHp;

        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            if (other.gameObject.tag == "PlayerAttack")
            {
                if (!isDamage)
                {
                    stat.hp -= other.gameObject.GetComponent<Damage>().damage;
                    slider.value = (float)stat.hp / (float)stat.maxHp;
                    if (stat.hp > 0)
                    {
                        isDamage = true;
                        StartCoroutine("OnDamage");
                        animator.SetTrigger("Hit");
                    }
                    else
                    {
                        Vector3 temp = transform.position;
                        soul = Instantiate<GameObject>(soul, playerTransform);
                        soul.transform.position = new Vector3(temp.x, 0, temp.z);
                        soul.transform.parent = GameObject.Find("Items").transform;

                        isDead = true;
                        animator.SetTrigger("Die");
                        monsterCollider.isTrigger = true;
                        gameObject.tag = "Dead";
                        Destroy(hpBar, 3f);
                        Destroy(gameObject, 3f);
                    }
                }
            }
        }
    }
}
