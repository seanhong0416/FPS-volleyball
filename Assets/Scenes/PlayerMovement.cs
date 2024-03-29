using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;
    public Transform ground_check;

    public float movement_speed = 12f;
    public float gravity = 9.8f;
    public float jumping_velocity = 6f;

    Vector3 velocity = new Vector3(0f, 0f, 0f);
    
    bool on_ground = false;
    public float ground_check_radius = 0.4f;
    public LayerMask ground_mask;

    // Start is called before the first frame update
    void Start()
    {
        ClientPlayerSpawnServerRpc();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (!PauseTransition.isPaused)
        {
            //for horizontal movement
            float movement_x = Input.GetAxis("Vertical");
            float movement_y = Input.GetAxis("Horizontal");

            Vector3 final_movement = transform.right * movement_y + transform.forward * movement_x;

            controller.Move(movement_speed * final_movement * Time.deltaTime);

            //for jumping
            if (on_ground && Input.GetButtonDown("Jump"))
                velocity.y = jumping_velocity;
            controller.Move(velocity * Time.deltaTime);

        }
        //for falling
        velocity.y -= gravity * Time.deltaTime;
        on_ground = Physics.CheckSphere(ground_check.position, ground_check_radius, ground_mask);
        if (on_ground && velocity.y < 0)
            velocity.y = -2;
        controller.Move(velocity * Time.deltaTime);
    }

    [ServerRpc(RequireOwnership =false)]
    void ClientPlayerSpawnServerRpc(ServerRpcParams serverRpcParams = default) {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            client.PlayerObject.GetComponent<CharacterController>().enabled = false;
            client.PlayerObject.transform.position = GameObject.Find("FloorLeft").transform.position + new Vector3(0, 6, 0);
            client.PlayerObject.GetComponent<CharacterController>().enabled = true;
        }
    }

}
