using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : Ship
{
    public float submarineCollisionHeight = 0.3f;

    public override bool CollideCheck(float height)
    {
        return submarineCollisionHeight > height;
    }
    public override void OnCollideFront()
    {
        Iscollision = true;
        Debug.Log("Submarine Front Collided");
    }


    public override void OnCollideUp()
    {
        Iscollision = true;
        Debug.Log("Submarine Middle Collided");
    }
}
