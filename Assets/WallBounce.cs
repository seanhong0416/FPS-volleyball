using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    public Rigidbody Ball;
    public float ImpactForce = 30f;
    int LayerOfWall;

    // Start is called before the first frame update
    void Start()
    {
        LayerOfWall = LayerMask.NameToLayer("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerOfWall)
            Ball.AddForce(collision.contacts[0].normal * ImpactForce);
    }

}
