using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : BaseVehicle
{
    public float shipCollisionHeight = 0.3f;
    public bool Iscollision = false;
    

    protected virtual void FixedUpdate()
    {   
        base.FixedUpdate();
        // transform.position += Direction * (speed * Time.fixedDeltaTime);
        if (state == State.Stop)
        {
            state = State.After;
        }
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
        if(isDead) return;
        Iscollision = true;
        isDead = true;
        Debug.Log("Front Collided");
    }
    /// <summary>
    /// Middle Collided
    /// </summary>
    public virtual void OnCollideUp()
    {
        if (isDead) return;
        Iscollision = true;
        isDead = true;
        Debug.Log("Middle Collided");
    }
}

