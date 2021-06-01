using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private new Camera camera;
    public GameObject other;
    public GameObject enemy;

    Weapon weapon;
    Stat stat;

    float _speed = 5f;

    float hAxis;
    float vAxis;
    bool dDown; // Dash
    bool aDown; // Attack

    bool isAttackReady = true;
    bool isDodgeReady = true;
    bool isDodge;
    bool isAttack = false;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Vector3 mousePos;
    Vector3 playerPos;
    Vector3 targetPos;

    Animator anim;
    Behaviour behaviour;

    float AttackDelay;
    float dodgeDelay;

    float dodgeCooltime = 1.0f;
    private void Awake()
    {
        camera = Camera.main;
        anim = GetComponentInChildren<Animator>();
        weapon = other.GetComponent<Weapon>();
        behaviour = GetComponent<Behaviour>();
        stat = GetComponent<Stat>();
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

        weapon.DamageUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Dodge();
        Attack();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        aDown = Input.GetButtonDown("Attack");
        dDown = Input.GetButtonDown("Dodge");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HitBox")
        {
            stat.hp -= 10;
        }
        Debug.Log("Player Hit : " + stat.hp);

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

        if (dDown && !isAttack && isDodgeReady)
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
        isAttackReady = (weapon.attackRate < AttackDelay);

        if (!isAttack && aDown && isAttackReady && !isDodge)
        {
            LookMouseCursor();
            weapon.Use();
            anim.SetTrigger("doShot");
            AttackDelay = 0;
            isAttack = true;
            Invoke("AttackOut", 0.4f);
        }
    }
    void AttackOut()
    {
        isAttack = false;
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