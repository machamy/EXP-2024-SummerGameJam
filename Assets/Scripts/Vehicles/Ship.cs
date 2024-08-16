using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : BaseVehicle
{
    public Vector3 spawnPosition;
    public Vector3 Direction; // ship Direction
    public float speed = 5.0f; // Ship Speed
    public float shipCollisionHeight = 0.3f;
    protected float startTime;

    public bool Iscollision = false;

    protected virtual void Start()
    {
        startTime = Time.time;
        Direction = GlobalData.shipDirection;
    }

    protected virtual void FixedUpdate()
    {   
        transform.position += Direction * (speed * Time.fixedDeltaTime);
    }

    protected virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public virtual bool CollideCheck(float height)
    {
        return (shipCollisionHeight < height);
  
    }

    /// <summary>
    /// Front Collided
    /// </summary>
    public virtual void OnCollideFront()
    {
        Iscollision = true;
        Debug.Log("Front Collided");
    }
    /// <summary>
    /// Middle Collided
    /// </summary>
    public virtual void OnCollideUp()
    {
        Iscollision = true;
        Debug.Log("Middle Collided");
    }
}

