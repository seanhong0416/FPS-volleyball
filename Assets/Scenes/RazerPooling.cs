using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;


public class RazerPooling : NetworkBehaviour
{
    private static RazerPooling _instance;
    public static RazerPooling Singleton { get { return _instance; } }

    public void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void TestNetworkObjectSpawn(int number, GameObject prefab)
    {
        GameObject instance = Instantiate(prefab);
        instance.GetComponent<NetworkObject>().Spawn();
    }
}
