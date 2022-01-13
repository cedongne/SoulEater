using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrocoGas : MonoBehaviour
{
    SphereCollider SkillCollider;
    private void Start()
    {
        SkillCollider = GetComponent<SphereCollider>();
        SkillCollider.enabled = false;
        SkillCollider.enabled = true;
    }
}
