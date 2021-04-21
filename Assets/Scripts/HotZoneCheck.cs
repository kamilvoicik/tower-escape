using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private EnemyBehaviour enemyBehaviour;
    private Animator enemyAnimator;

    private void Awake()
    {
        enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
        enemyAnimator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == Consts.PLAYER_LAYER_ID)
        {
            enemyBehaviour.inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == Consts.PLAYER_LAYER_ID)
        {
            enemyBehaviour.inRange = false;
            gameObject.SetActive(false);
            enemyBehaviour.triggerArea.SetActive(true);
            enemyBehaviour.inRange = false;
            enemyBehaviour.FlipEnemy();
            enemyAnimator.SetBool(Consts.RUN_ANIM, false);
        }
    }

}
