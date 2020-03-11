using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Collider2D trigger;
    public Transform spawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == trigger)
        {
            this.transform.position = spawnPoint.transform.position;
        }
    }
}
