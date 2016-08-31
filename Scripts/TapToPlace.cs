//Responsible for the placement of the Enviroment on the Visual Meshes around you
//Based on the class originated from github.com/Microsoft/HoloToolkit-Unity/
using UnityEngine;

public class TapToPlace : MonoBehaviour
{
    bool placing = false;

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        if (InteractibleManager.Instance.FocusedGameObject == GameObject.Find("Environment/Plane"))
        {
            // On each Select gesture, toggle whether the user is in placing mode.
            placing = !placing;

            // If the user is in placing mode, display the spatial mapping mesh.
            if (placing)
            {
                SpatialMapping.Instance.DrawVisualMeshes = true;
            }
            // If the user is not in placing mode, hide the spatial mapping mesh.
            else
            {
                SpatialMapping.Instance.DrawVisualMeshes = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!Source.Instance.con)
        {
            if (placing)
            {
                // Do a raycast into the world that will only hit the Spatial Mapping mesh.
                var headPosition = Camera.main.transform.position;
                var gazeDirection = Camera.main.transform.forward;

                RaycastHit hitInfo;
                if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                    30.0f, SpatialMapping.PhysicsRaycastMask))
                {
                    // Move this object's parent object to
                    // where the raycast hit the Spatial Mapping mesh.
                    transform.position = hitInfo.point;
                }
            }
        }
    }
}
