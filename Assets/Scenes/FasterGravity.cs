using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterGravity : MonoBehaviour
{
    Vector3 velocity = new Vector3(0f, 0f, 0f);
    bool on_ground = false;
    public LayerMask ground_mask;
    public float gravity = 4.9f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        on_ground = Physics.CheckSphere(transform.position, transform.lossyScale.y/2, ground_mask);
        if (on_ground && velocity.y <=0)
        {
            velocity.y = -2;
        }
        transform.position += velocity * Time.deltaTime;
    }
}
