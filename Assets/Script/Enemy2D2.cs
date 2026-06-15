using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D2 : EnemyBase
{
    [Header("ジャンプ設定")]
    public float jumpForce = 8f;
    public float jumpCooldown = 1.5f;

    private float lastJumpTime = -1f;

    protected override void Start()
    {
        base.Start();
        mass = 1f; // 標準的な重さ
    }

    protected override void HandleChase()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > 5f)
        {
            currentState = EnemyState.Idle;
            Debug.Log("見失った…");
            return;
        }

        if (isGrounded && Time.time - lastJumpTime > jumpCooldown)
        {
            Vector2 jumpDirection = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(jumpDirection.x * moveSpeed, jumpForce);
            lastJumpTime = Time.time;
        }
    }
}
