using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : BaseVehicle
{
    public GameObject ShipPrefab;
    public Vector3 spawnPosition;
    public Vector3 Direction; // ship Direction
    public float speed = 5.0f; // Ship Speed
    public float shipCollisionHeight = 0.3f; // downheight of ship
    float startTime;
    public bool Iscollision = false;

    void Start()
    {
        startTime = Time.time;
        Direction = GlobalData.shipDirection;
    }

    private void FixedUpdate()
    {
        transform.position += Direction * (speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(this);
    }

    /// <summary>
    /// Front Collided
    /// </summary>
    public void OnCollideFront()
    {
        Iscollision = true;
        Debug.Log("Front Collided");
    }
    /// <summary>
    /// Bottom Collided
    /// </summary>
    public void OnCollideUp()
    {
        Iscollision = true;
        Debug.Log("Bottom Collided");
    }
}

