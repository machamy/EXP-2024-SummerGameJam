using DefaultNamespace;
using Unity.VisualScripting;

namespace Vehicles.Cars
{
    public class NormalCar:Car
    {
        protected override void Start()
        {
            base.Start();
            SoundManager.Instance.Play("car_slow");
        }

        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;
            playerHp.Value -= 1;
            SoundManager.Instance.StopSFX("car_slow");
            SoundManager.Instance.Play("car_crash");
            OnDeath();
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