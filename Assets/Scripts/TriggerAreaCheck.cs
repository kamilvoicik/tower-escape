using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private EnemyBehaviour enemyBehaviour;

    private void Awake()
    {
        enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Consts.PLAYER_LAYER_ID)
        {
            gameObject.SetActive(false);
            enemyBehaviour.target = collider.gameObject;
            enemyBehaviour.inRange = true;
            enemyBehaviour.hotZone.SetActive(true);
        }
    }
}
