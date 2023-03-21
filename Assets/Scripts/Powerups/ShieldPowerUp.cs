using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
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
        PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();
        _audioSource.Play();
        playerManager.isShielded = true;
        playerManager.shieldChild.SetActive(true);
        Destroy(gameObject, _audioSource.clip.length);
    }
}