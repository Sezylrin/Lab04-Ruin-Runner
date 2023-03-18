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
        patrol
    };
    public Transform Target;

    private Vector2 targetLocation;
    private Vector2 currentLocation;
    private Vector2 deviatedLocation;
    private int pointInPath;

    [SerializeField]
    private Vector2 forwardDirection;
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private EnemyMovement enemyMovement;
    [SerializeField]
    private int moveState;
    private int prevMoveState;

    public float detectionDistance;
    public float maxDistance;
    public float idleRadius;
    public Vector2[] patrolPoints;

    private Vector2 idlePoint;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        CalculateForward();
        StateDecider();
        if (prevMoveState != moveState)
        {
            prevMoveState = moveState;
            enemyMovement.DeletePath();
        }
        ChooseAction();
        if (moveState == (int)Move_State.idle && !enemyMovement.isPathing())
        {
            timer -= Time.deltaTime;
        }

        //Debug.Log(timer);
    }
    //Calculate enemies facing direction
    private void CalculateForward()
    {
        Vector2 dir = enemyMovement.Direction();
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            forwardDirection = dir.x > 0 ? Vector2.right : Vector2.left;
        }
        else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
        {
            forwardDirection = dir.y > 0 ? Vector2.up : Vector2.down;
        }
    }
    //decides current move state based on logic
    public void StateDecider()
    {
        targetLocation = Target.position;
        currentLocation = this.transform.position;
        Debug.DrawRay(currentLocation, forwardDirection * detectionDistance,Color.blue, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(currentLocation, forwardDirection,detectionDistance,~enemyMask);
        //Debug.Log(hit.collider.gameObject.name);
        if (hit && hit.collider.CompareTag("Player"))
        {
            //Debug.Log("Chasing");
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
}
