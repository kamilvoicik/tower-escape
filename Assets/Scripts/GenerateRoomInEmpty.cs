using UnityEngine;

public class GenerateRoomInEmpty : MonoBehaviour
{
    public LayerMask RoomsLayer;
    public LevelGeneration levelGen;
    public GameObject map;

    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, RoomsLayer);

        if (roomDetection == null && levelGen.stopGenerate == true)
        {
            int rand = Random.Range(0, levelGen.rooms.Length - 2);
            GameObject instance = (GameObject)Instantiate(levelGen.rooms[rand], transform.position, Quaternion.identity);
            instance.transform.parent = map.transform;
            Destroy(gameObject);
        }
    }
}
