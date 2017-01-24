using UnityEngine;
using System.Collections;
using System;

public class RangedState : IEnemyState {

    private Enemy enemy;

    private float throwCooldown = 5;
    private static bool canThrow = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Throw();

        if(enemy.Target != null)
        {
            if (!enemy.InThrowRange)
            {
                enemy.Move();
            }
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Throw()
    {
        if(enemy.RangedTimer >= throwCooldown)
        {
            canThrow = true;
            enemy.RangedTimer = 0;
        }

        if (canThrow && enemy.InThrowRange)
        {
            canThrow = false;
            enemy.MyAnimator.SetTrigger("throw");
        }
    }
}
