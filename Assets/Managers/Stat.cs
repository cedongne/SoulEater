using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [HideInInspector] public int maxHp;
    public int hp;
    public int damage;
    public float attackSpeed;
    public int moveSpeed;
    public int criticalChance;
    public int coolDown;

    private void Awake()
    {
        maxHp = hp;
    }
}
