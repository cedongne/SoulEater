using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        Invoke("DestroyTimeOut", 2.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall" || other.gameObject.tag == "Monster")
        {
            Destroy(gameObject);
        }
    }

    private void DestroyTimeOut()
    {
        Destroy(gameObject);
    }

}
