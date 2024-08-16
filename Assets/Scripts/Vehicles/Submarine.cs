using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : Ship
{
    new public float shipCollisionHeight = 0.3f;
    override protected void Start()
    {
        base.Start();
    }

    public override bool CollideCheck(float height)
    {
        return shipCollisionHeight > height;
    }
    public override void OnCollideFront()
    {
        Iscollision = true;
        Debug.Log("Front Collided");
    }


    public override void OnCollideUp()
    {
        Iscollision = true;
        Debug.Log("Middle Collided");
    }
}
