using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator PlayerAnim;

    private Rigidbody2D rb;

    private PlayerManager playerManager;

    private float Timer;

    public float FPS;

    public Transform Speed;

    public bool Override;

    private int layer = 0;

    private string[] PlayerState = new string[] { "Side-Walk-Left", "Side-Walk-Right", "Up-Walk", "Down-Walk" };
    void Start()
    {
        Override = false;
        rb = GetComponentInChildren<Rigidbody2D>();
        PlayerAnim = GetComponentInChildren<Animator>();
        playerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManager.IsInvulnerable)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                if (layer == 0)
                {
                    PlayerAnim.SetLayerWeight(1, 0);
                }
                if (layer == 1)
                {
                    PlayerAnim.SetLayerWeight(1, 1);
                }

                Timer = FPS;
                layer++;
                layer %= 2;
            }
        }
        else
        {
            PlayerAnim.SetLayerWeight(1, 0);
        }
        if (Override)
            return;
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
            Speed.eulerAngles = dir.x > 0 ? new Vector3(180, 90, -180) : new Vector3(0, 90, -180);
        }
        else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
        {
            PlayerAnim.Play(dir.y > 0 ? PlayerState[2] : PlayerState[3]);
            Speed.eulerAngles = dir.y > 0 ? new Vector3(90, 90, -90) : new Vector3(-90, 90, -90);
        }
        
    }
}