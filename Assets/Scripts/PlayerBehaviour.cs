using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] float playerSpeed;
    [SerializeField] float playerJumpForce;
    [SerializeField] float playerClimbSpeed;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] int currentHealth = 200;
    [SerializeField] public float attackRange = 0.35f;
    [SerializeField] public int attackDamage = 30;
    #endregion

    #region Public Variables
    public Transform attackPoint;
    public LayerMask enemyLayers;
    #endregion

    #region Private Variables
    private Rigidbody2D playerRigidBody2D;
    private Animator playerAnimator;
    private CapsuleCollider2D playerCapsuleBody;
    private float gravityScaleAtStart;
    private bool isAlive = true;
    #endregion

    void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleBody = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = playerRigidBody2D.gravityScale;
    }


    void Update()
    {
        if (!isAlive) { return; } // Stops player to move

        Attack();
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
    }

    private void Run()
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player Attacking")) 
        {
            float axisValue = Input.GetAxisRaw("Horizontal");  // .GetAxis value of input axis between -1 to 1 / .GetAxisRaw makes it -1, 0 or 1 / instance responce
            Debug.Log(axisValue);
            Vector2 playerVelocity = new Vector2(axisValue * playerSpeed, playerRigidBody2D.velocity.y);
            playerRigidBody2D.velocity = playerVelocity;

            bool playerHorizontalMove = Mathf.Abs(playerRigidBody2D.velocity.x) > Mathf.Epsilon; // Checks if player is moving at all
            playerAnimator.SetBool("Running", playerHorizontalMove); // Makes animation bool to true and plays animations
        }
    }

    private void FlipSprite()
    {
        bool playerHorizontalMove = Mathf.Abs(playerRigidBody2D.velocity.x) > Mathf.Epsilon;

        if (playerHorizontalMove)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody2D.velocity.x), 1f); //Flips whole playerObject in moving direction
        }
    }

    private void ClimbLadder()
    {
        if (playerCapsuleBody.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Debug.Log("Ladder is touched");
            float axisValue = Input.GetAxisRaw("Vertical");
            Vector2 playerClimbVelocity = new Vector2(playerRigidBody2D.velocity.x, axisValue * playerClimbSpeed);
            playerRigidBody2D.velocity = playerClimbVelocity;

            playerRigidBody2D.gravityScale = 0f;
            bool playerClimbing = Mathf.Abs(playerRigidBody2D.velocity.y) > Mathf.Epsilon;
            //playerAnimator.SetBool("Climbing", playerClimbing);
        }else
        {
            playerRigidBody2D.gravityScale = gravityScaleAtStart;
            //playerAnimator.SetBool("Climbing", false);
            return;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, playerJumpForce);
            playerRigidBody2D.AddForce(jumpVelocityToAdd, ForceMode2D.Impulse);
        }

        if (isGrounded())
        {
            playerAnimator.SetBool("Jumping", false);
        }
        else
        {
            playerAnimator.SetBool("Jumping", true);
        }
    }
    
    private bool isGrounded()
    {
        RaycastHit2D isTouchingGround = Physics2D.CapsuleCast(playerCapsuleBody.bounds.center, playerCapsuleBody.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.1f, groundLayerMask);

        return isTouchingGround.collider != null;
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") && playerRigidBody2D.velocity == Vector2.zero)
        {
            playerAnimator.SetTrigger("Attacking");
            

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            Debug.Log(hitEnemies);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyBehaviour>().TakeDamage(attackDamage);
                Debug.Log(enemy.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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

    private void Die()
    {
        playerAnimator.SetTrigger("Die");
        isAlive = false;
        playerRigidBody2D.bodyType = RigidbodyType2D.Static;
        playerCapsuleBody.enabled = false;

        StartCoroutine(WaitUntillReload(2));
    }

    IEnumerator WaitUntillReload(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
