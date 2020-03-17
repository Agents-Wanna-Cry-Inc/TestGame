using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] public float height = 1.0f;
    [SerializeField] public float speed  = 1.0f;
    [SerializeField] public float offset = 0.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.y = (Mathf.Sin(Time.time * speed * 2 * Mathf.PI) + offset) * height;
        transform.localPosition = pos;
    }
}
