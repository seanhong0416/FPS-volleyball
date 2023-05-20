using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;


public class SceneTransitionFunction : NetworkBehaviour
{
    public void StartNewGameFunction()
    {
        Debug.Log("StartNewGameButton clicked");
        SceneManager.LoadScene("SampleScene");
        StartGameAsHost();
    }

    void StartGameAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
