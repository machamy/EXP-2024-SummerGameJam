using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class NormalShip : global::Vehicles.Ship
    {
        private void Start()
        {
            
        }

        public override void OnCollisionFront()
        {
            if(isDead)
                return;
            isDead = true;
            Debug.Log("NormalShip Front Collided");
            playerHp.Value -= 1;
            OnDeath();
        }
        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;
            StartCoroutine(FlyAwayRoutine(callback: () => playerHp.Value -= 1));
        }

        public override void OnArrival()
        {
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}
