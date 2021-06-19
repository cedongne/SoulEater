using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "New item/item")]
public class Soul : ScriptableObject
{
    public enum Type { Soul, Equip, Coin };
    public Type type;

    public int value;


    public GameObject soulPrefab;

    private void Start()
    {
        type = Type.Soul;
    }

    void ShowToolTip(Soul skill)
    {

    }


}
