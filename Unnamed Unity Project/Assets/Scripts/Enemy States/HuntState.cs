using UnityEngine;
using System.Collections;
using System;

public class HuntState : IEnemyState
{
    private Enemy enemy;

    private float patrolTimer;
    private float patrolDuration;

    public void Enter(Enemy enemy)
    {
        enemy.moveSpeed = enemy.moveSpeed / 1;
        patrolDuration = 3;
        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();

        enemy.Move();

        if (enemy.Target != null && enemy.InThrowRange)
        {
            enemy.SetState();
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
        enemy.MyAnimator.SetFloat("speed", 1);

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
