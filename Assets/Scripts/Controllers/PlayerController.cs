using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Camera camera;
    public GameObject other;
    public GameObject enemy;

    public Vector3 cursorPos;

    Attacks attacks;
    Stat stat;
    Passives passives;
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

    bool isDead = false;

    bool isAction;

    bool isAttackReady = true;
    bool isDodgeReady = true;
    bool isDodge;
    bool isAttack = false;
    bool isSkill1Ready = true;
    bool isSkill2Ready = true;
    bool isSkill3Ready = true;

    float skill1Time;
    float skill2Time;
    float skill3Time;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;

    float AttackDelay;
    float dodgeDelay;

    float dodgeCooltime = 1.0f;

    public GameObject soulTag;
    public Text InteractionText;
    List<GameObject> nearItemList;

    UIManager uiManager;

    Ray ray;
    private void Awake()
    {
        camera = Camera.main;
        anim = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
        attacks = GetComponentInChildren<Attacks>();
        passives = GetComponentInChildren<Passives>();

        nearItemList = new List<GameObject>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
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

        skill1Time = 10;
        skill2Time = 10;
        skill3Time = 10;

        attacks.DamageUpdate();
        transform.Translate(new Vector3(0, 0, -15));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            isAction = isAttack || isDodge;
            GetInput();
            Move();
            Turn();
            Dodge();
            Attack();
            Skill();
        }
        else if(isDead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
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
        if (!isDead)
        {
            if (other.gameObject.tag == "HitBox")
            {
                stat.hp -= other.GetComponentInParent<Stat>().damage;
                Debug.Log("Player Hit : " + stat.hp);
            }
            else if (other.gameObject.tag == "SkillHitBox")
            {
                stat.hp -= other.GetComponent<SkillStat>().damage;
                Debug.Log("Player Hit : " + stat.hp);
            }
            else if (other.gameObject.tag == "Portal")
            {
                GameObject mapSpawner = other.transform.parent.GetComponentInParent<MapSpawner>().gameObject;
                int mapNum = other.GetComponentInParent<MeshGenerator>().MapNum;
                int spawnDir = other.GetComponentInParent<MeshGenerator>().nextSpawnDir;

                GameObject map = mapSpawner.transform.GetChild(mapNum + 1).gameObject;
                Debug.Log("Next Map: " + map.name);
                transform.position = map.GetComponent<MeshGenerator>().getSpawnPos(spawnDir);
            }
            else if (other.gameObject.tag == "Soul")
            {
                // Collider에 들어오는 순서대로 List에 넣음
                nearItemList.Add(other.gameObject);
            }
            if (stat.hp <= 0)
            {
                isDead = true;
                anim.SetTrigger("Die");
                gameObject.tag = "Dead";
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soul"))
        {
            var _pickUp = soulTag.GetComponent<InteractionController>();
            _pickUp.targetTr = other.transform;
            soulTag.GetComponent<Image>().enabled = true;
            InteractionText.gameObject.SetActive(true);

            if (!isAction && eDown && other.gameObject == nearItemList[0])
            {
                soulTag.GetComponent<Image>().enabled = false;
                InteractionText.gameObject.SetActive(false);
                GameObject getItem = nearItemList[0];
                Souls currSoul = soulTag.GetComponent<InteractionController>().SkillGet(stat.skillNum);
                if (currSoul != null && currSoul.type == Souls.Type.PASSIVE)
                {
                    Debug.Log("Passive On");
                    passives.turnOnPassive(currSoul.monsterName);
                }

                getItem.SetActive(false);
                Destroy(getItem, 100f);
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
                soulTag.GetComponent<Image>().enabled = false;
                InteractionText.gameObject.SetActive(false);
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
            if (!moveVec.Equals(Vector3.zero))
            {
                dodgeVec = moveVec;
                _speed *= 8;

                anim.SetTrigger("doDodge");

                isDodge = true;
                dodgeDelay = 0f;

                Invoke("DodgeOut", 0.15f);
            }
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
        skill1Time += Time.deltaTime;
        skill2Time += Time.deltaTime;
        skill3Time += Time.deltaTime;
        if (stat.skill[0] != null)
        {
            isSkill1Ready = skill1Time >= stat.skill[0].coolTime;

            if (s1Down && !isAction && isSkill1Ready)
            {
                if (stat.skill[0].type != Souls.Type.PASSIVE)
                {
                    if (stat.skill[0].type == Souls.Type.PROJECTILE)
                        anim.SetTrigger("doShot");
                    else if (stat.skill[0].type == Souls.Type.SWING)
                        anim.SetTrigger("doSwing");
                    LookMouseCursor();
                    AttackDelay = stat.skill[0].beforeDelay;
                    isAttack = true;
                    Invoke("AttackOut", stat.skill[0].afterDelay);
                }
                attacks.curSoul = stat.skill[0];
                attacks.Use(stat.skill[0].monsterName);
                skill1Time = 0;
            }
        }
        if (stat.skill[1] != null)
        {
            isSkill2Ready = skill2Time >= stat.skill[1].coolTime;
            if (s2Down && !isAction && isSkill2Ready)
            {
                if (stat.skill[1].type != Souls.Type.PASSIVE)
                {
                    if (stat.skill[1].type == Souls.Type.PROJECTILE)
                        anim.SetTrigger("doShot");
                    else if (stat.skill[1].type == Souls.Type.SWING)
                        anim.SetTrigger("doSwing");
                    LookMouseCursor();
                    AttackDelay = stat.skill[1].beforeDelay;
                    isAttack = true;
                    Invoke("AttackOut", stat.skill[1].afterDelay);
                }
                attacks.curSoul = stat.skill[1];
                attacks.Use(stat.skill[1].monsterName);
                skill2Time = 0;
            }
        }

        if (stat.skill[2] != null)
        {
            isSkill3Ready = skill3Time >= stat.skill[2].coolTime;
            if (s3Down && !isAction && isSkill3Ready)
            {
                if (stat.skill[2].type != Souls.Type.PASSIVE)
                {
                    if (stat.skill[2].type == Souls.Type.PROJECTILE)
                        anim.SetTrigger("doShot");
                    else if (stat.skill[2].type == Souls.Type.SWING)
                        anim.SetTrigger("doSwing");
                    LookMouseCursor();
                    AttackDelay = stat.skill[2].beforeDelay;
                    isAttack = true;
                    Invoke("AttackOut", stat.skill[2].afterDelay);
                }
                attacks.curSoul = stat.skill[2];
                attacks.Use(stat.skill[2].monsterName);
                skill3Time = 0;
            }
        }
    }
    void LookMouseCursor()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hitResult;
        Debug.DrawRay(transform.position, ray.direction * 15, Color.red);
        hitResult = Physics.RaycastAll(ray);
        if (hitResult.Length > 0)
        {
            int index;
            for(index = 0; !hitResult[index].transform.CompareTag("Plane"); index++) { }

            Vector3 mouseDir = new Vector3(hitResult[index].point.x, transform.position.y, hitResult[index].point.z) - transform.position;
            cursorPos = new Vector3(hitResult[index].point.x, transform.position.y, hitResult[index].point.z);
            anim.transform.forward = mouseDir;
            transform.forward = mouseDir;
        }
    }
}