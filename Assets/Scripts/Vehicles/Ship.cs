using UnityEngine;

namespace Vehicles
{
    public abstract class Ship : BaseVehicle
    {
        public override bool isCollideHeight(float height)
        {
            return collisionHeight < height;
        }

        public override void OnBridgeCrossing()
        {
            // Do nothing
        }

        public override void OnWait()
        {
            state = VehicleState.MoveAfter;
            currentTime = 0f;
        }

        public override void OnCollisionFront()
        {
            Debug.Log("Front Collided");
        }

        public override void OnCollisionUp()
        {
            Debug.Log("Middle Collided");
        }

        public override void OnArrival()
        {
            Destroy(gameObject);
        }
    }
}

