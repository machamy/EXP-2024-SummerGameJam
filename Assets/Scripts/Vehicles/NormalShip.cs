using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShip : Ship
{
    override protected void Start()
    {
        base.Start();
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
