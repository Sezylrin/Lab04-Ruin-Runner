using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [Tooltip("Change this to how much you want the player to speed up when collecting this object.")]
    public float addedSpeed;
    public float Duration;

    private PlayerMovement _playerMovement;
    private PlayerManager playerManager;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
            return;
        _playerMovement = col.gameObject.GetComponent<PlayerMovement>();
        _playerMovement.moveSpeed += addedSpeed;
        playerManager = col.gameObject.GetComponent<PlayerManager>();
        playerManager.SpeedChild.SetActive(true);
        Invoke(nameof(ResetSpeedBuff), Duration);
        gameObject.SetActive(false);
    }

    private void ResetSpeedBuff()
    {
        playerManager.SpeedChild.SetActive(false);
        _playerMovement.moveSpeed -= addedSpeed;
        Destroy(gameObject);
    }
}