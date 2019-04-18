using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Enemy : Character
{
    public Vector2 wolfVelocity;
    Controller2D controller;

    public Transform groundInfo;
    public float dectectGroundDistance;

    private IEnemyState currentState;

    public GameObject target;

    [SerializeField]
    public float inAttackRange;
    [SerializeField]
    public float attackRange;
    public Transform attackPos;
    public LayerMask whatIsPlayer;

    Player player;

    public bool hitFromRight;
    public float hitForce;

    public float dizzyTime = 1f;

    public GameObject hitEffect;
    public Transform hitPos;

    public override void Start()
    {
        base.Start();
        controller = GetComponent<Controller2D>();
        player = FindObjectOfType<Player>();
        ChangeState(new IdleState());
    }

    private void FixedUpdate()
    {
        dizzyTime -= Time.fixedDeltaTime;
        if(!isDead)
        {
            if(!hurting)
            {
                wolfVelocity.y -= gravity * Time.fixedDeltaTime;
                controller.Move(wolfVelocity);
                currentState.Excute();
                if (!attacking)
                {
                    LookAtTarget();
                }
            }
            //knock back
            else if (hurting)
            {
                if (hitFromRight)
                {
                    wolfVelocity.x -= hitForce * Time.fixedDeltaTime;
                    controller.Move(wolfVelocity);
                }
                else
                {
                    wolfVelocity.x += hitForce * Time.fixedDeltaTime;
                    controller.Move(wolfVelocity);
                }
            }
        }
        DeadOrNot();
        if (isDead)
        {
            anim.Play("DieW");
        }
        if(player.isDead)
        {
            RemoveTarget();
        }
    }

    private void LookAtTarget()
    {
        if (target != null)
        {
            float xDir = target.transform.position.x - transform.position.x;

            if ((xDir > 0 && !facingRight) || (xDir < 0 && facingRight))
            {
                Flip();
            }
        }
    }

    public void RemoveTarget()
    {
        target = null;
        ChangeState(new PatrolState());
    }

    public bool InAttackRange()
    {
        if(target != null)
        {
            return Vector2.Distance(target.transform.position, transform.position) <= inAttackRange;
        }
        else
        {
            return false;
        }
    }

    public void EnemeyMovement()
    {
        if(!attacking && !hurting)
        {
            anim.Play("RunW");
            wolfVelocity = GetDir() * moveSpeed * Time.fixedDeltaTime;
            controller.Move(wolfVelocity);
        }     
    }

    public Vector2 GetDir()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public void EdgeDectection()
    {
        RaycastHit2D ground = Physics2D.Raycast(groundInfo.position, Vector2.down, dectectGroundDistance);
        if (ground.collider == null)
        {
            Flip();
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }


    public void StartAttack()
    {
        attacking = true;
        wolfVelocity.x = 0;
        controller.Move(wolfVelocity);

    }

    public void WolfAttack()
    {
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer);
        for (int i = 0; i < playerToDamage.Length; i++)
        {
            Instantiate(hitEffect, hitPos.position, Quaternion.identity);
            playerToDamage[i].GetComponent<Player>().StartCoroutine(playerToDamage[i].GetComponent<Player>().TakeDamage(1));
            if(playerToDamage[i].GetComponent<Player>().transform.position.x < transform.position.x)
            {
                playerToDamage[i].GetComponent<Player>().knockBackFromRight = true;
                if (!playerToDamage[i].GetComponent<Player>().facingRight)
                {
                    playerToDamage[i].GetComponent<Player>().Flip();
                    
                }
            }
            else
            {
                playerToDamage[i].GetComponent<Player>().knockBackFromRight = false;
                if (playerToDamage[i].GetComponent<Player>().facingRight)
                {
                    playerToDamage[i].GetComponent<Player>().Flip();
                    
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

    }

    public void StopAttack()
    {
        attacking = false;
        anim.Play("IdleW"); 
    }

    public void StartHurting()
    {
        hurting = true;
        wolfVelocity.x = 0;
        controller.Move(wolfVelocity);
    }

    

    public void StopHurting()
    {
        hurting = false;
        ChangeState(new IdleState());
        dizzyTime = 1f;
    }
    public override IEnumerator TakeDamage(int hurtType)
    {
        health -= 1;
        if(!isDead)
        {
            if (hurtType == 1)
            {
                anim.Play("HurtW02");
            }
            else if (hurtType == 2)
            {
                anim.Play("HurtW01");
            }
            else if (hurtType == 3)
            {
                anim.Play("HurtW03");
            }

        }
        else
        {
            yield return null;
        }
    }

   
}
