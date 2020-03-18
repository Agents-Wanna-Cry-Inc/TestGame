using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    // ================== References ==================
    private SpriteRenderer spriteRenderer;                                            // Reference to the Rigidbody2D component of the dog.
    private Rigidbody2D rigidbody;                                                    // Reference to the SpriteRenderer component of the dog.
    private GameObject[] enemies;                                                     // List of enemies.
    
    // ================ Player settings ===============
    [SerializeField] Transform playerTransform;                                        // Reference to the transform of the player.
    [SerializeField] Collider2D playerCollider;                                        // Reference to the collider of the player.
    
    // =============== Movement options ===============
    [Range(0.0f, 10.0f)] [SerializeField] float followSpeed = 3.0f;                    // Adjustable movement speed of the dog.
    [Range(0.0f,  5.0f)] [SerializeField] float movementSmoothing = 1.0f;              // Adjustable "glide" when starts or stops moving.
    [Range(0.0f, 10.0f)] [SerializeField] float jumpForce = 2.0f;                      // Adjustable force impulse provided to the go in the up direction.
    [Range(0.0f, 20.0f)] [SerializeField] float playerVisionRadius = 5.0f;             // Adjustable radius in which the dog can see the player.
    [Range(0.0f, 20.0f)] [SerializeField] float enemyVisionRadius = 5.0f;              // Adjustable radius in which the dog can see enemies.
    
    // =============== Class variables ================
    [SerializeField] LayerMask enemyObjects;                                            // Layer(s) that contain the enemy objects.
    [SerializeField] LayerMask groundObjects;                                           // Layer(s) that contain objects with colliders that can be considered ground.
    [SerializeField] Transform groundCheck;                                             // Adjustable reference to the (empty) child element that performs the ground check.

    // ============== Collision settings ==============
    private bool playerSeen = false;                                                   // Track if the player has seen the dog.
    private bool grounded = false;                                                     // Track if the dog is on the ground.

    // =========== Preconfigured variables ============
    private const float groundedRadius = 0.05f;                                       // Radius used by the groundCheck Transform to check contact with the ground.
    
    // Called when the script is enabled, before the Update is called for the first time
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        enemies = FindGameObjectsInLayer(enemyObjects.value);
        
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>());
    }

    // Update phase of the physics loop
    void FixedUpdate()
    {
        if (!playerSeen) {
            if (Vector2.Distance(transform.position, playerTransform.position) < playerVisionRadius)
            {
                playerSeen = true;
            }
        }
    
        if (playerSeen)
        {
            UpdateGrounded();
        
            if (grounded) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.position.x > playerTransform.position.x) ? Vector2.left : -Vector2.left, 1.0f, groundObjects);
                
                if (hit.collider != null) {
                    rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
        
            GameObject closestEnemy = FindClosestEnemy();
            
            Vector2 velocity = rigidbody.velocity;
            if (closestEnemy != null && Vector2.Distance(transform.position, closestEnemy.transform.position) < enemyVisionRadius)
            {
                if (transform.position.x > closestEnemy.transform.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
        
                Vector2.SmoothDamp(rigidbody.position, closestEnemy.transform.position, ref velocity, movementSmoothing, followSpeed, Time.deltaTime);
            }
            else
            {
                if (transform.position.x > playerTransform.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            
                Vector2.SmoothDamp(rigidbody.position, playerTransform.position, ref velocity, movementSmoothing, followSpeed, Time.deltaTime);
            }
            rigidbody.velocity = new Vector2(velocity.x, rigidbody.velocity.y);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (new List<GameObject>(enemies).Contains(collider.gameObject))
        {
            Destroy(collider.gameObject);
            enemies = FindGameObjectsInLayer(enemyObjects.value);
        }
    }
    
    // Update if the dog is currently on the ground
    private void UpdateGrounded()
    {
        // *** Reset grounded to false
        grounded = false;

        // *** If the player is moving up, it can never be grounded
        //     Fix for problem that applied the jump impulse multiple time on fast machines
        if (rigidbody.velocity.y <= 0)
        {
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
    }
    
    private GameObject FindClosestEnemy()
    {
        if (enemies == null)
        {
            return null;
        }
    
        GameObject[] gameObjects;
        GameObject closestGameObject = null;
        
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        
        foreach (GameObject gameObject in enemies)
        {
            Vector3 diff = gameObject.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            
            if (curDistance < distance)
            {
                closestGameObject = gameObject;
                distance = curDistance;
            }
        }
        
        return closestGameObject;
    }
    
    private GameObject[] FindGameObjectsInLayer(int layer)
    {
        GameObject[] goArray = FindObjectsOfType<GameObject>();
        var goList = new System.Collections.Generic.List<GameObject>();
        
        for (int i = 0; i < goArray.Length; i++)
        {
            if ((1 << goArray[i].layer) == layer)
            {
                goList.Add(goArray[i]);
            }
        }
        
        if (goList.Count == 0)
        {
            return null;
        }
        
        return goList.ToArray();
    }
}
