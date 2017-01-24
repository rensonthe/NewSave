using UnityEngine;
using System.Collections;
using System;

public class RangedMeleeState : IEnemyState
{
    private Enemy enemy;

    private float throwCooldown = 5;
    private static bool canThrow = true;
    private float attackCooldown = 3;
    private static bool canAttack = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Throw();
        Attack();

        if (enemy.Target != null && !enemy.InMeleeRange)
        {
            enemy.Move();
        }
        else
        {
            if (enemy.Target == null)
            {
                enemy.ChangeState(new IdleState());
            }
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
        if (enemy.RangedTimer >= throwCooldown)
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
