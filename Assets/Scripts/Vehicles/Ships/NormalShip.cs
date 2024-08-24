using UnityEngine;

namespace Vehicles.Ships
{
    public class NormalShip : global::Vehicles.Ship
    {
        public override void OnCollisionFront()
        {
            Debug.Log("NormalShip Front Collided");
            hp.Value -= 1;
            OnDeath();
        }
        public override void OnCollisionUp()
        {
            Debug.Log("NormalShip Middle Collided");
            hp.Value -= 1;
            OnDeath();
        }

        public override void OnArrival()
        {
            score.Value += 1;
            Destroy(gameObject);
        }
    }
}
