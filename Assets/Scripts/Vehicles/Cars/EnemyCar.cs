using DefaultNamespace;

namespace Vehicles.Cars
{
    public class EnemyCar : Car
    {
        public override void OnCollisionUp()
        {
            playerScore.Value += 1;
            SoundManager.Instance.StopSFX("car_slow");
            SoundManager.Instance.Play("car_crash", SoundManager.SoundType.SFX);
            OnDeath();
        }

        public override void OnArrival()
        {
            playerHp.Value -= 1;
            Destroy(gameObject);
        }
    }
}