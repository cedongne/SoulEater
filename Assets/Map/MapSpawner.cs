using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] maps;

    void Start()
    {
        LoadMap();
    }

    void Update()
    {
        
    }

    void LoadMap()
    {
        int mapNum = Random.Range(0, maps.Length);
        GameObject map = Instantiate(maps[mapNum], this.transform.position, Quaternion.identity);
        map.transform.parent = this.transform;
    }
}
