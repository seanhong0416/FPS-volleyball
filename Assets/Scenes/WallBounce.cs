using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WallBounce : NetworkBehaviour
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
        //ball hit wall, bounce back harder
        if(collision.gameObject.layer == LayerOfWall)
            Ball.AddForce(collision.contacts[0].normal * ImpactForce);
        //ball touch ground => reset ball and add score
        else if(collision.gameObject.layer == LayerOfGround)
        {
            Ball.position = BallResetPosition;
            Ball.useGravity = false;
            Ball.velocity = Vector3.zero;
            //add score
            if (collision.gameObject.CompareTag("OrangeCat"))
            {
                Scoring.ScoreOrangeCats.Value += 1;
            }
            else
            {
                Scoring.ScoreBoys.Value += 1;
            }
        }
    }

}
