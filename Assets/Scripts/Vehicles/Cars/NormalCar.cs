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
            playerHp.Value -= 1;
            SoundManager.Instance.StopSFX("car_slow");
            SoundManager.Instance.Play("car_crash");
            OnDeath();
        }

        public override void OnArrival()
        {
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}