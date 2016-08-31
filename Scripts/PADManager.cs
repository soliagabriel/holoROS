//Here all the movement related to gazing at the DPAD is made.

using UnityEngine;
using Scripts;

public class PADManager : Singleton<PADManager>
{
    public Button forwardButton;

    public Button backwardButton;

    public Button countercklw;

    public Button cklw;

    public GameObject turtle;

    void Start()
    {

    }

    void Update()
    {
        if (Source.Instance.con)
        {
            forwardButton.SetActive(true);
            backwardButton.SetActive(true);
            countercklw.SetActive(true);
            cklw.SetActive(true);

            PerformMovement();
        }
        else
        {
            forwardButton.SetActive(false);
            backwardButton.SetActive(false);
            countercklw.SetActive(false);
            cklw.SetActive(false);
        }
    }

    private void PerformMovement()
    {
        if (InteractibleManager.Instance.FocusedGameObject == GameObject.Find("DPAD/Forward") &&
            forwardButton.IsOn())
        {
            turtle.transform.localPosition += Vector3.right * 0.005f;
        }
        else if (InteractibleManager.Instance.FocusedGameObject == GameObject.Find("DPAD/Backward") &&
            backwardButton.IsOn())
        {
             turtle.transform.localPosition += -Vector3.right * 0.005f;
        }

        if (InteractibleManager.Instance.FocusedGameObject == GameObject.Find("DPAD/CounterClockwise") &&
            countercklw.IsOn())
        {
            turtle.transform.localPosition += Vector3.up * 0.005f;
        }
        else if (InteractibleManager.Instance.FocusedGameObject == GameObject.Find("DPAD/Clockwise") &&
            cklw.IsOn())
        {
            turtle.transform.localPosition += -Vector3.up * 0.005f;
        }
    }
}
