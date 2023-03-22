using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Scoring : NetworkBehaviour
{
    public static Text OrangeCatsText;
    public static Text BoysText;
    public static NetworkVariable<int> ScoreOrangeCats = new NetworkVariable<int>(0);
    public static NetworkVariable<int> ScoreBoys = new NetworkVariable<int>(0);
    // Start is called before the first frame update
    void Start()
    {
        OrangeCatsText = GameObject.Find("ScoreLeft").GetComponent<Text>();
        BoysText = GameObject.Find("ScoreRight").GetComponent<Text>();
        //Debug.Log("current ScoreBoys.Value = " + ScoreBoys.Value);
        //Debug.Log("current ScoreBoys = " + ScoreBoys);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //Debug.Log(OwnerClientId + " before subscribing onValueChanged function");
        ScoreOrangeCats.OnValueChanged += ScoreOrangeCatsChanged;
        ScoreBoys.OnValueChanged += ScoreBoysChanged;
        OrangeCatsText.text = ScoreOrangeCats.Value.ToString();
        BoysText.text = ScoreBoys.Value.ToString();
        ScoreClientInitializationServerRpc();
        //Debug.Log("network : current ScoreBoys.Value = " + ScoreBoys.Value);
        //Debug.Log("network : current ScoreBoys = " + ScoreBoys);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreReset()
    {
        ScoreResetServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void ScoreResetServerRpc()
    {
        ScoreOrangeCats.Value = 0;
        ScoreBoys.Value = 0;
    }

    [ServerRpc(RequireOwnership =false)]
    void ScoreClientInitializationServerRpc()
    {
        ScoreBoysChangedClientRpc(ScoreBoys.Value);
        ScoreOrangeCatsChangedClientRpc(ScoreOrangeCats.Value);
    }

    /*
    public static void ScoreAddOrangeCats()
    {
        Debug.Log(ScoreOrangeCats.Value);
        ScoreOrangeCats += 1;
        OrangeCatsText.text = ScoreOrangeCats.ToString();
    }

    public static void ScoreAddBoys()
    {
        Debug.Log(ScoreBoys);
        ScoreBoys += 1;
        BoysText.text = ScoreBoys.ToString();
    }
    */

    void ScoreOrangeCatsChanged(int previous, int current)
    {
        //Debug.Log(OwnerClientId + " changing score of orange cats");
        OrangeCatsText.text = current.ToString();
        ScoreOrangeCatsChangedClientRpc(current);
    }

    void ScoreBoysChanged(int previous, int current)
    {
        //Debug.Log(OwnerClientId + " changing score of boys");
        BoysText.text = current.ToString();
        ScoreBoysChangedClientRpc(current);
    }

    [ClientRpc]
    void ScoreOrangeCatsChangedClientRpc(int current)
    {
        OrangeCatsText.text = current.ToString();
    }

    [ClientRpc]
    void ScoreBoysChangedClientRpc(int current)
    {
        BoysText.text = current.ToString();
    }

}
