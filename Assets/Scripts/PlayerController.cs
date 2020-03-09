using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;                                                  // Reference to the Animator component of the player.
    private Rigidbody2D rigidbody;                                              // Reference to the Rigidbody2D component of the player.
    private SpriteRenderer spriteRenderer;                                      // Reference to the SpriteRenderer component of the player.

    [Range(0.0f, 10.0f)] [SerializeField] float movementSpeed = 5.0f;           // Adjustable movement speed of the player.
    [Range(0.0f,  0.1f)] [SerializeField] float movementSmoothing = 0.05f;      // Adjustable "glide" when starts or stops moving.
    [Range(0.0f, 10.0f)] [SerializeField] float jumpForce = 2.0f;               // Adjustable force impulse provided to the player in the up direction.
    [Range(0.0f, 10.0f)] [SerializeField] float jumpMultiplier = 2.0f;          // Adjustable gravity modifier for when the player is pressing jump and is moving up.
    [Range(0.0f, 10.0f)] [SerializeField] float fallMultiplier = 2.5f;          // Adjustable gravity modifier for when the player is falling.

    [SerializeField] LayerMask groundObjects;                                   // Layer(s) that contain objects with colliders that can be considered ground.
    [SerializeField] Transform groundCheck;                                     // Adjustable reference to the (empty) child element that performs the ground check.

    private Vector2 currentVelocity = Vector2.zero;                             // Current velocity of the player.
    private float horizontalMove = 0.0f;                                        // Direction and amount the player has to move from input.
    private float defaultGravityScale = 1.0f;                                   // Copy the intial gravity scale.
    private bool facingRight = true;                                            // Track of the sprite direction.
    private bool jumpRequest = false;                                           // Track if the player is jumping.
    private bool grounded = false;                                              // Track if the player is on the ground.

    private const float groundedRadius = 0.2f;                                  // Radius used by the groundCheck Transform to check contact with the ground.

    // Called when the script is enabled, before the Update is called for the first time
    void Start()
    {
        // *** Assign references to the players components
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // *** Copy the default gravity scale
        defaultGravityScale = rigidbody.gravityScale;
    }

    // Update phase of the native player loop
    void Update()
    {
        // *** Get the horizontal move amount and direction
        horizontalMove = Input.GetAxisRaw("Horizontal");

        // *** Get if the player is holding the jump button
        jumpRequest = Input.GetButton("Jump");
    }

    // Update phase of the physics loop
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

    // Update if the player is currently on the ground
    private void UpdateGrounded()
    {
        // *** Reset grounded to false
        grounded = false;

        // *** The player is grounded if a circlecast to the groundcheck position hits anything designated as ground.
        //     This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundObjects);

        // *** Loop through all the collliders in radius
        for (int i = 0; i < colliders.Length; i++)
        {
            // *** Set grounded to true when colliding with an object other than the player itself
            grounded |= colliders[i].gameObject != gameObject;
        }
    }

    // Flip the sprite on the y-axis
    private void FlipSprite()
    {
        // *** Get the scale of local scale of the sprite
        Vector3 scale = transform.localScale;

        // *** Flip the x coordinates
        scale.x *= -1;

        // *** Apply the flipped scale
        transform.localScale = scale;
    }
}
