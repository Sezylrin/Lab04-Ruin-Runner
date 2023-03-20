using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // States
    public bool IsInvulnerable { get; private set; }
    [HideInInspector]
    public bool isShielded;
    // Delegate Events
    public static event Action OnDeath;
    // Components
    [Tooltip("Set this to where you want the player to respawn.")]
    public Vector3 spawnPoint;
    [Tooltip("Set this to the shield component in the Player's children if it is not set already.")]
    public GameObject shieldChild;
    public GameObject SpeedChild;
    private PlayerMovement _playerMovement;
    private Collider2D _collider2D;
    public Rigidbody2D _rigidbody2D;
    public LayerMask enemyMask;
    //Timers
    private float _invulnerableTimer;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
    }

    private void Start()
    {
        OnDeath += HandleDeath; // Subscribe to death event/Called when OnDeath is Invoked.
        isShielded = false; // Set back to false at runtime just in case.
        IsInvulnerable = false; // Set back to false at runtime just in case.
    }

    private void Update()
    {
        if (!IsInvulnerable)
        {
            _collider2D.excludeLayers = 0;
            return;
        }
        _invulnerableTimer -= Time.deltaTime;
        IsInvulnerable = _invulnerableTimer > 0;
    }

    public void TakeDamage()
    {
        if (isShielded)
        {
            isShielded = false;
            if (shieldChild) shieldChild.SetActive(false);
            TriggerInvulnerable(5.0f);
        }
        else
        {
            OnDeath?.Invoke();
        }
    }

    private void HandleDeath()
    {
        _playerMovement.canMove = false; // Disable movement input.
        _rigidbody2D.velocity = Vector2.zero; // Edge case of moving while dying.
        _rigidbody2D.isKinematic = true;
        _collider2D.enabled = false;
        if (GameManager.Instance) GameManager.Instance.DecrementLives(); // TODO: change this out for full death handling on GameManager.
        TriggerInvulnerable(999.9f); // Become invulnerable while playing death animation.
        //TODO: Play Death Animation
        //Then Respawn();
        GetComponentInChildren<Animator>().Play("Dying");
        GetComponent<PlayerAnimation>().Override = true;
        Invoke(nameof(Respawn), 2.0f); // TODO: change time to fit animation or remove.
    }

    private void Respawn()
    {
        GetComponent<PlayerAnimation>().Override = false;
        _rigidbody2D.isKinematic = false;
        _collider2D.enabled = true;
        TriggerInvulnerable(3.0f); // Be invulnerable for 3s after respawning to avoid immediate death.
        transform.position = spawnPoint; // Respawn at spawn point. TODO: change to facilitate multiple spawns/different levels.
        _playerMovement.canMove = true; // Enable movement input.
    }

    // Trigger Invulnerability for amount of secs, different for respawn and shield break.
    private void TriggerInvulnerable(float timeInSecs)
    {
        IsInvulnerable = true;
        _collider2D.excludeLayers = enemyMask;
        _invulnerableTimer = timeInSecs;
    }

    public void ClearAction()
    {
        OnDeath = null;
    }
}