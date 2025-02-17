using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Swim : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;                   // Speed of movement
    public float directionChangeInterval = 2f; // How often to choose a new direction
    public float turnSpeed = 15f;
    public Transform headPivot;

    [Header("Boundary Settings")]
    public Vector3 minBounds = new Vector3(-10f, -10f, -10f);
    public Vector3 maxBounds = new Vector3( 10f,  10f,  10f);

    private Vector3 currentDirection;
    private float directionTimer;

    void Start()
    {
        ChooseNewDirection();
    }

    void Update()
    {
         // Movement
        transform.Translate(currentDirection * speed * Time.deltaTime, Space.World);

        //  Or smoothly:
        Quaternion targetRot = Quaternion.LookRotation(currentDirection);
        headPivot.rotation = Quaternion.Slerp(headPivot.rotation, targetRot, turnSpeed * Time.deltaTime);

         // Timer & bounds
         directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f) ChooseNewDirection();
             CheckBoundsAndReflectIfNeeded();
    }

    void ChooseNewDirection()
    {
        // Pick a random 3D direction (unit sphere)
        currentDirection = Random.insideUnitSphere.normalized;
        directionTimer = directionChangeInterval;
    }

    void CheckBoundsAndReflectIfNeeded()
    {
        Vector3 pos = transform.position;

        // We'll store which axes are out of bounds and then reflect those components.
        bool hitBoundary = false;

        // X bounds
        if (pos.x < minBounds.x)
        {
            pos.x = minBounds.x;                 // Clamp position
            currentDirection.x = -currentDirection.x;  // Reflect direction
            hitBoundary = true;
        }
        else if (pos.x > maxBounds.x)
        {
            pos.x = maxBounds.x;
            currentDirection.x = -currentDirection.x;
            hitBoundary = true;
        }

        // Y bounds
        if (pos.y < minBounds.y)
        {
            pos.y = minBounds.y;
            currentDirection.y = -currentDirection.y;
            hitBoundary = true;
        }
        else if (pos.y > maxBounds.y)
        {
            pos.y = maxBounds.y;
            currentDirection.y = -currentDirection.y;
            hitBoundary = true;
        }

        // Z bounds
        if (pos.z < minBounds.z)
        {
            pos.z = minBounds.z;
            currentDirection.z = -currentDirection.z;
            hitBoundary = true;
        }
        else if (pos.z > maxBounds.z)
        {
            pos.z = maxBounds.z;
            currentDirection.z = -currentDirection.z;
            hitBoundary = true;
        }

        // Apply corrected position
        transform.position = pos;

        // If you prefer to re-roll a brand-new random direction upon hitting any boundary,
        // just uncomment the line below instead of reflecting:
        //
        // if (hitBoundary) ChooseNewDirection();
    }
}