using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : IEnemyState
{
    private Enemy enemy;

    //switch btw idle and patrol
    private float HurtTimer;
    private float HurtDuration = 0.5f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Excute()
    {
        
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }
}
