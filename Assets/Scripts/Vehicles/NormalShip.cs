using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShip : Ship
{
    public override void OnCollideFront()
    {
        Iscollision = true;
        Debug.Log("NormalShip Front Collided");
        hp.Value -= 1;
        OnDeath();
    }


    public override void OnCollideUp()
    {
        Iscollision = true;
        Debug.Log("NormalShip Middle Collided");
        hp.Value -= 1;
        OnDeath();
    }
}
