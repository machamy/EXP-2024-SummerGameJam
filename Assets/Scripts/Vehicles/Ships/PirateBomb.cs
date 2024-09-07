using DefaultNamespace;
using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class PirateBomb : Ship
    {
        private void Start()
        {
            base.Start();
            SoundManager.Instance.Play("speedboat");
        }

        public override void OnCollisionFront()
        {
            playerHp.Value -= 1;
            isDead = true;
            SoundManager.Instance.StopSFX("speedboat");
            SoundManager.Instance.Play("sheep_crash");
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            isDead = true;

            SoundManager.Instance.StopSFX("speedboat");
            SoundManager.Instance.Play("bridge_jump_2");

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
