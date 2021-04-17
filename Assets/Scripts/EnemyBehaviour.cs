using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] float currentHealth = 100f;

    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    #endregion


    #region Private Variables
    private RaycastHit2D hit;
    private GameObject target;
    private Animator enemyAnimator;
    private Rigidbody2D enemyRigidBody2D;
    private float distance; //Store the distance b/w enemy and player
    private float intTimer;
    private bool attackMode;
    private bool inRange; //Check if Player is in range
    private bool cooling; //Check if Enemy is cooling after attack
    private bool isAlive = true;
    #endregion



    void Awake()
    {
        intTimer = timer; //Store initial value timer
        enemyAnimator = GetComponent<Animator>();
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        InvokeRepeating("FlipEnemy", 3f, 5f);
    }

    void Update()
    {
        if (inRange)
        {
            if(transform.localScale.x > 0)
            {
                hit = Physics2D.Raycast(rayCast.position, Vector2.right, rayCastLength, raycastMask);
                RaycastDebugger();
            }else if(transform.localScale.x < 0)
            {
                hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
            }
            
        }

        if(hit.collider != null)
        {
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            inRange = false;
        }

        if(inRange == false)
        { 
            enemyAnimator.SetBool("Running", false);
            StopAttack();
            Debug.Log("Stop Running");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            target = collision.gameObject;
            inRange = true;
        }
    }

    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if(distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            enemyAnimator.SetBool("Attacking", false);
        }
    }

    private void Move()
    {
        enemyAnimator.SetBool("Running", true);

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            Debug.Log("Moving");
        }
    }

    private void Attack()
    {
        timer = intTimer; //Reset Timer when player enter Attack Range
        attackMode = true;

        enemyAnimator.SetBool("Running", false);
        enemyAnimator.SetBool("Attacking", true);

    }

    private void Cooldown()
    {
        timer -= Time.deltaTime;

        if(timer <=0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    private void StopAttack()
    {
        cooling = false;
        attackMode = false;
        enemyAnimator.SetBool("Attacking", false);
    }

    private void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.right * rayCastLength, Color.red);
        }
        else if(attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.right * rayCastLength, Color.green);
        }
    }
    private void FlipEnemy()
    {
        if (inRange == false && isAlive == true)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, 1f);
        }
    }
    private void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        enemyRigidBody2D.bodyType = RigidbodyType2D.Static;

        Debug.Log("Enemy Died");
        enemyAnimator.SetBool("Die", true);
        isAlive = false;

        
        this.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        StopAttack();
        currentHealth -= damage;
        enemyAnimator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }
}
