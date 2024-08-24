namespace Vehicles.Car
{
    public class EnemyCar : global::Car
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