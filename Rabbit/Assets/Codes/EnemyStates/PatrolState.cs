using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;

    //switch btw idle and patrol
    private float patrolTimer;
    private float patrolDuration = 5f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Excute()
    {
        Patrol();
        enemy.EnemeyMovement();
        enemy.EdgeDectection();
        if (enemy.target != null && enemy.InAttackRange())
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if(patrolTimer >= patrolDuration && !enemy.hurting)
        {
            enemy.ChangeState(new IdleState());
        }
    }

}
