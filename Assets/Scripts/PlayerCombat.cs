using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool isAlive = true;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [SerializeField] int currentHealth = 200;
    [SerializeField] public float attackRange = 0.35f;
    [SerializeField] public int attackDamage = 30;

    private Animator playerAnimator;
    private Rigidbody2D playerRigidBody2D;
    private CapsuleCollider2D playerCapsuleBody;
    private Config config;
    private ScoreLivesCounter counter;
    private BoxCollider2D playerCollider;

    private void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleBody = GetComponent<CapsuleCollider2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        counter = FindObjectOfType<ScoreLivesCounter>();
        config = FindObjectOfType<Config>();
    }

    private void Update()
    {
        Attack();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        playerAnimator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            playerAnimator.SetTrigger("Attacking");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyBehaviour>().TakeDamage(attackDamage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Consts.HAZARD_LAYER_ID)
        {
            TakeDamage(config.spikesDamage);
        }
        if (collider.gameObject.layer == Consts.INTERACTIVE_LAYER_ID)
        {
            counter.AddToScore();
            Destroy(collider.gameObject);
        }
    }

    private void Die()
    {
        playerAnimator.SetTrigger("Die");
        isAlive = false;
        playerRigidBody2D.bodyType = RigidbodyType2D.Static;
        playerCapsuleBody.enabled = false;
        playerCollider.enabled = false;

        StartCoroutine(WaitUntillReload(2));
    }

    IEnumerator WaitUntillReload(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
