using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Souls : MonoBehaviour
{
    public enum Type { ACTIVE, PROJECTILE, SWING, PASSIVE };
    public Type type;
    public string monsterName;

    public float damage;
    public float coolTime;
    public float duration;

    public float beforeDelay;
    public float afterDelay;
    public Sprite skillIcon;

    public bool isCoolDown;
}