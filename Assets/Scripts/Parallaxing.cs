using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    [SerializeField] Transform[] backgrounds;
    [SerializeField] public float smoothing = 1.0f;

    private float[] parallaxScales;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void Awake()
    {
        // *** Create the referense to the transform of the main camera
        cameraTransform = Camera.main.transform;
    }

    void Start()
    {
        // *** Initialize the previousCameraPosition with the current position
        previousCameraPosition = cameraTransform.position;

        // *** Initialize with float array with the length of backgrounds
        parallaxScales = new float[backgrounds.Length];

        // *** Loop through all the backgrounds
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // *** Get the inverted z coordinate of the backgrounds to determine its scale
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // *** Calcalate the opposite movement of the camera multiplied by the parallax scale
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxScales[i];

            // *** Set the new background position
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // *** Create the
            backgrounds[i].position = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
        }

        previousCameraPosition = cameraTransform.position;
    }
}
