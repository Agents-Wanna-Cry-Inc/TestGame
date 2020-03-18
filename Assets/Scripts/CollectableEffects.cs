using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEffects : MonoBehaviour
{
    [SerializeField] public float height = 1.0f;
    [SerializeField] public float speed  = 1.0f;
    [SerializeField] public float offset = 0.0f;

    [SerializeField] public ParticleSystem destructionEffect;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.y = (Mathf.Sin(Time.time * speed * 2 * Mathf.PI) + offset) * height;
        transform.localPosition = pos;
    }

    void OnDestroy()
    {
        // *** Instantiate our one-off particle system
        ParticleSystem destructionEffectParticleSystem = Instantiate(destructionEffect) as ParticleSystem;

        // *** Get the main module of the particle system
        ParticleSystem.MainModule mainModule = destructionEffectParticleSystem.main;

        // *** Move the particle system to the position of the collectable
        destructionEffectParticleSystem.transform.position = transform.position;

        // *** Disable looping
        mainModule.loop = false;

        // *** Play the animation
        destructionEffectParticleSystem.Play();

        // *** Destroy the particle system at the end of its cycle
        Destroy(destructionEffectParticleSystem.gameObject, mainModule.duration);
    }
}
