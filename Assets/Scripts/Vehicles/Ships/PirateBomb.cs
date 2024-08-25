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
            Debug.Log("Pirate Middle Collided");
            playerScore.Value += 1;
            OnDeath(); //TODO ���ư���
        }

        public override void OnArrival()
        {
            playerHp.Value -= 1;
            Destroy(gameObject);
        }
    }
}
