using DefaultNamespace;
using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class PirateBomb : Ship
    {
        public GameObject FailEffect;

        public override bool isBridgeMustSink => true;
        private void Start()
        {
            base.Start();
            SoundManager.Instance.Play("speedboat");
        }

        public override void AutoInit() => AutoInit(-0.3f, 0);
        public override void OnCollisionFront()
        {
            playerHp.Value -= 1;
            isDead = true;

            deathEffect = FailEffect;
            SoundManager.Instance.StopSFX("speedboat");
            SoundManager.Instance.Play("ship_crash");
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
            var go = Instantiate(FailEffect);
            SoundManager.Instance.Play("ship_crash");
            go.transform.position = MoveLine.EndEffect.position;
            playerHp.Value -= 1;
            Destroy(gameObject);
        }
    }
}
