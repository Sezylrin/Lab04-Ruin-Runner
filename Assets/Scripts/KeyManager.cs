using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private bool _collected;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_collected)
        {
            _collected = true;
            _spriteRenderer.enabled = false;
            GameManager.Instance.CollectKey();
            _audioSource.Play();
            Destroy(gameObject, _audioSource.clip.length);
        }
    }
}