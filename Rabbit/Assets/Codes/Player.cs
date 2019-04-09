using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    Controller2D controller;

    public float moveSpeed = 6f;

    //jump variable
    public float jumpHeight = 4f;
    public float timeToJumpApex = .4f;
    float gravity;
    float jumpVelocity;

    Vector3 velocity;

    //flip character
    bool facingRight = false;
    SpriteRenderer sprite;

    //animations
    Animator anim;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

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
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //jump and gravity
        if(input.y > 0 && controller.collsions.below)
        {
            velocity.y = jumpVelocity;
        }
        velocity.y -= gravity * Time.fixedDeltaTime;

        velocity.x = input.x * moveSpeed;
        anim.SetFloat("SpeedX", Mathf.Abs(input.x));
        controller.Move(velocity);

        //flip
        if(input.x > 0 && !facingRight)
        {
            Flip();
        }
        else if(input.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 charScale = transform.localScale;
        charScale.x *= -1;
        transform.localScale = charScale;
    }
}
