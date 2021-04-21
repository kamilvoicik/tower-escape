using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public Config config;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            collision.GetComponent<PlayerCombat>().TakeDamage(config.enemyDamage);
        }
    }
}
