namespace Vehicles.Cars
{
    public class Ambulance : Car
    {


        public override void OnIdleStay()
        {
            //TODO : 사이렌 후 BeforeMove상태로 바꾸기.
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