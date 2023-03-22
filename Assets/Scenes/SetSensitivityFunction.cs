using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class SetSensitivityFunction : NetworkBehaviour
{
    GameObject sensitivity_input;

    public void SetSensitivity()
    {
        sensitivity_input = GameObject.Find("SensitivityInput");
        if (GameObject.Find("SensitivityInput"))
            Debug.Log("sensitivity input assigned");
        else
            Debug.Log("can'find sensitivity input");
        float sensitivity = float.Parse(sensitivity_input.GetComponent<TMP_InputField>().text);
        Debug.Log("sensitivity input = " + sensitivity);
        MouseController.mouse_sensitivity = sensitivity;
    }
}
