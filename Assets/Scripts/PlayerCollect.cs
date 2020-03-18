using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [SerializeField] public LayerMask collectables;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((collectables.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) 
        {
            Destroy(other.gameObject);
        }
    }
}
