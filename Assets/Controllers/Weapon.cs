using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Stat playerStat;
    public int damage;
    public float attackRate = 0.6f;

    public GameObject bullet;
    public Transform bulletPos;
    
    public GameObject waveEffect;


    private void Awake()
    {
    }
    public void DamageUpdate()
    {
        playerStat = GetComponentInParent<Stat>();
        damage = playerStat.damage;
    }
    public void Use()
    {
        StopCoroutine("Fire");
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        GameObject instantWave = Instantiate(waveEffect, bulletPos.position, bulletPos.rotation);
        Destroy(instantWave, 0.5f);
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null;
    }
}
