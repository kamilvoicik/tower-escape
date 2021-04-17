using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    #region Public Variables
    public Transform[] startingPositions;
    public GameObject[] rooms; // 0 --> LR, 1 --> LRB, 2 --> LRT, 3 --> LRTB
    public GameObject map;
    public GameObject player;
    public GameObject escapeDoor;
    public LayerMask room;

    public float moveAmount;
    public float startTimeBtwRoom = 1f;
    public float minX;
    public float maxX;
    public float maxY;
    public bool stopGenerate;
    public int upCounter = 0;
    #endregion

    #region Private Variables
    private float timeBtwRoom;
    private int direction;
    #endregion


    void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        GameObject instance = (GameObject)Instantiate(rooms[0], transform.position, Quaternion.identity);
        instance.transform.parent = map.transform;

        Instantiate(player, transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if(timeBtwRoom <= 0 && stopGenerate == false)
        {
            NextRoomDirection();
            timeBtwRoom = startTimeBtwRoom;
        }else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void NextRoomDirection()
    {
        if(direction == 1 || direction == 2) // RIGHT
        {
            if(transform.position.x < maxX)
            {
                upCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length); // Every room type
                GameObject instance = (GameObject)Instantiate(rooms[rand], transform.position, Quaternion.identity);
                instance.transform.parent = map.transform;

                direction = Random.Range(1, 6);
                if(direction == 3) // More chance to go right instead of going one left
                {
                    direction = 2;
                }else if (direction == 4) // More chance to go up instead of going one left
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5; // top
            }
            
        }else if (direction == 3 || direction == 4) // LEFT
        {
            if (transform.position.x > minX)
            {
                upCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);// Every room type
                GameObject instance = (GameObject)Instantiate(rooms[rand], transform.position, Quaternion.identity);
                instance.transform.parent = map.transform;

                direction = Random.Range(3, 6); // Going only left
            }
            else
            {
                direction = 5;
            }
                
        }
        else if (direction == 5) // UP
        {

            upCounter++;

            if(transform.position.y < maxY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);

                if (roomDetection.GetComponent<RoomType>().type != 2 && roomDetection.GetComponent<RoomType>().type != 3) // Makes sure that previous room has a TOP opening
                {
                    if (upCounter >=2) // If it goes double UP it makes previous one with every way opening
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        GameObject instance1 = (GameObject)Instantiate(rooms[3], transform.position, Quaternion.identity);
                        instance1.transform.parent = map.transform;
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        int randBottomRoom = Random.Range(2, 4);
                        GameObject instance2 = (GameObject)Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                        instance2.transform.parent = map.transform;

                    }

                    
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
                transform.position = newPos;

                int rand = Random.Range(1,4); // Makes room with bottom opening
                if(rand == 2)
                {
                    rand = 3;
                }

                GameObject instance = (GameObject)Instantiate(rooms[rand], transform.position, Quaternion.identity);
                instance.transform.parent = map.transform;

                direction = Random.Range(1, 6);

            }else
            {
                stopGenerate = true;
                Instantiate(escapeDoor, transform.position, Quaternion.identity);
            }
        }
    }
}
