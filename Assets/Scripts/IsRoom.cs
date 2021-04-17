using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRoom : MonoBehaviour
{
    public LayerMask IsRoomLayer;
    public LevelGeneration levelGen;
    public GameObject map;

    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, IsRoomLayer);

        if (roomDetection == null && levelGen.stopGenerate == true)// Checks if there is a room already and all path rooms are generated
        {
            int rand = Random.Range(0, levelGen.rooms.Length);
            GameObject instance = (GameObject)Instantiate(levelGen.rooms[rand], transform.position, Quaternion.identity);
            instance.transform.parent = map.transform;
            Destroy(gameObject); // It makes sure that rooms are spawns only ones
        }
    }
}
