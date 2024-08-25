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
            if(isDead)
                return;
            Debug.Log("Pirate Front Collided");
            isDead = true;
            playerScore.Value += 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;
            StartCoroutine(FlyAwayRoutine(callback: () => playerScore.Value += 1));
        }

        public override void OnArrival()
        {
            if(isDead)
                return;
            playerHp.Value -= 1;
            Destroy(gameObject);
        }
    }
}
