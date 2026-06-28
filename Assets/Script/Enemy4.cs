using System.Collections;
using UnityEngine;

public class Enemy4 : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        hp = 50;
        moveSpeed = 2f;
        moveRange = 5f;
        mass = 0.5f;
    }

    protected override void HandleChase()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
