using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine.UI;

public class PauseMenuButtonFunctions : NetworkBehaviour
{
    NetworkObject player;
    ulong local_client_id;
    //floor right
    [SerializeField] Button TeamLeftButton;
    //floor left
    [SerializeField] Button TeamRightButton;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        TeamLeftButton.onClick.AddListener(TPtoOrangeCats);
        TeamRightButton.onClick.AddListener(TPtoBoys);
        Debug.Log("button add listener done");
        local_client_id = NetworkManager.Singleton.LocalClientId;
        /*
        Debug.Log("found local client id");
        if (NetworkManager.ConnectedClients.ContainsKey(local_client_id))
        {
            Debug.Log("get player reference");
            player = NetworkManager.ConnectedClients[local_client_id].PlayerObject;
        }
        */
    }

    public void ServerButton()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void HostButton()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ClientButton()
    {
        NetworkManager.Singleton.StartClient();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerTpOrangeCatsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("before if");
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
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