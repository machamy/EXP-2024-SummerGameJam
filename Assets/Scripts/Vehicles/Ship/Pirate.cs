using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : Ship
{
    public override void OnCollisionFront()
    {
        Iscollision = true;
        Debug.Log("Pirate Front Collided");
        score.Value += 1;
        OnDeath();
    }


    public override void OnCollisionUp()
    {
        Iscollision = true;
        Debug.Log("Pirate Middle Collided");
        score.Value += 1;
        OnDeath(); //TODO 날아가기
    }
}
