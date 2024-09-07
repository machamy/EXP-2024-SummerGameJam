using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

namespace Vehicles.Cars
{
    public class NormalCar:Car
    {

        protected virtual IEnumerator PlayCarLoop()
        {
            for (int i = 0; i < 1; i++)
            {
                SoundManager.Instance.Play("car_loop2");

            }

            yield return new WaitForSeconds(4f);

            for (int i = 0; i < 1; i++)
            {
                SoundManager.Instance.Play("car_loop2");

            }
        }

        protected override void Start()
        {
            base.Start();
            SoundManager.Instance.Play("car_loop1");
            StartCoroutine(PlayCarLoop());
        }



        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;
            playerHp.Value -= 1;
            SoundManager.Instance.StopSFX("car_loop2");
            SoundManager.Instance.Play("car_crash");
            OnDeath();
        }

        public override void OnArrival()
        {
            SoundManager.Instance.StopSFX("car_loop2");
            SoundManager.Instance.Play("car_loop3");
            if (isDead)
                return;
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }

}