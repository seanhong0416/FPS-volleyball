/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerTeleportation : NetworkBehaviour
{
    /*
    [SerializeField] NetworkObject player;
    //floor right
    [SerializeField] Button TeamLeftButton;
    //floor left
    [SerializeField] Button TeamRightButton;

    ulong local_client_id;

    private void onNetworkSpawn()
    {
        TeamLeftButton = GameObject.Find("TeamLeftButton").GetComponent<Button>();
        TeamLeftButton.onClick.AddListener(TPtoOrangeCats);
        TeamRightButton = GameObject.Find("TeamRightButton").GetComponent<Button>();
        TeamRightButton.onClick.AddListener(TPtoBoys);
        Debug.Log("button add listener done");
        local_client_id = NetworkManager.Singleton.LocalClientId;
        Debug.Log("found local client id");
        if (NetworkManager.ConnectedClients.ContainsKey(local_client_id))
        {
            Debug.Log("get player reference");
            player = NetworkManager.ConnectedClients[local_client_id].PlayerObject;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerTpOrangeCatsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("before if");
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            Debug.Log("after if");
            var client = NetworkManager.Singleton.ConnectedClients[clientId];
            Debug.Log("after assigning client");
            client.PlayerObject.GetComponent<CharacterController>().enabled = false;
            client.PlayerObject.transform.position = GameObject.Find("FloorRight").transform.position + new Vector3(0, 6, 0);
            client.PlayerObject.GetComponent<CharacterController>().enabled = true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerTpBoysServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            client.PlayerObject.GetComponent<CharacterController>().enabled = false;
            client.PlayerObject.transform.position = GameObject.Find("FloorLeft").transform.position + new Vector3(0, 6, 0);
            client.PlayerObject.GetComponent<CharacterController>().enabled = true;
        }
    }

    public void TPtoOrangeCats()
    {
        Debug.Log("Tp to Orange Cats");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = GameObject.Find("FloorRight").transform.position + new Vector3(0, 6, 0);
        player.GetComponent<CharacterController>().enabled = true;
        Debug.Log("Tp to Orange Cats done");
    }

    public void TPtoBoys()
    {
        Debug.Log("Tp to Boys");
        PlayerTpBoysServerRpc();
    }
}
*/

using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleportation : MonoBehaviour
{
    public Button TeamLeftButton;
    public Button TeamRightButton;
    public Transform FloorRight;
    public Transform FloorLeft;
    public CharacterController controller;

    void Start()
    {
        TeamLeftButton = GameObject.Find("TeamLeftButton").GetComponent<Button>();
        TeamLeftButton.onClick.AddListener(TeleportPlayerLeft);
        FloorRight = GameObject.Find("FloorRight").GetComponent<Transform>();

        TeamRightButton = GameObject.Find("TeamRightButton").GetComponent<Button>();
        TeamRightButton.onClick.AddListener(TeleportPlayerRight);
        FloorLeft = GameObject.Find("FloorLeft").GetComponent<Transform>();

        controller = gameObject.GetComponent<CharacterController>();
    }

    void TeleportPlayerLeft()
    {
        // Teleport the local player to the specified position.
        Vector3 destination = FloorRight.position + new Vector3(0,6,0);
        Vector3 offset = destination - transform.position;
        controller.enabled = false;
        gameObject.transform.position = destination;
        controller.enabled = true;
    }

    void TeleportPlayerRight()
    {
        // Teleport the local player to the specified position.
        Vector3 destination = FloorLeft.position + new Vector3(0, 6, 0);
        Vector3 offset = destination - transform.position;
        controller.enabled = false;
        gameObject.transform.position = destination;
        controller.enabled = true;
    }
}