using DefaultNamespace;
using System;
using UnityEngine;
using System.Collections;

namespace Vehicles.Ships
{
    public class NormalShip : global::Vehicles.Ship
    {
        public override bool isBridgeMustSink => true;
        
        protected virtual IEnumerator PlayShipLoop()
        {
            for (int i = 0; i < 5; i++)
            {
                SoundManager.Instance.Play("water");

                yield return new WaitForSeconds(1f); // 사운드 텀 여기서 조정 하면 됨 1f, 2f 이런식으로 (숫자만 바꾸면 됨)

            }
        }

        private void Start()
        {
            base.Start();
            StartCoroutine(PlayShipLoop());
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
