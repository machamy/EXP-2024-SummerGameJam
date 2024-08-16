using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVehicle : MonoBehaviour
{
    public enum VehicleType
    {
        Car,
        Ship
    }

    public VehicleType type;
    
    /// <summary>
    /// 다리 이전 이동 시간
    /// </summary>
    public float priorMoveDelay = 1;
    /// <summary>
    /// 다리 이전 대기시간(자동차만 해당)
    /// </summary>
    public float priorWaitDelay = 1;
    /// <summary>
    /// 다리 건너는 시간
    /// </summary>
    public float crossBridgeDelay = 1;
    /// <summary>
    /// 다리 건넌 후 시간
    /// </summary>
    public float afterMoveTime = 1;
}
