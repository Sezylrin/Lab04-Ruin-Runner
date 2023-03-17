using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();
        playerManager.isShielded = true;
        playerManager.shieldChild.SetActive(true);
        Destroy(gameObject);
    }
}