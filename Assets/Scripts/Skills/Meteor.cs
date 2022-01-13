using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public GameObject Explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Plane" || other.gameObject.tag == "Cave")
        {
            GameObject instance = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            Destroy(instance, 1f);
        }
    }
}
