using UnityEngine;
using System.Collections;

public class ClearManager : MonoBehaviour {

    public Button clearButton;

	void Start () {
	
	}
	void Update () {

        if(InteractibleManager.Instance.FocusedGameObject == gameObject && Source.Instance.con)
        {
            clearButton.SetActive(true);
        }
        else
        {
            clearButton.SetActive(false);
        }
	
	}

    void OnSelect()
    {
        if(clearButton.IsOn())
        {
            //Accessing ROS service "clear" to clean the turlte environment on ROS.
            Source.Instance.SendService("/clear", "");
        }

    }
}
