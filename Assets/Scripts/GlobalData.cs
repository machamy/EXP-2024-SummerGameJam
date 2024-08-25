using UnityEngine;

namespace DefaultNamespace
{
    public static class GlobalData
    {
        public const float shipDegree =240; // 240도가 맞음
        public const float carDegree = 346; // 346도가 맞음
        public static Vector2 shipDirection { get; private set; } = new Vector2(Mathf.Cos(shipDegree * Mathf.Deg2Rad), Mathf.Sin(239* Mathf.Deg2Rad)).normalized; //239
        public static Vector2 carDirection { get; private set; } = new Vector2(Mathf.Cos(carDegree* Mathf.Deg2Rad), Mathf.Sin(347* Mathf.Deg2Rad)).normalized; //347
    }
}