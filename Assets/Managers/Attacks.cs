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
    private GameObject instance;

    public Souls GreenCrocoSoul;
    public GameObject GreenCrocoGas;
    public GameObject GreenCrocoHitEffect;

    public Souls DrakeSoul;
    public GameObject DrakeFireBall;
    public GameObject DrakeExplosion;
    public GameObject DrakeHitEffect;

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
        if (soul == "Green Crocodile")
        {
            GreenCroco_PoisonGas();
        }
        else if (soul == "Drake")
        {
            Drake_FireBall();
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
        Destroy(instance, GreenCrocoSoul.duration);
    }
    public void Drake_FireBall()
    {
        instance = Instantiate(DrakeFireBall, transform.position, transform.rotation);
        Rigidbody instanceRigid = instance.GetComponent<Rigidbody>();
        instanceRigid.velocity = transform.forward * 30;
    }

    public void DamageUpdate()
    {
        playerStat = GetComponentInParent<Stat>();
        damage = playerStat.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(curSoul.isProjectile);
        if (curSoul.isProjectile)
        {
            Destroy(instance);
            instance = Instantiate(DrakeExplosion, transform.position, transform.rotation);
        }
    }
}
