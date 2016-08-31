//Manage the connector text box status

using UnityEngine;
using UnityEngine.UI;

public class tipText : MonoBehaviour
{
    public Text tipMessage;

    void Update()
    {
        if(Source.Instance.con)
        {
            tipMessage.text = "Connected. Select to disconnect.";
        }
        else
        {
            tipMessage.text = "Select to connect.";
        }
    }
}