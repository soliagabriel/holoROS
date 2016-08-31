using UnityEngine;
using System.Collections;

public class ResetManager : MonoBehaviour
{

    public Button resetButton;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (InteractibleManager.Instance.FocusedGameObject == gameObject && Source.Instance.con)
        {
            resetButton.SetActive(true);
        }
        else
        {
            resetButton.SetActive(false);
        }

    }

    void OnSelect()
    {
        if (resetButton.IsOn())
        {
            //Resetting turtle/cube to initial position.
            Source.Instance.turtle.transform.localPosition = new Vector3(0, 0, -0.7f);
        }

    }
}

