using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShotgunShoot : NetworkBehaviour
{
    [SerializeField] float fire_range = 15f;
    [SerializeField] float impact_force = 15f;
    [SerializeField] float fire_rate = 1f;
    [SerializeField] float shotgun_razer_delay = 1f;
    [SerializeField] float spread_range = 1;
    [SerializeField] int pellets_per_shot = 6;

    //only for single player
    //[SerializeField] LineRenderer line_renderer;
    [SerializeField] Camera character;
    [SerializeField] Transform gun;
    [SerializeField] GameObject line_renderer_prefab;
    GameObject[] line_renderer_instance;

    float next_fire_time = 0f;
    //int line_render_start = 0;
    //int line_render_end = 1;
    //Vector3 line_render_start_point;
    //Vector3 line_render_end_point;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;

        line_renderer_instance = new GameObject[pellets_per_shot];
        for(int i = 0; i < pellets_per_shot; i++)
        {
            line_renderer_instance[i] = Instantiate(line_renderer_prefab);
            line_renderer_instance[i].GetComponent<LineRenderer>().positionCount = 0;
        }
        SpawnLineRendererServerRpc();
        gameObject.SetActive(false);
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
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                transform.parent.gameObject
                    .transform.GetChild(0).gameObject
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

        for (int i = 0; i < pellets_per_shot; i++)
        {
            Vector3 offset = Random.Range(-spread_range, spread_range) * character.transform.right + Random.Range(-spread_range, spread_range) * character.transform.up;
            Vector3 final_direction = Vector3.Normalize(fire_range * character.transform.forward + offset);

            if (Physics.Raycast(character.transform.position, final_direction, out hit))
            {
                //Debug.Log(hit.transform.name);
                if (hit.rigidbody != null)
                {
                    HitBallServerRpc(hit.collider.gameObject.name, hit.normal);
                }
                Vector3 first = gun.position;
                Vector3 second = hit.point - line_renderer_instance[i].transform.position;
                line_renderer_instance[i].GetComponent<LineRenderer>().positionCount = 2;
                line_renderer_instance[i].GetComponent<LineRenderer>().SetPosition(0, first);
                line_renderer_instance[i].GetComponent<LineRenderer>().SetPosition(1, second);
                SetLineRendererServerRpc(first, second, i);

                Invoke("ResetRazer", 0.3f);
                Invoke("ResetRazerServerRpc", 0.3f);
            }
            else
            {
                ResetRazer();
                ResetRazerServerRpc();
            }
        }
    }

    void ResetRazer()
    {
        for(int i = 0; i < pellets_per_shot; i++)
        {
            line_renderer_instance[i].GetComponent<LineRenderer>().positionCount = 0;
        }
    }

    [ServerRpc]
    void SpawnLineRendererServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        line_renderer_instance = new GameObject[pellets_per_shot];
        for (int i = 0; i < pellets_per_shot; i++)
        {
            line_renderer_instance[i] = Instantiate(line_renderer_prefab);
            line_renderer_instance[i].GetComponent<LineRenderer>().positionCount = 0;
            line_renderer_instance[i].GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
    }

    [ServerRpc]
    void ResetRazerServerRpc()
    {
        for (int i = 0; i < pellets_per_shot; i++)
        {
            line_renderer_instance[i].GetComponent<LineRenderer>().positionCount = 0;
        }
    }

    [ServerRpc]
    void SetLineRendererServerRpc(Vector3 first, Vector3 second,int current_pellet)
    {
        line_renderer_instance[current_pellet].GetComponent<LineRenderer>().positionCount = 2;
        line_renderer_instance[current_pellet].GetComponent<LineRenderer>().SetPosition(0, first);
        line_renderer_instance[current_pellet].GetComponent<LineRenderer>().SetPosition(1, second);
    }
}
