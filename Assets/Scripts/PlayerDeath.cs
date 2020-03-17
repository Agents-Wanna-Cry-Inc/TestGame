using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Collider2D trigger;      // The collider that triggers a respawn
    public Transform spawnPoint;    // The spawnpoint of the player

    // Fires when colliding with a collider that is flagged as trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // *** Check if the collider that triggered the event is the respawn trigger
        if (other == trigger)
        {
            // *** Move the player back to the spawnpoint
            this.transform.position = spawnPoint.transform.position;
        }
    }
}
