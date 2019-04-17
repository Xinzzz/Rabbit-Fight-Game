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
    public float attackRange;

    public override void Start()
    {
        base.Start();
        controller = GetComponent<Controller2D>();
        ChangeState(new IdleState());
    }

    private void FixedUpdate()
    {
        wolfVelocity.y -= gravity * Time.fixedDeltaTime;
        controller.Move(wolfVelocity);
        currentState.Excute();
        if(!attacking)
        {
            LookAtTarget();
        }
        DeadOrNot();
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

    public bool InAttackRange()
    {
        if(target != null)
        {
            return Vector2.Distance(target.transform.position, transform.position) <= attackRange;
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
        anim.Play("IdleW");
    }
    public override IEnumerator TakeDamage(int hurtType)
    {
        //health -= 1;
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

    /*  public void TakeDamage(int hurtType)
      {
          Debug.Log("hurting");
          StartCoroutine(DoHurtAnim(0.3f, hurtType));
          hurting = true;
      }

      IEnumerator DoHurtAnim(float waitTime, int hurtType)
      {
          yield return new WaitForSeconds(waitTime);
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
      }*/
}
