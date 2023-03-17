using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();
        if (playerManager.IsInvulnerable) return;
        col.gameObject.GetComponent<PlayerManager>().TakeDamage();
        Destroy(gameObject);
    }
}