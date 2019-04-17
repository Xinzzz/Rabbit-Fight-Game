using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    //switch btw idle and patrol
    private float idleTimer;
    private float idleDuration = 2f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Excute()
    {
        Idle();
        if(enemy.target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void Idle()
    {
        if(!enemy.hurting)
        {
            enemy.anim.Play("IdleW");
            idleTimer += Time.deltaTime;
            enemy.wolfVelocity.x = 0;
            if (idleTimer >= idleDuration)
            {
                enemy.ChangeState(new PatrolState());
            }
        }     
    }
}
