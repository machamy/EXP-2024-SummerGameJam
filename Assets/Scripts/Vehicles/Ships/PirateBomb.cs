using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class PirateBomb : Ship
    {
        private void Start()
        {
            
        }

        public override void OnCollisionFront()
        {
            playerHp.Value -= 1;
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
