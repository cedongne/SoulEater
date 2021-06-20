using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrakeFireBall : MonoBehaviour
{
    GameObject playerWeapon;
    Transform weaponTransform;

    public Souls DrakeSoul;
    public GameObject FireBall;
    public GameObject Explosion;
    public GameObject HitEffect;

    GameObject instance;
    private void Awake()
    {
        playerWeapon = GameObject.Find("Weapon").gameObject;
        weaponTransform = playerWeapon.transform;
    }

    public void Drake_FireBall()
    {
        instance = Instantiate(FireBall, weaponTransform);
        Rigidbody instanceRigid = instance.GetComponent<Rigidbody>();
        instanceRigid.velocity = weaponTransform.forward * 30;
    }
    private void OnTriggerEnter(Collider other)
    {
        instance = Instantiate(Explosion, weaponTransform);
        Destroy(instance);;
    }
}
