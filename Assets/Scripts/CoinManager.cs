using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // [SerializeField, Tooltip("First clip is collect coin, Second clip is key spawn")]
    // private List<AudioClip> audioClips;
    [SerializeField] private AudioClip keySpawnAudioClip;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private bool _collected;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_collected)
        {
            _collected = true;
            _spriteRenderer.enabled = false;
            GameManager.Instance.CollectCoin(collision.gameObject.transform.position);
            if (GameManager.Instance.CheckKeyCanSpawn()) _audioSource.clip = keySpawnAudioClip;
            _audioSource.Play();
            Destroy(gameObject, _audioSource.clip.length);
        }
    }
}