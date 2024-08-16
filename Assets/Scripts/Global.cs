using UnityEngine;

namespace DefaultNamespace
{
    public static class Global
    {
        public static Vector2 shipDirection { get; private set; } = new Vector2(Mathf.Cos(239 * Mathf.Deg2Rad), Mathf.Sin(239* Mathf.Deg2Rad)).normalized; //239
        public static Vector2 carDirection { get; private set; } = new Vector2(Mathf.Cos(347* Mathf.Deg2Rad), Mathf.Sin(347* Mathf.Deg2Rad)).normalized; //347
    }
}