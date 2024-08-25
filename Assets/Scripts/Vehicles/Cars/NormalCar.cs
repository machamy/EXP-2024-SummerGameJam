using DefaultNamespace;

namespace Vehicles.Cars
{
    public class NormalCar:Car
    {
        public override void OnCollisionUp()
        {
            playerHp.Value -= 1;
            SoundManager.Instance.StopSFX("car_slow");
            SoundManager.Instance.Play("car_crash", SoundManager.SoundType.SFX);
            OnDeath();
        }

        public override void OnArrival()
        {
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}