using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public Collider2D trigger;    // The collider that triggers a respawn

    // Fires when colliding with a collider that is flagged as trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // *** Check if the collider that triggered the event is the respawn trigger
        if (other == trigger)
        {
            // *** Destroy the object as it is out of bounds to save memory and prevent floating point overflow
            Destroy(gameObject);
        }
    }
}
