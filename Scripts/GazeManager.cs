//Based on the class originated from github.com/Microsoft/HoloToolkit-Unity/
// GazeManager determines the location of the user's gaze, hit position and normals.

using Scripts;
using UnityEngine;
public class GazeManager : Singleton<GazeManager>
{
    [Tooltip("Maximum gaze distance for calculating a hit.")]
    public float MaxGazeDistance = 5.0f;

    [Tooltip("Select the layers raycast should target.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    /// <summary>
    /// Physics.Raycast result is true if it hits a Hologram.
    /// </summary>
    public bool Hit { get; private set; }

    /// <summary>
    /// HitInfo property gives access
    /// to RaycastHit public members.
    /// </summary>
    public RaycastHit HitInfo { get; private set; }

    /// <summary>
    /// Position of the user's gaze.
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <summary>
    /// RaycastHit Normal direction.
    /// </summary>
    public Vector3 Normal { get; private set; }

    private GazeStabilizer gazeStabilizer;
    private Vector3 gazeOrigin;
    private Vector3 gazeDirection;

    void Awake()
    {
        // GetComponent GazeStabilizer and assign it to gazeStabilizer.
        gazeStabilizer = GetComponent<GazeStabilizer>();
    }

    private void Update()
    {
        //Assign Camera's main transform position to gazeOrigin.
        gazeOrigin = Camera.main.transform.position;

        //Assign Camera's main transform forward to gazeDirection.
        gazeDirection = Camera.main.transform.forward;

        // Using gazeStabilizer, call function UpdateHeadStability.
        // Pass in gazeOrigin and Camera's main transform rotation.
        gazeStabilizer.UpdateHeadStability(gazeOrigin, Camera.main.transform.rotation);

        // Using gazeStabilizer, get the StableHeadPosition and
        // assign it to gazeOrigin.
        gazeOrigin = gazeStabilizer.StableHeadPosition;

        UpdateRaycast();
    }

    /// <summary>
    /// Calculates the Raycast hit position and normal.
    /// </summary>
    private void UpdateRaycast()
    {
        // Create a variable hitInfo of type RaycastHit.
        RaycastHit hitInfo;

        // Perform a Unity Physics Raycast.
        // Collect return value in public property Hit.
        // Pass in origin as gazeOrigin and direction as gazeDirection.
        // Collect the information in hitInfo.
        // Pass in MaxGazeDistance and RaycastLayerMask.
        Hit = Physics.Raycast(gazeOrigin,
                       gazeDirection,
                       out hitInfo,
                       MaxGazeDistance,
                       RaycastLayerMask);

        // Assign hitInfo variable to the HitInfo public property 
        // so other classes can access it.
        HitInfo = hitInfo;

        if (Hit)
        {
            // If raycast hit a hologram...

            // Assign property Position to be the hitInfo point.
            Position = hitInfo.point;
            // Assign property Normal to be the hitInfo normal.
            Normal = hitInfo.normal;
        }
        else
        {
            // If raycast did not hit a hologram...
            // Save defaults ...

            // Assign Position to be gazeOrigin plus MaxGazeDistance times gazeDirection.
            Position = gazeOrigin + (gazeDirection * MaxGazeDistance);
            // Assign Normal to be the user's gazeDirection.
            Normal = gazeDirection;
        }
    }
}