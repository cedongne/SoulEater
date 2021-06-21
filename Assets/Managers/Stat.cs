using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [HideInInspector] public float maxHp;
    public float hp;
    public float damage;
    public float attackSpeed;
    public float moveSpeed;
    public int criticalChance;
    public int coolDown;

    public Souls[] skill;
    public int skillNum;

    private void Awake()
    {
        maxHp = hp;
        skillNum = 0;
    }
}
