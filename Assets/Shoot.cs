using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float fire_range = 15f;
    public Camera character;
    public float impact_force = 15f;
    public float fire_rate = 15f;
    public LineRenderer line_renderer;
    public Transform gun;

    float next_fire_time = 0f;
    //int line_render_start = 0;
    //int line_render_end = 1;
    //Vector3 line_render_start_point;
    //Vector3 line_render_end_point;

    // Start is called before the first frame update
    void Start()
    {
        line_renderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= next_fire_time)
        {
            fire();
            next_fire_time = Time.time + 1 / fire_rate;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            line_renderer.positionCount = 0;
        }
    }

    void fire()
    {
        RaycastHit hit;

        if (Physics.Raycast(character.transform.position, character.transform.forward, out hit)){
            Debug.Log(hit.transform.name);
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impact_force);
            }

            line_renderer.positionCount = 2;
            line_renderer.SetPosition(0, gun.position);
            line_renderer.SetPosition(1, hit.point - line_renderer.transform.position);
           
        }
        else
        {

        }
    }
}
