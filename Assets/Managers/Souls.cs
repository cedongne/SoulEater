using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Souls : MonoBehaviour
{
    public enum Type { ACTIVE, PASSIVE };
    public bool isProjectile;
    public Type type;
    public string name;

    public float damage;
    public float coolTime;
    public float duration;

    public float beforeDelay;
    public float afterDelay;
    public Sprite skillIcon;
}