using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;

public class SetIpFunction : NetworkBehaviour
{
    [SerializeField] GameObject ip_input;
    public void SetIp()
    {
        Debug.Log("ip address input = " + ip_input.GetComponent<TMP_InputField>().text);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip_input.GetComponent<TMP_InputField>().text, (ushort)12345, "0.0.0.0");
    }
}
