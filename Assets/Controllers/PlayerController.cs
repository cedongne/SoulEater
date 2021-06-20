using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private new Camera camera;
    public GameObject other;
    public GameObject enemy;

    Attacks attacks;
    Stat stat;
    //    PassiveSkill passiveSkill;

    float _speed = 5f;

    float hAxis;
    float vAxis;
    bool dDown; // Dash
    bool aDown; // Attack
    bool eDown; // Interaction
    bool s1Down; // Interaction
    bool s2Down; // Interaction
    bool s3Down; // Interaction

    bool isAction;

    bool isAttackReady = true;
    bool isDodgeReady = true;
    bool isDodge;
    bool isAttack = false;


    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;

    float AttackDelay;
    float dodgeDelay;

    float dodgeCooltime = 1.0f;

    public GameObject soulTag;
    List<GameObject> nearItemList;

    private void Awake()
    {
        camera = Camera.main;
        anim = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
        attacks = GetComponentInChildren<Attacks>();

        nearItemList = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        stat.maxHp = 100;
        stat.hp = 100;
        stat.damage = 10;
        stat.moveSpeed = 5;
        stat.attackSpeed = 2;
        stat.criticalChance = 0;
        stat.coolDown = 0;

        attacks.DamageUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        isAction = isAttack || isDodge;
        GetInput();
        Move();
        Turn();
        Dodge();
        Attack();
        Skill();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        aDown = Input.GetButtonDown("Attack");
        dDown = Input.GetButtonDown("Dodge");
        eDown = Input.GetButtonDown("Interaction");

        s1Down = Input.GetButtonDown("Skill1");
        s2Down = Input.GetButtonDown("Skill2");
        s3Down = Input.GetButtonDown("Skill3");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HitBox")
        {
            stat.hp -= other.GetComponentInParent<Stat>().damage;
            Debug.Log("Player Hit : " + stat.hp);
        }
        else if (other.gameObject.tag == "Portal")
        {
            MapGenerator map = other.GetComponentInParent<MapGenerator>();
            Debug.Log(map.transform.gameObject);
            this.transform.position = new Vector3(map.transform.position.x + 100, map.transform.position.y, map.transform.position.z);
        }
        else if (other.gameObject.tag == "Soul")
        {
            // Collider에 들어오는 순서대로 Queue에 넣음
            nearItemList.Add(other.gameObject);
        }
        Debug.Log("Player Hit : " + stat.hp);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soul"))
        {
            var _pickUp = soulTag.GetComponent<InteractionController>();
            _pickUp.targetTr = other.transform;
            soulTag.SetActive(true);

            if (!isAction && eDown && other.gameObject == nearItemList[0])
            {
                soulTag.SetActive(false);
                GameObject getItem = nearItemList[0];
                soulTag.GetComponent<InteractionController>().SkillGet();

                Destroy(getItem);
                nearItemList.Remove(nearItemList[0]);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Soul")
        {
            // Collider에서 빠져나갈 땐 List에서 제거
            for (int index = 0; index < nearItemList.Count; index++)
            {
                if (nearItemList[index] == other.gameObject)
                {
                    nearItemList.RemoveAt(index);
                }
            }
            if (nearItemList.Count == 0)
            {
                soulTag.SetActive(false);
            }
        }
    }
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isAttack || !isAttackReady)
            moveVec = Vector3.zero;

        if (isDodge)
            moveVec = dodgeVec;
        transform.position += moveVec * _speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
    }
    void Turn()
    {
        if (!isAttack)
            transform.LookAt(transform.position + moveVec);
    }
    void Dodge()
    {
        dodgeDelay += Time.deltaTime;
        isDodgeReady = (dodgeDelay > dodgeCooltime);

        if (dDown && !isAction && isDodgeReady)
        {
            dodgeVec = moveVec;
            _speed *= 8;
            anim.SetTrigger("doDodge");
            isDodge = true;
            dodgeDelay = 0f;

            Invoke("DodgeOut", 0.15f);
        }
    }

    void DodgeOut()
    {
        _speed *= 0.125f;
        isDodge = false;
    }

    void Attack()
    {
        AttackDelay += Time.deltaTime;
        isAttackReady = (attacks.attackRate < AttackDelay);

        if (aDown && !isAction && isAttackReady)
        {
            LookMouseCursor();
            attacks.Attack();
            anim.SetTrigger("doShot");
            AttackDelay = 0;
            isAttack = true;
            Invoke("AttackOut", 0.1f);
        }
    }
    void AttackOut()
    {
        isAttack = false;
    }

    void Skill()
    {

        if (s1Down && !isAction)
        {
            if (stat.skill[0].isProjectile)
            {
                LookMouseCursor();
                anim.SetTrigger("doShot");
                AttackDelay = stat.skill[0].beforeDelay;
                isAttack = true;
                Invoke("AttackOut", stat.skill[0].afterDelay);
            }
            attacks.curSoul = stat.skill[0];
            attacks.Use(stat.skill[0].name);
        }
        else if (s2Down && !isAction)
        {
            if (stat.skill[1].isProjectile)
            {
                LookMouseCursor();
                anim.SetTrigger("doShot");
                AttackDelay = stat.skill[1].beforeDelay;
                isAttack = true;
                Invoke("AttackOut", stat.skill[1].afterDelay);
            }
            attacks.curSoul = stat.skill[1];
            attacks.Use(stat.skill[1].name);
        }

        else if (s3Down && !isAction)
        {
            if (stat.skill[2].isProjectile)
            {
                LookMouseCursor();
                anim.SetTrigger("doShot");
                AttackDelay = stat.skill[2].beforeDelay;
                isAttack = true;
                Invoke("AttackOut", stat.skill[2].afterDelay);
            }
            attacks.curSoul = stat.skill[2];
            attacks.Use(stat.skill[2].name);
        }

    }
    void LookMouseCursor()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;

        if(Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
            anim.transform.forward = mouseDir;
            transform.forward = mouseDir;
        }
    }
}