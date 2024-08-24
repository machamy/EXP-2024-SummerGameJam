using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShip : Ship
{
    public override void OnCollisionFront()
    {
        Iscollision = true;
        Debug.Log("NormalShip Front Collided");
        hp.Value -= 1;
        OnDeath();
    }
    public override void OnCollisionUp()
    {
        Iscollision = true;
        Debug.Log("NormalShip Middle Collided");
        hp.Value -= 1;
        OnDeath();
    }
}
