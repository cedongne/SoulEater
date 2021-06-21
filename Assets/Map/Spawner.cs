using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] monster;
    public GameObject namedMonster;
    public GameObject bossMonster;
    public int MonsterCount;
    public int namedSpawnRate;
    public bool isBoss;

    int[,] map;

    Vector3 pos = new Vector3();
    public void MonsterSpawn(List<List<MapGenerator.Coord>> spawnableRooms, int width, int height)
    {
        GameObject go = new GameObject { name = "Monsters" };
        go.transform.parent = this.transform;
        int rate = Random.Range(0, 100);
        if (rate < namedSpawnRate)
        {
            while (true)
            {
                int roomNum = Random.Range(0, spawnableRooms.Count);
                int tileNum = Random.Range(0, spawnableRooms[roomNum].Count);

                pos = new Vector3(-width / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileX, 0, -height / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileY);
                if (pos.x < width / 4 && pos.x > -width / 4 && pos.z < height / 4 && pos.z > -height / 4)
                    break;
            }

            GameObject instance;
            instance = Instantiate(namedMonster, new Vector3(this.transform.position.x + pos.x, this.transform.position.y + pos.y, this.transform.position.z + pos.z), transform.rotation);
            instance.transform.parent = go.transform;
        }

        for (int i = 0; i < MonsterCount; i++)
        {
            int monsterIdx = Random.Range(0, monster.Length);
            while (true)
            {
                int roomNum = Random.Range(0, spawnableRooms.Count);
                int tileNum = Random.Range(0, spawnableRooms[roomNum].Count);

                pos = new Vector3(-width / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileX, 0, -height / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileY);
                if (pos.x < width / 4 && pos.x > -width / 4 && pos.z < height / 4 && pos.z > -height / 4)
                    break;
            }

            GameObject instance;
            instance = Instantiate(monster[monsterIdx], new Vector3 (this.transform.position.x + pos.x, this.transform.position.y + pos.y, this.transform.position.z + pos.z), transform.rotation);
            instance.transform.parent = go.transform;
        }
    }

    public void BossSpawn(List<List<MapGenerator.Coord>> spawnableRooms, int width, int height)
    {
        GameObject go = new GameObject { name = "Monsters" };
        go.transform.parent = this.transform;
        while (true)
        {
            int roomNum = Random.Range(0, spawnableRooms.Count);
            int tileNum = Random.Range(0, spawnableRooms[roomNum].Count);

            pos = new Vector3(-width / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileX, 0, -height / 2 + 0.5f + spawnableRooms[roomNum][tileNum].tileY);
            if (pos.x < width / 4 && pos.x > -width / 4 && pos.z < height / 4 && pos.z > -height / 4)
                break;
        }

        GameObject instance;
        instance = Instantiate(bossMonster, new Vector3 (this.transform.position.x + pos.x, this.transform.position.y + pos.y, this.transform.position.z + pos.z), transform.rotation);
        instance.transform.parent = go.transform;
    }

    public bool bossFlag()
    {
        return isBoss;
    }
}
