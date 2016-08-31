//Based on the class originated from github.com/Microsoft/HoloToolkit-Unity/
//CursorFeedback class takes GameObjects to give cursor feedback to users based on different states.

using Scripts;
using UnityEngine;

public class CursorFeedback : Singleton<CursorFeedback>
{
    public GameObject HandDetectedAsset;
    private GameObject handDetectedGameObject;

    public GameObject PathingDetectedAsset;
    private GameObject pathingDetectedGameObject;

    public GameObject FeedbackParent;

    private Interactible FocusedInteractible
    {
        get
        {
            if (InteractibleManager.Instance.FocusedGameObject != null)
            {
                return InteractibleManager.Instance.FocusedGameObject.GetComponent<Interactible>();
            }

            return null;
        }
    }

    void Awake()
    {
        if (HandDetectedAsset != null)
        {
            handDetectedGameObject = InstantiatePrefab(HandDetectedAsset);
        }

        if (PathingDetectedAsset != null)
        {
            pathingDetectedGameObject = InstantiatePrefab(PathingDetectedAsset);
        }
    }

    private GameObject InstantiatePrefab(GameObject inputPrefab)
    {
        GameObject instantiatedPrefab = null;

        if (inputPrefab != null && FeedbackParent != null)
        {
            instantiatedPrefab = GameObject.Instantiate(inputPrefab);

            // Assign parent to be the FeedbackParent so that feedback assets move and rotate with this parent.
            instantiatedPrefab.transform.parent = FeedbackParent.transform;

            // Set starting state of gameobject to be inactive.
            instantiatedPrefab.gameObject.SetActive(false);
        }

        return instantiatedPrefab;
    }

    void Update()
    {
        UpdateHandDetectedState();

        UpdatePathDetectedState();
    }

    private void UpdateHandDetectedState()
    {
        if (handDetectedGameObject == null || CursorManager.Instance == null)
        {
            return;
        }

        handDetectedGameObject.SetActive(HandsManager.Instance.HandDetected);
    }

    private void UpdatePathDetectedState()
    {
        if (pathingDetectedGameObject == null)
        {
            return;
        }

        if (CursorManager.Instance == null || FocusedInteractible == null ||
            GestureManager.Instance.ActiveRecognizer != GestureManager.Instance.ManipulationRecognizer)
        {
            pathingDetectedGameObject.SetActive(false);
            return;
        }

        if(InteractibleManager.Instance.FocusedGameObject.GetComponent<GestureAction>())
        {
            pathingDetectedGameObject.SetActive(true);
        }
    }
}