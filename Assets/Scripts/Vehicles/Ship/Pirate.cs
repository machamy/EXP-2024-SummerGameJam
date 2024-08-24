using UnityEngine;

namespace Vehicles.Ship
{
    public class Pirate : global::Ship
    {
        public override void OnCollisionFront()
        {
            Debug.Log("Pirate Front Collided");
            score.Value += 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            Debug.Log("Pirate Middle Collided");
            score.Value += 1;
            OnDeath(); //TODO 날아가기
        }

        public override void OnArrival()
        {
            score.Value -= 1;
            Destroy(gameObject);
        }
    }
}
