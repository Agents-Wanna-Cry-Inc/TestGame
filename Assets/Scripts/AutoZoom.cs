using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoZoom : MonoBehaviour
{
    [SerializeField] public Rigidbody2D playerRigidbody;                                // Adjustable reference to the Rigidbody2D component of the player.
    [SerializeField] public Cinemachine.CinemachineVirtualCamera cinemachineCamera;     // Adjustable reference to the virtual camera of Cinemachine.

    [SerializeField] public float maxSpeed = 1.0f;                                      // Adjustable speed of the player at which the camera is zoomed maximally dynamically.
    [Range(0.0f, 5.0f)] [SerializeField] public float zoomFactor = 1.1f;                // Adjustable maximal zoom factor of the camera.
    [Range(0.0f, 2.0f)] [SerializeField] public float smoothTime = 0.5f;                // Adjustable time for the camera zoom smoothing.

    private float defaultOrthographicSize = 1.0f;                                       // Copy of the default orhtographic size of the camera
    private float zoomVelocity = 0.0f;                                                  // Current velocity in the x direction of the player

    // Called when the script is enabled, before the Update is called for the first time
    void Start()
    {
        // *** Copy the default orthographic size
        defaultOrthographicSize = cinemachineCamera.m_Lens.OrthographicSize;
    }

    // Update phase of the physics loop
    void FixedUpdate()
    {
        // *** Get the amount of the scaling that has to be applied to the camera (between 0 [none] and 1 [full])
        float scaleFactor = Mathf.Clamp01(Mathf.Abs(playerRigidbody.velocity.x) / maxSpeed);

        // *** Calculate the new orthgraphic size
        float targetOrthographicSize = defaultOrthographicSize + defaultOrthographicSize * scaleFactor * (zoomFactor - 1);

        // *** Smoothly transition to the new orthographic size
        cinemachineCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(cinemachineCamera.m_Lens.OrthographicSize, targetOrthographicSize, ref zoomVelocity, smoothTime);
    }
}
