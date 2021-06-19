using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Souls : MonoBehaviour
{
    public string name;
    public float damage;
    public float coolTime;
    public enum Type { ACTIVE, PASSIVE };
    public Type type;

    public GameObject skillEffect;
    public Sprite skillIcon;

    public GameObject player;

    public int order;

    private void Awake()
    {
        player = GameObject.Find("Player").gameObject;
        type = Type.ACTIVE;
    }

    public void Use()
    {
        Debug.Log("Skill1Use");
        GameObject skillUse = Instantiate(skillEffect, player.transform.position, player.transform.rotation);
        Destroy(skillUse, 2f);
    }
}
