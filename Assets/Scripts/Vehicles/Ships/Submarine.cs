using UnityEngine;

namespace Vehicles.Ships
{
    public class Submarine : global::Vehicles.Ship
    {
        public float submarineCollisionHeight = 0.3f;

        public override bool isCollideHeight(float height)
        {
            return submarineCollisionHeight > height;
        }
        public override void OnCollisionFront()
        {
            Debug.Log("Submarine Front Collided");
            playerHp.Value -= 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            //위에서 아래로 찌부되는 경우
            playerHp.Value -= 1;
            OnDeath();
            Debug.Log("Submarine Middle Collided");
        }

        public override void OnArrival()
        {
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}
