using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : Character
{
    Controller2D controller;

    SpriteRenderer sprite;

    //jump variable
    public float jumpHeight = 4f;
    public float timeToJumpApex = .4f;
    float jumpVelocity;

    public GameObject runDust;
    public Transform runDustPoint;

    public Transform jumpDustPoint;

    public Vector3 velocity;

    bool immortal = false;

    [SerializeField]
    float immortalTime;
    [SerializeField]
    public float knockBackForce;

    float dieFlyForce = 1f;
    bool dying = false;

    public bool knockBackFromRight;   
    AttackSystem attack;



    public override void Start()
    {
        base.Start();
        
        controller = GetComponent<Controller2D>();
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<AttackSystem>();
        gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = gravity * timeToJumpApex;
    }

    private void Update()
    {
        DeadOrNot();
        if(isDead)
        {
            anim.Play("Die");
            gameObject.layer = 0;
            if (dying)
            {
                Debug.Log("flyyy");
                if (knockBackFromRight)
                {
                    velocity.x -= dieFlyForce * Time.fixedDeltaTime;
                    controller.Move(velocity);
                }
                else
                {
                    velocity.x += dieFlyForce * Time.fixedDeltaTime;
                    controller.Move(velocity);
                }
            }
        }
        //Reset Y velocity
        if (controller.collsions.above || controller.collsions.below)
        {
            velocity.y = 0;
        }
       
        //Input
        if(!isDead)
        {
            if (!attack.attacking && !hurting)
            {
                Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                //jump and gravity
                if (input.y > 0 && controller.collsions.below)
                {
                    velocity.y = jumpVelocity;
                    JumpEffect();
                }
                velocity.y -= gravity * Time.fixedDeltaTime;

                velocity.x = input.x * moveSpeed;

                //Animations transition btw idle and run and jump
                if(controller.collsions.below)
                {
                    if (Mathf.Abs(velocity.x) <= 0.001f)
                    {
                        anim.Play("Idle");
                    }
                    else
                    {
                        anim.Play("Run");
                    }
                }
                else
                {
                    anim.Play("Jump");
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
            //knock back
            else if(hurting)
            {
                if (knockBackFromRight)
                {
                    velocity.x -= knockBackForce * Time.fixedDeltaTime;
                    controller.Move(velocity);
                }
                else
                {
                    velocity.x += knockBackForce * Time.fixedDeltaTime;
                    controller.Move(velocity);
                }
            }
        }
        


    }

    IEnumerator IndicateImmortal()
    {
        while(immortal)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RunEffect()
    {
        Instantiate(runDust, runDustPoint.position, Quaternion.identity);
    }

    public void JumpEffect()
    {
        Instantiate(runDust, jumpDustPoint.position, Quaternion.identity);
    }
    public override IEnumerator TakeDamage(int hurtType)
    {
        if(!immortal)
        {
            health -= 1;
            Debug.Log(health);
            if (!isDead && health >= 1)
            {
                anim.Play("Hurt");             
                immortal = true;

                yield return new WaitForSeconds(0.4f);
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
        }     
    }


    public void StartHurting()
    {
        hurting = true;
        velocity.x = 0;
        controller.Move(velocity);
    }

    public void StopHurting()
    {
        hurting = false;
        anim.Play("Idle");
        attack.attacking = false;
    }

    public void StartDie()
    {
        dying = true;
    }
    public void EndDie()
    {
        dying = false;
    }

}
