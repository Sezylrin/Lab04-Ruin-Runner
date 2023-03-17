using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("How fast the player moves")]
    public float moveSpeed;
    [Tooltip("Allows for movement in all directions.")]
    public bool allowDiagonalMovement;
    [HideInInspector]
    public bool canMove;

    private Rigidbody2D _rb;
    private Vector2 _movement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        canMove = true; // Enable movement input
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (!canMove) return; // Return if Input is disabled
        if (_movement.y > 0)
            _rb.velocity = Vector2.up * moveSpeed;
        else if (_movement.y < 0)
            _rb.velocity = Vector2.down * moveSpeed;
        else if (_movement.x > 0)
            _rb.velocity = Vector2.right * moveSpeed;
        else if (_movement.x < 0)
            _rb.velocity = Vector2.left * moveSpeed;
        else _rb.velocity = Vector2.zero;

        if (allowDiagonalMovement) _rb.velocity = _movement.normalized * moveSpeed; // Simpler/smoother movement with whole vector if toggled.
    }
}