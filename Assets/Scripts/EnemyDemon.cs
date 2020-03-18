using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemon : MonoBehaviour
{
    private Rigidbody2D rigidbody;

    [SerializeField] public Transform playerTransform;
    [Range(0.0f, 10.0f)] [SerializeField] public float followSpeed = 3.0f;
    [Range(0.0f,  5.0f)] [SerializeField] public float movementSmoothing = 1.0f;
    [Range(0.0f, 20.0f)] [SerializeField] public float visionRadius = 5.0f;
    
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > playerTransform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    
    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < visionRadius)
        {
            Vector3 velocity = rigidbody.velocity;
            Vector3.SmoothDamp(rigidbody.position, playerTransform.position, ref velocity, movementSmoothing, followSpeed, Time.deltaTime);
            rigidbody.velocity = velocity;
        }
        else 
        {
            Vector3 velocity = rigidbody.velocity;
            Vector3.SmoothDamp(rigidbody.position, rigidbody.position, ref velocity, movementSmoothing, followSpeed, Time.deltaTime);
            rigidbody.velocity = velocity;
        }
    }
}
