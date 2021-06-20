using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrocoGas : MonoBehaviour
{
    SphereCollider collider;
    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        InvokeRepeating("setEnable", 0.05f, 1f);
        InvokeRepeating("setDisable", 0f, 1f);
    }

    void setEnable()
    {
        collider.enabled = true;
    }
    void setDisable()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
