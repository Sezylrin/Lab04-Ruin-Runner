using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public enum Move_State : int
    {
        idle,
        chasing,
        patrol,
        stop
    };
    public enum Enemy_Type : int
    {
        Normal,
        Special
    }
    public Transform Target;

    public bool debugging = false;

    private Vector2 targetLocation;
    private Vector2 currentLocation;
    private Vector2 deviatedLocation;
    private int pointInPath;
    
    [SerializeField]
    private Enemy_Type EnemyType;
    [SerializeField]
    private Vector2 forwardDirection;
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private EnemyMovement enemyMovement;
    [SerializeField]
    private int moveState;
    private int prevMoveState;
    private bool allowChase;
    private Rigidbody2D rb;

    public float detectionDistance;
    public float maxDistance;
    private float idleRadius;
    public float chaseSpeedMultiplier;
    public Vector2[] patrolPoints;

    private Vector2 idlePoint;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        allowChase = true;
        GameObject Grid = GameObject.Find("Astargrid");
        if (!Grid)
        {
            Debug.Log("Please place an Astargrid in scene");
            Debug.Break();
        }
        Target = GameObject.Find("Player").transform;
        if (!Target)
            Debug.Log("please drag player transform into target in enemy ai on enemy objects");
        prevMoveState = 0;
        pointInPath = 0;
        idlePoint = this.transform.position;
        moveState = (int)Move_State.patrol;
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerManager.OnDeath += SetToPatrol;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateForward();
        StateDecider();
        if (prevMoveState != moveState)
        {
            prevMoveState = moveState;
            enemyMovement.DeletePath();
        }
        ChooseAction();
        if (EnemyType != Enemy_Type.Special)
            return;
        if (!allowChase && timer <= 0)
            timer = 3;
        if (timer > 0)
            timer -= Time.deltaTime;
        if (timer <= 0)
            allowChase = true;
        //Debug.Log(timer);
    }
    //Calculate enemies facing direction
    private void CalculateForward()
    {
        Vector2 dir = enemyMovement.Direction();
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0.1f)
                forwardDirection = Vector2.right;
            else if (dir.x < -0.1f)
                forwardDirection = Vector2.left;
        }
        else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
        {
            if (dir.y > 0.1f)
                forwardDirection = Vector2.up;
            else if (dir.y < -0.1f)
                forwardDirection = Vector2.down;
        }
    }
    //decides current move state based on logic
    public void StateDecider()
    {
        
        targetLocation = Target.position;
        currentLocation = transform.position;
        if (EnemyType == Enemy_Type.Special)
        {
            Debug.Log(allowChase);
            if (GameManager.Instance != null && GameManager.Instance.level == GameManager.Instance.keysCollected)
                enemyMovement.SetSpeed(enemyMovement.BaseSpeed * chaseSpeedMultiplier);
            if (allowChase)
                moveState = (int)Move_State.chasing;
            else
                moveState = (int)Move_State.stop;
            return;
        }
        //Debug.DrawRay(currentLocation, forwardDirection * detectionDistance,Color.blue, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(currentLocation, forwardDirection,detectionDistance,~enemyMask);
        //Debug.Log(hit.collider.gameObject.name);
        if (debugging && hit && hit.collider.CompareTag("Player"))
            Debug.Log(allowChase + " " + !hit.collider.GetComponent<PlayerManager>().IsInvulnerable);
        if (hit && hit.collider.CompareTag("Player") && allowChase && !hit.collider.GetComponent<PlayerManager>().IsInvulnerable)
        {
            enemyMovement.SetSpeed(enemyMovement.BaseSpeed * chaseSpeedMultiplier);
            if (deviatedLocation == Vector2.zero)
                deviatedLocation = currentLocation;
            if (CalculateDistance(deviatedLocation, currentLocation) <= maxDistance)
                moveState = (int)Move_State.chasing;
        }
        if (CalculateDistance(deviatedLocation, currentLocation) > maxDistance)
        {
            //Debug.Log("return to patrol");
            deviatedLocation = Vector2.zero;
            moveState = (int)Move_State.patrol;
        }
        /*else if (CalculateDistance(targetLocation, idlePoint) >= maxDistance)
        {
            moveState = (int)Move_State.idle;
        }*/
    }
    //based on move state decide which method to run
    public void ChooseAction()
    {
        switch (moveState)
        {
            case (int)Move_State.idle:
                Idle();
                break;
            case (int)Move_State.chasing:
                Chase();
                break;
            case (int)Move_State.patrol:
                Patrol();
                break;

        }
    }
    //Patrol between points
    public void Patrol()
    {
        enemyMovement.SetSpeed(enemyMovement.BaseSpeed);
        if (patrolPoints.Length == 0)
        {
            Debug.Log("Please Set Patrol Points");
            Debug.Break();
            return;
        }
        if (!enemyMovement.isPathing() && !enemyMovement.currentlyPathing())
        {
            enemyMovement.SetTarget(patrolPoints[pointInPath]);
            pointInPath++;
            pointInPath %= patrolPoints.Length;
            enemyMovement.StartPathing();
        }
        
        if (CalculateDistance(currentLocation, patrolPoints[pointInPath]) < maxDistance * 0.5)
            allowChase = true;
    }
    // Chases target 
    public void Chase()
    {
        if (!enemyMovement.isPathing() && !enemyMovement.currentlyPathing())
        {
            enemyMovement.StartPathing();
        }
        enemyMovement.SetTarget(targetLocation);
    }
    //idle wandering
    public void Idle()
    {
        //determines current position, if too far and in idle state will then return to idlepoint
        if (CalculateDistance(currentLocation, idlePoint) > idleRadius)
        {
            if (!enemyMovement.isPathing())
            {
                enemyMovement.StartPathing();
            }
            enemyMovement.SetTarget(idlePoint);
        }
        //random point inside of a circle with center at idlepoint which the enemy will move to, random timer  between 1 and 5 seconds before moving again
        else if (timer <= 0)
        {
            enemyMovement.StartPathing();
            enemyMovement.SetTarget((Random.insideUnitCircle * idleRadius) + idlePoint);
            timer = (float)Random.Range(100, 600) / 100f;
        }
        else
        {
            return;
        }
    }
    public float CalculateDistance(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }

    public void SetToPatrol()
    {
        moveState = (int)Move_State.patrol;
    }

    public Vector2 GetForwardDirection()
    {
        return forwardDirection;
    }

    public void SetChase(bool toChase)
    {
        allowChase = toChase;
    }
}