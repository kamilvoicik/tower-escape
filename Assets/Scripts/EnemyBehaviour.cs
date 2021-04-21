using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region SerializedField
    [SerializeField] float currentHealth = 100f;
    #endregion

    #region Public Variables
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public bool inRange;  //Check if Player is in range
    public GameObject target;
    public GameObject hotZone;
    public GameObject triggerArea;
    #endregion

    #region Private Variables
    private Animator enemyAnimator;
    private Rigidbody2D enemyRigidBody2D;
    private Collider2D enemyCollider2D;
    private Collider2D enemySword;
    private float distanceToPlayer; 
    private bool isAlive = true;
    #endregion

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyCollider2D = GetComponent<Collider2D>();
        enemySword = GetComponentInChildren<CircleCollider2D>();
        InvokeRepeating("FlipEnemyInIdle", 3f, 5f);
    }

    private void Update()
    {
        if (inRange)
        {
            AttackPlayerInRange();
        }
    }

    public void FlipEnemy()
    {
        if (isAlive)
        {
            if (transform.position.x < target.transform.position.x)
            {
                transform.localScale = new Vector2(1f, 1f);
            }
            else
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
        }
        
    }
    public void TakeDamage(int damage)
    {
        StopAttack();
        currentHealth -= damage;
        enemyAnimator.SetTrigger(Consts.HURT_ANIM);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void AttackPlayerInRange()
    {
        distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

        if (distanceToPlayer > attackDistance)
        {
            Move();
            StopAttack();
        }
        else
        {
            Attack();
        }
    }

    private void Move()
    {
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(Consts.ATTACK_ANIM))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            enemyAnimator.SetBool(Consts.RUN_ANIM, true);
        }
    }

    private void Attack()
    {
        enemyAnimator.SetBool(Consts.RUN_ANIM, false);
        enemyAnimator.SetBool(Consts.ATTACK_ANIM, true);
    }

    private void StopAttack()
    {
        enemyAnimator.SetBool(Consts.ATTACK_ANIM, false);
    }


    private void FlipEnemyInIdle()
    {
        if (inRange == false && isAlive == true)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, 1f);
        }
    }

    private void Die()
    {
        enemyCollider2D.enabled = false;
        enemySword.enabled = false;
        enemyRigidBody2D.bodyType = RigidbodyType2D.Static;
        enemyAnimator.SetTrigger(Consts.DIE_ANIM);
        triggerArea.SetActive(false);
        hotZone.SetActive(false);
        isAlive = false;

        this.enabled = false;
    }
}
