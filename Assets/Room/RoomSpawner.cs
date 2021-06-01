using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;
    private void Start()
    {
        templates = GetComponent<RoomTemplates>();
        Invoke("spawn", 0.1f);
    }

    private void Invoke()
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {
        if(spawned == false)
        {
            if (openingDirection == 1)
            {
                //when TOP open, need room with a BOTTOM door
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand]);
            }
            else if (openingDirection == 2)
            {
                //when LEFT open, need room with a RIGHT door
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand]);
            }
            else if (openingDirection == 3)
            {
                //when RIGHT open, need room with a LEFT door
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand]);
            }
            else if (openingDirection == 4)
            {
                //when BOTTOM open, need room with a TOP door
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand]);
            }
            spawned = true;
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RoomSpawner>().spawned == true)
            Destroy(gameObject);
    }
}
