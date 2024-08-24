namespace Vehicles.Car
{
    public class Ambulance : global::Car
    {


        public override void OnIdleStay()
        {

        }

        public override void OnWait()
        {
            state = VehicleState.MoveAfter;
            currentTime = 0f;
        }

        public override void OnCollisionUp()
        {
            hp.Value -= 1;
            OnDeath();
        }

        public override void OnArrival()
        {
            score.Value += 1;
            Destroy(gameObject);
        }
    }
}