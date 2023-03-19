using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator PlayerAnim;

    private Rigidbody2D rb;

    private string[] PlayerState = new string[] { "Side-Walk-Left", "Side-Walk-Right", "Up-Walk", "Down-Walk" };
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        PlayerAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = rb.velocity;
        if (dir.magnitude <= 0.01f)
        {
            PlayerAnim.SetBool("Idle", true);
            return;
        }
        PlayerAnim.SetBool("Idle", false);
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            PlayerAnim.Play(dir.x > 0 ? PlayerState[1] : PlayerState[0]);
        }
        else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
        {
            PlayerAnim.Play(dir.y > 0 ? PlayerState[2] : PlayerState[3]);
        }
    }
}
