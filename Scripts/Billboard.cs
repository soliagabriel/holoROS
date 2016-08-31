//Based on the class originated from github.com/Microsoft/HoloToolkit-Unity/
//The Billboard class implements the behaviors needed to keep the ROS environment oriented towards the user.

using UnityEngine;

namespace Scripts
{
    public enum PivotAxis
    {
        // Rotate about all axes.
        Free,
        // Rotate about an individual axis.
        X,
        Y
    }

    public class Billboard : MonoBehaviour
    {
        // The axis about which the object will rotate.
        [Tooltip("Specifies the axis about which the object will rotate (Free rotates about both X and Y).")]
        public PivotAxis PivotAxis = PivotAxis.Free;
        
        // Overrides the cached value of the GameObject's default rotation.
        public Quaternion DefaultRotation { get; private set; }

        private void Awake()
        {
            // Cache the GameObject's default rotation.
            DefaultRotation = gameObject.transform.rotation;
        }
        // The billboard logic is performed in FixedUpdate to update the object
        // with the player independent of the frame rate.  This allows the object to 
        // remain correctly rotated even if the frame rate drops.
        private void FixedUpdate()
        {
            // Get a Vector that points from the Camera to the Target.
            Vector3 directionToTarget = Camera.main.transform.position - gameObject.transform.position;

            // Adjust for the pivot axis.
            switch (PivotAxis)
            {
                case PivotAxis.X:
                    directionToTarget.x = gameObject.transform.position.x;
                    break;

                case PivotAxis.Y:
                    directionToTarget.y = gameObject.transform.position.y;
                    break;

                case PivotAxis.Free:
                default:
                    break;
            }

            // Calculate and apply the rotation required to reorient the object and apply the default rotation to the result.
            gameObject.transform.rotation = Quaternion.LookRotation(-directionToTarget) * DefaultRotation;
        }
    }
}