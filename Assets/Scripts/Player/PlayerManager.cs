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

    #region Components
        [Tooltip("Set this to where you want the player to respawn.")]
        public Vector3 spawnPoint;
        [Tooltip("Set this to the speed component in the Player's children if it is not set already.")]
        public GameObject speedChild;
        private Collider2D _collider2D;
        private Rigidbody2D _rigidbody2D;
        private AudioSource _audioSource;
    #endregion

    // Globals
    public LayerMask enemyMask;
    private PlayerMovement _playerMovement;

    //Timers
    private float _invulnerableTimer;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        spawnPoint = transform.position;
    }

    private void Start()
    {
        OnDeath += HandleDeath; // Subscribe to death event/Called when OnDeath is Invoked.
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
        Shield shield = GetComponentInChildren<Shield>();
        if (shield)
        {
            StartCoroutine(shield.BreakShield());
            TriggerInvulnerable(3f);
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
        _audioSource.Play();
        if (GameManager.Instance) GameManager.Instance.DecrementLives();
        TriggerInvulnerable(999.9f); // Become invulnerable while playing death animation.
        GetComponentInChildren<Animator>().Play("Dying");
        GetComponent<PlayerAnimation>().Override = true;
        Invoke(nameof(Respawn), 2.0f);
    }

    private void Respawn()
    {
        GetComponent<PlayerAnimation>().Override = false;
        _rigidbody2D.isKinematic = false;
        _collider2D.enabled = true;
        TriggerInvulnerable(3.0f); // Be invulnerable for 3s after respawning to avoid immediate death.
        transform.position = spawnPoint;
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