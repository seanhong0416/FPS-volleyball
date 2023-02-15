using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    public Rigidbody Ball;
    public float ImpactForce = 30f;
    int LayerOfWall;
    int LayerOfGround;
    Vector3 BallResetPosition;

    // Start is called before the first frame update
    void Start()
    {
        LayerOfWall = LayerMask.NameToLayer("Wall");
        LayerOfGround = LayerMask.NameToLayer("Ground");
        Ball.velocity = Vector3.zero;
        BallResetPosition = Ball.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Ball.useGravity && Ball.velocity != Vector3.zero)
            Ball.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerOfWall)
            Ball.AddForce(collision.contacts[0].normal * ImpactForce);
        else if(collision.gameObject.layer == LayerOfGround)
        {
            Ball.position = BallResetPosition;
            Ball.useGravity = false;
            Ball.velocity = Vector3.zero;
        }
    }

}
