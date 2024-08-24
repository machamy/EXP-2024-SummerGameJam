using UnityEngine;

namespace Vehicles.Ship
{
    public class Submarine : global::Ship
    {
        public float submarineCollisionHeight = 0.3f;

        public override bool isCollideHeight(float height)
        {
            return submarineCollisionHeight > height;
        }
        public override void OnCollisionFront()
        {
            Debug.Log("Submarine Front Collided");
            hp.Value -= 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            //위에서 아래로 찌부되는 경우
            OnDeath();
            Debug.Log("Submarine Middle Collided");
        }
    }
}
