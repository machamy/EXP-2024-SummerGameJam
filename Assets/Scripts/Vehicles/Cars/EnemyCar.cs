using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

namespace Vehicles.Cars
{
    public class EnemyCar : Car
    {
        public override bool isBridgeMustSink => true;

        public GameObject FailEffect;

        protected override void Start()
        {
            base.Start();
            SoundManager.Instance.Play("car_slow", SoundManager.SoundType.SFX);
        }

        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;
            playerScore.Value += 1;
            SoundManager.Instance.StopSFX("car_slow");
            SoundManager.Instance.Play("car_crash", SoundManager.SoundType.SFX);
            OnDeath();
        }

        public override void OnArrival()
        {
            playerHp.Value -= 1;
            var go = Instantiate(FailEffect);
            go.transform.position = MoveLine.Wait02.position;
            Destroy(gameObject);
        }
    }
}