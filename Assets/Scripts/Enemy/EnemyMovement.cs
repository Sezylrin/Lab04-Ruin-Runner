using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{

    private float speed;

    public float BaseSpeed;
    public float nextWayPointDistance = 0.5f;
    private Vector2 currentTarget;

    Path path;
    int CurrentWaypoint = 0;
    bool startedPathing = false;
    bool isMoving = false;
    Vector2 dir;

    Seeker seeker;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        speed = BaseSpeed;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, (Vector3)currentTarget, OnPathComplete);
    }

    public void StartPathing()
    {
        startedPathing = true;
        InvokeRepeating("UpdatePath", 0f, .5f);
        CurrentWaypoint = 0;
    }

    public void CancelPathing()
    {
        CancelInvoke("UpdatePath");
        startedPathing = false;
    }

    void OnPathComplete(Path P)
    {
        if (!P.error)
        {
            path = P;
            CurrentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(startedPathing);
        if (startedPathing) 
        Pathfinding();

    }

    private void Pathfinding()
    {
        if (path == null)
            return;
        if (CurrentWaypoint >= path.vectorPath.Count)
        {
            //Debug.Log(CurrentWaypoint + " " + path.vectorPath.Count);
            DeletePath();
            return;
        }
        isMoving = true;
        dir = ((Vector2)path.vectorPath[CurrentWaypoint] - rb.position).normalized;
        Vector2 force = dir * speed * Time.deltaTime;
        //Debug.Log(direction);
        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[CurrentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            CurrentWaypoint++;
        }
    }

    public void SetTarget(Vector2 target)
    {
        currentTarget = target;
    }

    public bool isPathing()
    {
        return startedPathing;
    }

    public bool currentlyPathing()
    {
        return isMoving;
    }

    public Vector2 Direction()
    {
        return dir;
    }
    public void DeletePath()
    {
        isMoving = false;
        rb.velocity = Vector2.zero;
        CancelPathing();
        path = null;
    }
    public void SetSpeed(float Speed)
    {
        speed = Speed;
    }
    public float GetSpeed()
    {
        return speed;
    }
}
