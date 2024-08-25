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
            isDead = true;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
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
