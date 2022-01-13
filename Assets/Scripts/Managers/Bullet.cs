using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public GameObject hitEffect;

    private void Start()
    {
        Invoke("DestroyTimeOut", 2.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall" || other.gameObject.tag == "Monster")
        {
            GameObject instantHitEffect = Instantiate(hitEffect, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
            Destroy(instantHitEffect, 1.0f);
        }
    }

    private void DestroyTimeOut()
    {
        Destroy(gameObject);
    }

}
