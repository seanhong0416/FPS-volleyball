using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Shoot : NetworkBehaviour
{
    [SerializeField] float fire_range = 15f;
    [SerializeField] float impact_force = 15f;
    [SerializeField] float fire_rate = 15f;
    //only for single player
    //[SerializeField] LineRenderer line_renderer;
    [SerializeField] Camera character;
    [SerializeField] Transform gun;
    [SerializeField] GameObject line_renderer_prefab;
    GameObject line_renderer_instance;
    //[SerializeField] GameObject TestPrefab;

    float next_fire_time = 0f;
    //int line_render_start = 0;
    //int line_render_end = 1;
    //Vector3 line_render_start_point;
    //Vector3 line_render_end_point;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;

        //Instantiate locally
        line_renderer_instance = Instantiate(line_renderer_prefab);
        line_renderer_instance.GetComponent<LineRenderer>().positionCount = 0;
        //Spawn on network
        SpawnLineRendererServerRpc();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (!PauseTransition.isPaused)
        {
            if (Input.GetButton("Fire1") && Time.time >= next_fire_time)
            {
                fire();
                next_fire_time = Time.time + 1 / fire_rate;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                ResetRazer();
                ResetRazerServerRpc();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                transform.parent.gameObject
                    .transform.GetChild(1).gameObject
                    .SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    [ServerRpc]
    void HitBallServerRpc(string ObjectName, Vector3 normal)
    {
        GameObject.Find(ObjectName).GetComponent<Rigidbody>().AddForce(-normal * impact_force);
    }

    void fire()
    {
        RaycastHit hit;

        if (Physics.Raycast(character.transform.position, character.transform.forward, out hit)){
            //Debug.Log(hit.transform.name);
            if(hit.rigidbody != null)
            {
                HitBallServerRpc(hit.collider.gameObject.name, hit.normal);
            }
            Vector3 first = gun.position;
            Vector3 second = hit.point - line_renderer_instance.transform.position;
            line_renderer_instance.GetComponent<LineRenderer>().positionCount = 2;
            line_renderer_instance.GetComponent<LineRenderer>().SetPosition(0, first);
            line_renderer_instance.GetComponent<LineRenderer>().SetPosition(1, second);
            SetLineRendererServerRpc(first, second);
        }
        else
        {
            ResetRazer();
            ResetRazerServerRpc();
        }
    }

    [ServerRpc]
    void SpawnLineRendererServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        line_renderer_instance = Instantiate(line_renderer_prefab);
        line_renderer_instance.GetComponent<LineRenderer>().positionCount = 0;
        line_renderer_instance.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }

    void ResetRazer()
    {
        line_renderer_instance.GetComponent<LineRenderer>().positionCount = 0;
    }

    [ServerRpc]
    void ResetRazerServerRpc()
    {
        line_renderer_instance.GetComponent<LineRenderer>().positionCount = 0;
    }

    [ServerRpc]
    void SetLineRendererServerRpc(Vector3 first, Vector3 second)
    {
        line_renderer_instance.GetComponent<LineRenderer>().positionCount = 2;
        line_renderer_instance.GetComponent<LineRenderer>().SetPosition(0, first);
        line_renderer_instance.GetComponent<LineRenderer>().SetPosition(1, second);
    }
}
