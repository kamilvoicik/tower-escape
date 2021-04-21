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
    private int direction;
    #endregion


    void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        GenerateRoom(0);
        Instantiate(player, transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if(stopGenerate == false)
        {
            NextRoomDirection();
        }
    }

    private void NextRoomDirection()
    {
        if(direction == 1 || direction == 2) // RIGHT
        {
            if(transform.position.x < maxX)
            {
                upCounter = 0;

                NewPositionToGenerateHorizontal(moveAmount);
                GenerateRandomRoom();

                direction = RandomFromArray(new int[] { 1, 2, 5 });
            }
            else
            {
                direction = 5; 
            }
            
        }else if (direction == 3 || direction == 4) // LEFT
        {
            if (transform.position.x > minX)
            {
                upCounter = 0;
                NewPositionToGenerateHorizontal(-moveAmount);
                GenerateRandomRoom();

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
                
        }
        else // UP
        {
            upCounter++;

            if(transform.position.y < maxY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                RoomType roomType = roomDetection.GetComponent<RoomType>();

                if (roomType.type != 2 && roomType.type != 3)
                {
                    if (upCounter >=2)
                    {
                        roomType.RoomDestruction();
                        GenerateRoom(3);
                    }
                    else
                    {
                        roomType.RoomDestruction();
                        GenerateRandomRoomFromTypes(new int[] { 2, 3 });
                    }
                }

                NewPositionToGenerateVectical(moveAmount);
                GenerateRandomRoomFromTypes(new int[] { 1, 3});
                direction = Random.Range(1, 6);

            }else
            {
                stopGenerate = true;
                Instantiate(escapeDoor, transform.position, Quaternion.identity);
            }
        }
    }

    private void GenerateRoom(int numberOfRoom)
    {
        GameObject instance = (GameObject)Instantiate(rooms[numberOfRoom], transform.position, Quaternion.identity);
        instance.transform.parent = map.transform;
    }

    private void GenerateRandomRoom()
    {
        int rand = Random.Range(0, rooms.Length);
        GameObject instance = (GameObject)Instantiate(rooms[rand], transform.position, Quaternion.identity);
        instance.transform.parent = map.transform;
    }

    private void GenerateRandomRoomFromTypes(int[] numbersOfRooms)
    {
        int rand = RandomFromArray(numbersOfRooms);
        GameObject instance = (GameObject)Instantiate(rooms[rand], transform.position, Quaternion.identity);
        instance.transform.parent = map.transform;
    }

    private void NewPositionToGenerateHorizontal(float moveAmount)
    {
        Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
        transform.position = newPos;
    }

    private void NewPositionToGenerateVectical(float moveAmount)
    {
        Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
        transform.position = newPos;
    }

    public static int RandomFromArray(int[] x)
    {
        System.Random random = new System.Random();
        int index = random.Next(x.Length);
        return x[index];
    }
}
