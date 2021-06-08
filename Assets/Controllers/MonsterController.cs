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
    Transform mainCamera;
    NavMeshAgent nvAgent;
    Animator animator;

    SphereCollider monsterCollider;
    GameObject hitBox;
    MeshRenderer[] meshs;

    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);

    GameObject hpBar;
    Canvas hpCanvas;
    Slider slider;

    public float attackDist = 4.0f;
    public float moveDist = 10.0f;
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

        mainCamera = Camera.main.transform;
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
                else
                {
                    nvAgent.speed = moveSpeedFar;
                    nvAgent.destination = playerTransform.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("isTrace", true);
                }
            }
        }
    }

    IEnumerator OnDamage()
    {
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        yield return new WaitForSeconds(0.2f);
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
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
                    Weapon weapon = FindObjectOfType<Weapon>();
                    stat.hp -= weapon.damage;
                    slider.value = (float)stat.hp / (float)stat.maxHp;
                    if (stat.hp > 0)
                    {
                        isDamage = true;
                        StartCoroutine("OnDamage");
                        animator.SetTrigger("Hit");
                    }
                    else
                    {
                        isDead = true;
                        animator.SetTrigger("Die");
                        monsterCollider.isTrigger = true;
                        Destroy(hpBar, 3f);
                        Destroy(gameObject, 3f);
                    }
                }
            }
        }
    }
}
