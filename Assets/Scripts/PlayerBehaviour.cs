using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpForce;
    [SerializeField] private float playerClimbSpeed;
    [SerializeField] private LayerMask groundLayerMask;
    #endregion


    #region Private Variables
    private Rigidbody2D playerRigidBody2D;
    private Animator playerAnimator;
    private CapsuleCollider2D playerCapsuleBody;
    private PlayerCombat playerCombat;
    private float gravityScaleAtStart;
    private Vector2 playerBodySize;
    #endregion

    void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleBody = GetComponent<CapsuleCollider2D>();
        playerCombat = GetComponent<PlayerCombat>();

        gravityScaleAtStart = playerRigidBody2D.gravityScale;
        playerBodySize = playerCapsuleBody.bounds.size;
    }

    void Update()
    {
        if (playerCombat.isAlive) 
        {
            Run();
            FlipSprite();
            Jump();
            ClimbLadder(); 
        }
    }

    private void Run()
    {
            float axisValue = Input.GetAxisRaw(Consts.HORIZONTAL_AXIS);
            Vector2 playerVelocity = new Vector2(axisValue * playerSpeed, playerRigidBody2D.velocity.y);
            playerRigidBody2D.velocity = playerVelocity;

            bool playerHorizontalMove = Mathf.Abs(playerRigidBody2D.velocity.x) > Mathf.Epsilon; 
            playerAnimator.SetBool(Consts.RUN_ANIM, playerHorizontalMove); 
    }

    private void FlipSprite()
    {
        bool playerHorizontalMove = Mathf.Abs(playerRigidBody2D.velocity.x) > Mathf.Epsilon;

        if (playerHorizontalMove)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody2D.velocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        if (playerCapsuleBody.IsTouchingLayers(LayerMask.GetMask(Consts.CLIMB_LAYER_NAME)))
        {
            float axisValue = Input.GetAxisRaw(Consts.VERTICAL_AXIS);
            Vector2 playerClimbVelocity = new Vector2(playerRigidBody2D.velocity.x, axisValue * playerClimbSpeed);
            playerRigidBody2D.velocity = playerClimbVelocity;

            playerRigidBody2D.gravityScale = 0f;
            bool playerClimbing = Mathf.Abs(playerRigidBody2D.velocity.y) > Mathf.Epsilon;
        }else
        {
            playerRigidBody2D.gravityScale = gravityScaleAtStart;
            return;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown(Consts.JUMP_BTN) && PlayerGrounded())
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, playerJumpForce);
            playerRigidBody2D.AddForce(jumpVelocityToAdd, ForceMode2D.Impulse);
        }

        playerAnimator.SetBool(Consts.JUMP_ANIM, !PlayerGrounded());
    }
    
    private RaycastHit2D castRestultHit()
    {
        RaycastHit2D hitsGround = Physics2D.CapsuleCast(playerCapsuleBody.bounds.center, playerBodySize, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.15f, groundLayerMask);
        return hitsGround;
    }

    private bool PlayerGrounded()
    {
        return castRestultHit().collider != null;
    }
}
