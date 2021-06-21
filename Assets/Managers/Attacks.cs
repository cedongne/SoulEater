using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attacks : MonoBehaviour
{
    Stat playerStat;
    public int damage;

    public float attackDamage = 10f;
    public float attackRate = 0.6f;

    public GameObject bullet;
    public Transform bulletPos;

    public GameObject waveEffect;

    public Souls curSoul;
    public GameObject CurExplosion;
    private GameObject instance;

    public Souls GreenCrocoSoul;
    public GameObject GreenCrocoGas;
    public GameObject GreenCrocoHitEffect;

    public Souls DrakeSoul;
    public GameObject DrakeFireBall;
    public GameObject DrakeExplosion;
    public GameObject DrakeHitEffect;

    public Souls GruntSoul;
    public GameObject GruntRock;
    public GameObject GruntExplosion;

    public Souls FireDragonSoul;
    public GameObject FireDragonMeteor;
    public GameObject MeteorExplosion;

    private void Awake()
    {
    }

    public void Attack()
    {
        StopCoroutine("Fire");
        StartCoroutine("Fire");
    }
    public void Use(string soul)
    {
        Debug.Log(soul);
        if (soul == "Green Crocodile")
        {
            Invoke("GreenCroco_PoisonGas", curSoul.beforeDelay);
        }
        else if (soul == "Drake")
        {
            Invoke("Drake_FireBall", curSoul.beforeDelay);
        }
        else if( soul == "Grunt")
        {
            Invoke("Grunt_Slap", curSoul.beforeDelay);
        }
        else if(soul == "Fire Dragon")
        {
            Invoke("FireDragon_Meteor", curSoul.beforeDelay);
        }
    }
    IEnumerator Fire()
    {
        GameObject instantWave = Instantiate(waveEffect, transform.position, transform.rotation);
        Destroy(instantWave, 0.5f);

        GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = transform.forward * 50;
        yield return null;
    }
    public void GreenCroco_PoisonGas()
    {
        instance = Instantiate(GreenCrocoGas, transform.position, transform.rotation);
        Invoke("GasOff", GreenCrocoSoul.duration - 0.1f);
        Destroy(instance, GreenCrocoSoul.duration);
    }
    void GasOff()
    {
        Debug.Log("SET");
        instance.GetComponent<SphereCollider>().center = new Vector3(0, 0, 300f);
    }
    public void Drake_FireBall()
    {
        instance = Instantiate(DrakeFireBall, transform.position, transform.rotation);
        instance.transform.Rotate(new Vector3(0, 30, 0));
        Rigidbody instanceRigid = instance.GetComponent<Rigidbody>();
        instanceRigid.velocity = instance.transform.forward * 30;

        GameObject instance2 = Instantiate(DrakeFireBall, transform.position, transform.rotation);
        Rigidbody instanceRigid2 = instance2.GetComponent<Rigidbody>();
        instanceRigid2.velocity = instance2.transform.forward * 30;

        GameObject instance3 = Instantiate(DrakeFireBall, transform.position, transform.rotation);
        instance3.transform.Rotate(new Vector3(0, -30, 0));
        Rigidbody instanceRigid3 = instance3.GetComponent<Rigidbody>();
        instanceRigid3.velocity = instance3.transform.forward * 30;

        CurExplosion = DrakeExplosion;
    }
    public void Grunt_Slap()
    {
        instance = Instantiate(GruntRock, transform.position, transform.rotation);
        Rigidbody instanceRigid = instance.GetComponent<Rigidbody>();
        instanceRigid.velocity = transform.forward * 20;
        CurExplosion = GruntExplosion;
    }
    public void FireDragon_Meteor()
    {
        instance = Instantiate(FireDragonMeteor, new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z), transform.rotation);
        Vector3 meteorDir = GameObject.Find("Player").GetComponent<PlayerController>().cursorPos - instance.transform.position;
        Rigidbody instanceRigid = instance.GetComponent<Rigidbody>();
        instanceRigid.velocity = meteorDir.normalized * 20;
        CurExplosion = MeteorExplosion;
    }

    public void DamageUpdate()
    {
        playerStat = GetComponentInParent<Stat>();
        damage = playerStat.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (curSoul.type == Souls.Type.PROJECTILE)
        {
            Destroy(instance);
            instance = Instantiate(CurExplosion, transform.position, transform.rotation);
        }
    }
}
