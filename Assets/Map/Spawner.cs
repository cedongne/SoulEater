using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] monster;
    public int MonsterCount;

    int[,] map;

    Vector3 pos = new Vector3();

    int monsterNum = 2;
    public void MonsterSpawn(List<List<MapGenerator.Coord>> spawnableRooms, int width, int height)
    {
        GameObject go = new GameObject { name = "Monsters" };
        go.transform.parent = this.transform;

        for (int i = 0; i < MonsterCount; i++)
        {
            int monsterIdx = Random.Range(0, monster.Length);
            
            int roomNum = Random.Range(0, spawnableRooms.Count);
            int tileNum = Random.Range(0, spawnableRooms[roomNum].Count);

            pos = new Vector3(-width / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileX, 0, -height / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileY);

            GameObject instance;
            instance = Instantiate(monster[monsterIdx], pos, Quaternion.identity);
            instance.transform.parent = go.transform;
        }
    }
}
