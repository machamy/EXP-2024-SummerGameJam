using UnityEngine;

namespace DefaultNamespace.Vehicles
{
    public class NormalCar:Car
    {
        public override void OnCollisionUp()
        {
            hp.Value -= 1;
            SoundManager.Instance.StopSFX("car_slow");
            SoundManager.Instance.Play("car_crash", SoundManager.SoundType.SFX);
            OnDeath();
        }

        public override void OnArrival()
        {
            score.Value += 1;
            Destroy(gameObject);
        }
    }
}