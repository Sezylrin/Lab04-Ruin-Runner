using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator enemyAnim;
    private EnemyAI enemyAi;
    private EnemyMovement enemyMovement;
    private string[] enemyState = new string[] { "Ghost-Up", "Ghost-Right", "Ghost-Left", "Ghost-Down" };
    void Start()
    {
        enemyAnim = GetComponentInChildren<Animator>();
        enemyAi = GetComponent<EnemyAI>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemyAnim)
        {
            Debug.Break();
            Debug.Log("Missing Enemy Animator");
        }
        if (!enemyAi)
        {
            Debug.Break();
            Debug.Log("Missing EnemyAi");
        }
        if (!enemyMovement)
        {
            Debug.Break();
            Debug.Log("Missing Enemy Movement");
        }
        enemyAnim.speed = enemyMovement.GetSpeed() * 0.01f;
        if (enemyAi.GetForwardDirection() == Vector2.up)
            enemyAnim.Play(enemyState[0]);
        if (enemyAi.GetForwardDirection() == Vector2.right)
            enemyAnim.Play(enemyState[1]);
        if (enemyAi.GetForwardDirection() == Vector2.left)
            enemyAnim.Play(enemyState[2]);
        if (enemyAi.GetForwardDirection() == Vector2.down)
            enemyAnim.Play(enemyState[3]);
    }
}
