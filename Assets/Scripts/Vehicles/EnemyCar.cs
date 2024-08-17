namespace DefaultNamespace.Vehicles
{
    public class EnemyCar : Car
    {
        public override void OnCollideDown()
        {
            Isflooding = true;
            hp.Value -= 1;
            OnDeath();
        }
    }
}