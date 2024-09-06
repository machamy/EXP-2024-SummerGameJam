using DefaultNamespace;
using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class NormalShip : global::Vehicles.Ship
    {
        private void Start()
        {
            SoundManager.Instance.Play("splash_long");
        }

        public override void OnCollisionFront()
        {
            if(isDead)
                return;
            isDead = true;
            SoundManager.Instance.StopSFX("splash_long");
            SoundManager.Instance.Play("ship_crash");
            // print($"{bridgeStartTime} {currentTime}");
            playerHp.Value -= 1;
            OnDeath();
        }
        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;

            SoundManager.Instance.StopSFX("splash_long");
            SoundManager.Instance.Play("bridge_jump_2");
            StartCoroutine(FlyAwayRoutine(callback: () => playerHp.Value -= 1));
        }

        public override void OnArrival()
        {
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}
