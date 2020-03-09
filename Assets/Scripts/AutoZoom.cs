using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoZoom : MonoBehaviour
{
    [SerializeField] public Rigidbody2D playerRigidbody;
    [SerializeField] public Cinemachine.CinemachineVirtualCamera cinemachineCamera;

    [SerializeField] public float maxSpeed = 1.0f;
    [Range(0.0f, 5.0f)] [SerializeField] public float zoomFactor = 1.1f;
    [Range(0.0f, 2.0f)] [SerializeField] public float smoothTime = 0.05f;

    private float defaultOrthographicSize = 1.0f;
    private float zoomVelocity = 0.0f;

    void Start()
    {
        defaultOrthographicSize = cinemachineCamera.m_Lens.OrthographicSize;
    }

    void FixedUpdate()
    {
        float scaleFactor = Mathf.Clamp01(Mathf.Abs(playerRigidbody.velocity.x) / maxSpeed);
        float targetOrthographicSize = defaultOrthographicSize + defaultOrthographicSize * scaleFactor * (zoomFactor - 1);

        cinemachineCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(cinemachineCamera.m_Lens.OrthographicSize, targetOrthographicSize, ref zoomVelocity, smoothTime);
    }
}
