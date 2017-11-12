using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {

    private Enemy enemy;

    private float patrolTimer;
    private float patrolDuration = 5f;

    public void Enter(Enemy enemy)
    {
        patrolDuration = Random.Range(3, 7);
        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();

        enemy.Move();
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

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
