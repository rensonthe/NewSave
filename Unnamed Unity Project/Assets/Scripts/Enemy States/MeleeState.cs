using UnityEngine;
using System.Collections;
using System;

public class MeleeState : IEnemyState
{
    private Enemy enemy;

    private float attackCooldown = 3;
    private static bool canAttack = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        if (!enemy.InMeleeRange && enemy.InThrowRange)
        {
            enemy.Move();
        }
        else if (enemy.Target == null)
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

    private void Attack()
    {
        if (enemy.MeleeTimer >= attackCooldown)
        {
            canAttack = true;
            enemy.MeleeTimer = 0;
        }

        if (canAttack && enemy.InMeleeRange)
        {
            canAttack = false;
            enemy.MyAnimator.SetTrigger("attack");
            enemy.MeleeTimer = 0;
        }
    }
}
