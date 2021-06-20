using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] maps;
    public int mapCount;
    public GameObject bossMap;

    void Start()
    {
        for(int i = 0; i < mapCount; i++)
        {
            LoadNormalMap(i);
        }
        LoadBossMap();
    }

    void Update()
    {
        
    }

    void LoadNormalMap(int i)
    {
        int mapNum = Random.Range(0, maps.Length);
        GameObject map = Instantiate(maps[mapNum], new Vector3 (this.transform.position.x + 100 * i, this.transform.position.y, this.transform.position.z), transform.rotation);
        map.name = $"Map{i}";
        map.transform.parent = this.transform;
    }

    void LoadBossMap()
    {
        GameObject map = Instantiate(bossMap, new Vector3 (this.transform.position.x + 100 * mapCount, this.transform.position.y, this.transform.position.z), transform.rotation);
        map.name = $"BossMap";
        map.transform.parent = this.transform;
    }
}
