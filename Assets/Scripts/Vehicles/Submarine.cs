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
        if (shipCollisionHeight > height)
        {
            return true;
        }
        else return false;
    }
    public override void OnCollideFront()
    {
        Iscollision = true;
    }


    public override void OnCollideUp()
    {
        Iscollision = false;
    }
}
