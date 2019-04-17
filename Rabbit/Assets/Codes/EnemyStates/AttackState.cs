using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    Enemy enemy;

    float attackTimer = 0;
    float attackCD = 3f;
    bool canAttack = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Excute()
    {
        Attack();
        if(enemy.target != null && !enemy.InAttackRange())
        {
            enemy.EnemeyMovement();
        }
        else if(enemy.target == null)
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

    private void Attack()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer >= attackCD)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if(canAttack)
        {
            canAttack = false;
            enemy.anim.Play("AttackW");
        }
        if(enemy.hurting)
        {
            canAttack = false;
            attackTimer = 0;
        }
    }
}
