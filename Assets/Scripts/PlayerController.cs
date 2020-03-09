using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    [Range(0.0f, 10.0f)] [SerializeField] float movementSpeed = 5.0f;
    [Range(0.0f,  0.1f)] [SerializeField] float movementSmoothing = 0.05f;
    [Range(0.0f, 10.0f)] [SerializeField] float jumpForce = 2.0f;
    [Range(0.0f, 10.0f)] [SerializeField] float jumpMultiplier = 2.0f;
    [Range(0.0f, 10.0f)] [SerializeField] float fallMultiplier = 2.5f;

    [SerializeField] LayerMask groundObjects;
    [SerializeField] Transform groundCheck;

    private Vector2 currentVelocity = Vector2.zero;
    private float horizontalMove = 0.0f;
    private float defaultGravityScale = 1.0f;
    private bool facingRight = true;
    private bool jumpRequest = false;
    private bool grounded = false;

    private const float groundedRadius = 0.2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultGravityScale = rigidbody.gravityScale;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        jumpRequest = Input.GetButton("Jump");
    }

    void FixedUpdate()
    {
        UpdateGrounded();

        // *** Calculate the desired velocity
        Vector2 targetVelocity = new Vector2(horizontalMove * movementSpeed * Time.fixedDeltaTime * 100.0f, rigidbody.velocity.y);

        // *** Smoothly reach the target velocity
        rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref currentVelocity, movementSmoothing);

        // *** Check if the player wants to and can jump
        if (jumpRequest && grounded)
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // *** Update the gravity scale when falling
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.gravityScale = defaultGravityScale * fallMultiplier;
        }
        else if (rigidbody.velocity.y > 0 && !jumpRequest)
        {
            rigidbody.gravityScale = defaultGravityScale * jumpMultiplier;
        }
        else
        {
            rigidbody.gravityScale = defaultGravityScale;
        }

        // Todo: Implement checking jump
        // *** Check if the player is walking
        if (System.Math.Abs(horizontalMove) > 0)
        {
            // *** Play the walk animation
            animator.Play("Player_walk");
        }
        else
        {
            // *** Play the idle animation
            animator.Play("Player_idle");
        }

        // *** Check if the sprite needs flipping
        if (facingRight && horizontalMove < 0)
        {
            FlipSprite();
            facingRight = false;
        }
        else if (!facingRight && horizontalMove > 0)
        {
            FlipSprite();
            facingRight = true;
        }
    }

    private void UpdateGrounded()
    {
        // *** Reset grounded to false
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundObjects);
        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.Log(colliders[i]);
            // *** Set grounded to true when colliding with an object other than the player itself
            grounded |= colliders[i].gameObject != gameObject;
        }
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
