using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField, Tooltip("Causes enemy to die when colliding with a shielded player")]
    private bool dieOnShieldedCollision;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();
        if (playerManager.IsInvulnerable) return;
        if (playerManager.isShielded) HandleShieldedCollision();
        col.gameObject.GetComponent<PlayerManager>().TakeDamage();
    }

    private void HandleShieldedCollision()
    {
        if (!dieOnShieldedCollision) return;
        //Play death animation
        Destroy(gameObject);
    }
}