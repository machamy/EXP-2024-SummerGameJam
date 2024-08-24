using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ambulance : Car
{
    public override void OnWait()
    {
        state = VehicleState.MoveAfter;
        currentTime = 0f;
    }

    public override void OnCollisionUp()
    {
        hp.Value -= 1;
        OnDeath();
    }
}
    