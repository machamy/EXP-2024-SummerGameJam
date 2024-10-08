using DefaultNamespace;
using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class Pirate : global::Vehicles.Ship
    {
        public GameObject FailEffect;

        private void Start()
        {
            base.Start();
            SoundManager.Instance.Play("speedboat");
        }

        public override void OnCollisionFront()
        {
            if(isDead)
                return;
            // Debug.Log("Pirate Front Collided");
            isDead = true;

            SoundManager.Instance.StopSFX("speedboat");
            SoundManager.Instance.Play("ship_crash");

            playerScore.Value += 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            if(isDead)
                return;
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
