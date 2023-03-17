using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // States
    public bool IsInvulnerable { get; private set; }
    // Delegate Events
    public static event Action OnDeath;
    // Components
    public Vector3 spawnPoint;
    private PlayerMovement _playerMovement;

    private SpriteRenderer _spriteRenderer;
    //Timers
    private float _invulnerableTimer;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        OnDeath += HandleDeath;
        IsInvulnerable = false;
    }

    private void Update()
    {
        _spriteRenderer.color = IsInvulnerable ? Color.blue : Color.red;
        if (!IsInvulnerable) return;
        _invulnerableTimer -= Time.deltaTime;
        IsInvulnerable = _invulnerableTimer > 0;
    }

    public void TakeDamage()
    {
        OnDeath?.Invoke();
    }

    private void HandleDeath()
    {
        _playerMovement.canMove = false;
        GameManager.Instance.DecrementLives();
        TriggerInvulnerable(999.9f);
        //Play Death Animation
        //Then Respawn();
        Invoke(nameof(Respawn), 3.0f);
    }
    private void Respawn()
    {
        TriggerInvulnerable(3.0f);
        transform.position = spawnPoint;
        _playerMovement.canMove = true;
    }

    private void TriggerInvulnerable(float timeInSecs)
    {
        IsInvulnerable = true;
        _invulnerableTimer = timeInSecs;
    }
}