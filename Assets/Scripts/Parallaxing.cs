using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    [SerializeField] Transform[] backgrounds;       // Adjustable rray of the background objects that have to parallax.

    private float[] parallaxScales;                 // Array of the movement scales of the backgrounds based on their z position.

    private Transform cameraTransform;              // Reference to the Transform property of the main camera.
    private Vector3 previousCameraPosition;         // Position of the camera in the previous frame (to calculate delta movement).

    // Called when the script is enabled, before Start()
    void Awake()
    {
        // *** Create the referense to the transform of the main camera
        cameraTransform = Camera.main.transform;
    }

    // Called when the script is enabled, before the Update is called for the first time
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

    // Update phase of the native player loop
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
