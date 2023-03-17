using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    public float addedSpeed;

    private PlayerMovement _playerMovement;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
            return;
        _playerMovement = col.gameObject.GetComponent<PlayerMovement>();
        _playerMovement.moveSpeed += addedSpeed;
        Invoke(nameof(ResetSpeedBuff), 3.0f);
        gameObject.SetActive(false);
    }

    private void ResetSpeedBuff()
    {
        _playerMovement.moveSpeed -= addedSpeed;
        Destroy(gameObject);
    }
}