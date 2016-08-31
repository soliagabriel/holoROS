//Based on the class originated from github.com/Microsoft/HoloToolkit-Unity/

using UnityEngine;
using System.Collections;

public class GestureAction : MonoBehaviour
{
    private Vector3 manipulationPreviousPosition;

    // Use this for initialization

    void Start()
    {
        GestureManager.Instance.Transition(GestureManager.Instance.ManipulationRecognizer);
    }

    // Update is called once per frame
    void Update()
    {
        //Updating cube position respecting the plane boundaries.
        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Clamp(pos.x, - 0.46f, 0.46f);
        pos.y = Mathf.Clamp(pos.y, - 0.46f, + 0.46f);
        pos.z = - 0.7f;

        transform.localPosition = pos;
    }

    void PerformManipulationStart(Vector3 position)
    {
        manipulationPreviousPosition = position;
    }

    void PerformManipulationUpdate(Vector3 position)
    {
        if (GestureManager.Instance.IsManipulating)
        {
            Vector3 moveVector = Vector3.zero;

            // Calculate the moveVector as position - manipulationPreviousPosition.
            moveVector = position - manipulationPreviousPosition;

            //Update the manipulationPreviousPosition with the current position.
            manipulationPreviousPosition = position;

            //Increment this transform's position by the moveVector.
            transform.position += moveVector;
            
        }
    }
}
