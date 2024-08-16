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
    }


    public override void OnCollideUp()
    {
        Iscollision = false;
    }
}
