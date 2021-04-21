using UnityEngine;

public class Config : MonoBehaviour
{
    [SerializeField] public int playerLives = 3;
    [SerializeField] public int pointsForCoin = 1;
    [SerializeField] public int spikesDamage = 20;
    [SerializeField] public float LevelLoadDelay = 1f;
    [SerializeField] public int enemyDamage = 20;
}
