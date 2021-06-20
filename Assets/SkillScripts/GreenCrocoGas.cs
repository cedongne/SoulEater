using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrocoGas : MonoBehaviour
{
    SphereCollider collider;
    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
        collider.enabled = true;
    }
}
