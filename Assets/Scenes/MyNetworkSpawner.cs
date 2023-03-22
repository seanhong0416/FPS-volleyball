using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Multiplayer.Samples.Utilities
{
    public class MyNetworkSpawner : MonoBehaviour
    {
        public NetworkObject prefabReference;

        public void Awake()
        {
            if (NetworkManager.Singleton && NetworkManager.Singleton.IsServer &&
                NetworkManager.Singleton.SceneManager != null)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManagerOnOnLoadEventCompleted;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            if (NetworkManager.Singleton && NetworkManager.Singleton.IsServer &&
                NetworkManager.Singleton.SceneManager != null)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManagerOnOnLoadEventCompleted;
            }
        }

        void SceneManagerOnOnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            SpawnNetworkObject();
            Destroy(gameObject);
        }

        void SpawnNetworkObject()
        {
            Vector3 SpawnPosition = GameObject.Find("VolleyBallNet").transform.position + new Vector3(0, 6, 0);
            var instantiatedNetworkObject = Instantiate(prefabReference, SpawnPosition, transform.rotation, null);
            SceneManager.MoveGameObjectToScene(instantiatedNetworkObject.gameObject,
                SceneManager.GetSceneByName(gameObject.scene.name));
            instantiatedNetworkObject.transform.localScale = transform.lossyScale;
            instantiatedNetworkObject.Spawn(destroyWithScene: true);
        }
    }
    
}
