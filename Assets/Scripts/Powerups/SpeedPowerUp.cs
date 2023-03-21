using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [Tooltip("Change this to how much you want the player to speed up when collecting this object.")]
    public float addedSpeed;
    [Tooltip("Change this to how long you want the player to speed up when collecting this object.")]
    public float duration;

    private PlayerMovement _playerMovement;
    private PlayerManager _playerManager;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private bool _collected;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || _collected) return;
        _collected = true;
        _spriteRenderer.enabled = false;
        _audioSource.Play();
        _playerMovement = col.gameObject.GetComponent<PlayerMovement>();
        _playerMovement.moveSpeed += addedSpeed;
        _playerManager = col.gameObject.GetComponent<PlayerManager>();
        _playerManager.speedChild.SetActive(true);
        Invoke(nameof(ResetSpeedBuff), duration);
    }

    private void ResetSpeedBuff()
    {
        _playerManager.speedChild.SetActive(false);
        _playerMovement.moveSpeed -= addedSpeed;
        Destroy(gameObject);
    }
}