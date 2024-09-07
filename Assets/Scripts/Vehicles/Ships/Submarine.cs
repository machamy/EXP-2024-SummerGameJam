using DefaultNamespace;
using System;
using UnityEngine;

namespace Vehicles.Ships
{
    public class Submarine : global::Vehicles.Ship
    {
        public float submarineCollisionHeight = 0.3f;

        private void Start()
        {
            base.Start();
            SoundManager.Instance.Play("splash_long");
        }

        public override bool isCollideHeight(float height)
        {
            return submarineCollisionHeight > height;
        }
        public override void OnCollisionFront()
        {
            if(isDead)
                return;
            isDead = true;
            SoundManager.Instance.StopSFX("splash_long");
            SoundManager.Instance.Play("ship_crash");
            Debug.Log("Submarine Front Collided");
            playerHp.Value -= 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;
            //위에서 아래로 찌부되는 경우
            SoundManager.Instance.StopSFX("splash_long");
            SoundManager.Instance.Play("ship_crash");
            StartCoroutine(FlyAwayRoutine(callback: () => playerHp.Value -= 1));
        }

        public override void OnArrival()
        {
            if(isDead)
                return;
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}
