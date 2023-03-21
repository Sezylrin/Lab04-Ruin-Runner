using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        CollsionHandling(col);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CollsionHandling(collision);
    }
    private void CollsionHandling(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();
        if (playerManager.IsInvulnerable) return;
        col.gameObject.GetComponent<PlayerManager>().TakeDamage();
        EnemyAI tempAi = GetComponent<EnemyAI>();
        tempAi.SetChase(false);
        tempAi.SetToPatrol();
    }
}