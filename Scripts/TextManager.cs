//Connector text box gameObject

using UnityEngine;
using Scripts;

public class TextManager : Singleton<TextManager>
{
    public GameObject connectorInfo;

    void Awake()
    {

    }

    void Update()
    {
        connectorInfo.SetActive(true);
    }
}