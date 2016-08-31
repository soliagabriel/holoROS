//Based on the class originated from github.com/Microsoft/HoloToolkit-Unity/
//The button class controls the state of all the button used in the project.

using UnityEngine;
using Scripts;
using UnityEngine.Events;

public class Button : Singleton<Button>
{
    public State StartingState;

    private Renderer buttonRenderer;

    public State currentState = State.Inactive;

    public enum State { Inactive, Active, Gazed, Selected };

    void Awake()
    {
        buttonRenderer = GetComponent<Renderer>();

        ChangeButtonState(StartingState);
    }

    public bool IsOn()
    {
        return currentState != State.Inactive;
    }

    public void SetActive(bool setOn)
    {
        if (setOn)
        {
            ChangeButtonState(State.Active);
            if (InteractibleManager.Instance.FocusedGameObject == gameObject)
            {
                ChangeButtonState(State.Gazed);
            }
        }
        else
        {
            ChangeButtonState(State.Inactive);
        }
    }

    public void ChangeButtonState(State newState)
    {
        State oldState = currentState;
        currentState = newState;

        if (newState > oldState)
        {
            for (int j = (int)newState; j > (int)oldState; j--)
            {
                buttonRenderer.material.SetFloat("_BlendTex0" + j, 1.0f);
            }
        }
        else
        {
            for (int j = (int)oldState; j > (int)newState; j--)
            {
                buttonRenderer.material.SetFloat("_BlendTex0" + j, 0.0f);
            }
        }
    }
}
