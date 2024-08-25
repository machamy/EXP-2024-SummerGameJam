using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class Pirate : global::Vehicles.Ship
    {
        private void Start()
        {
            
        }

        public override void OnCollisionFront()
        {
            Debug.Log("Pirate Front Collided");
            playerScore.Value += 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            StartCoroutine(FlyAwayRoutine(callback: () => playerScore.Value += 1));
        }

        public override void OnArrival()
        {
            playerHp.Value -= 1;
            Destroy(gameObject);
        }
    }
}
