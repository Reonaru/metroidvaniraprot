using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        mass = 2f; // 重い敵
    }

    protected override void HandleChase()
    {
        float step = moveSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, player.position);

        // moveRange が大きいので遠くから追う
        if (distance > moveRange)
        {
            currentState = EnemyState.Idle;
            Debug.Log("見失った…");
            return;
        }

        // 速く追う（moveSpeed が高い）
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }
}
