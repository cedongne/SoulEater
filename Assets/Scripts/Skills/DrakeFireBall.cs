using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrakeFireBall : MonoBehaviour
{
    public GameObject Explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster" || other.tag == "Wall")
        {
            GameObject instance = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            Destroy(instance, 1f);
        }
    }
}
