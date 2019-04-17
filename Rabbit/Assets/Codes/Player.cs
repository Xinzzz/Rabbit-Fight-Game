using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : Character
{
    Controller2D controller;


    //jump variable
    public float jumpHeight = 4f;
    public float timeToJumpApex = .4f;
    float jumpVelocity;

    public Vector3 velocity;


    AttackSystem attack;
    public override void Start()
    {
        base.Start();

        controller = GetComponent<Controller2D>();

        attack = GetComponent<AttackSystem>();
        gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = gravity * timeToJumpApex;
    }

    private void FixedUpdate()
    {
        //Reset Y velocity
        if(controller.collsions.above || controller.collsions.below)
        {
            velocity.y = 0;
        }
        //Input
        if (!attack.attacking)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            //jump and gravity
            if (input.y > 0 && controller.collsions.below)
            {
                velocity.y = jumpVelocity;
            }
            velocity.y -= gravity * Time.fixedDeltaTime;

            velocity.x = input.x * moveSpeed;
            
            //Animations transition btw idle and run
            if(Mathf.Abs(velocity.x) <= 0.001f)
            {
                anim.Play("Idle");
            }
            else
            {
                anim.Play("Run");
            }

            //Movement      
            controller.Move(velocity);


            //flip
            if (input.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (input.x < 0 && facingRight)
            {
                Flip();
            }
        }
        
    }

    public override IEnumerator TakeDamage(int hurtType)
    {
        throw new System.NotImplementedException();
    }

}
