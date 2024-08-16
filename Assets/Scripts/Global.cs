using UnityEngine;

namespace DefaultNamespace
{
    public static class Global
    {
        public static Vector2 shipDirection { get; private set; } = new Vector2(Mathf.Cos(239/180 * Mathf.PI), Mathf.Sin(239/180 * Mathf.PI)).normalized; //239
        public static Vector2 carDirection { get; private set; } = new Vector2(Mathf.Cos(347/180 * Mathf.PI), Mathf.Sin(347/180 * Mathf.PI)).normalized; //347
    }
}