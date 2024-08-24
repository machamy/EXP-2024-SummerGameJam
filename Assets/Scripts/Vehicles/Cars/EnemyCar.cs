namespace Vehicles.Cars
{
    public class EnemyCar : Car
    {
        public override void OnCollisionUp()
        {
            hp.Value -= 1;
            OnDeath();
        }

        public override void OnArrival()
        {
            score.Value -= 1;
            Destroy(gameObject);
        }
    }
}