namespace Vehicles.Cars
{
    public class Ambulance : Car
    {


        public override void OnIdleStay()
        {
            //TODO : 사이렌 후 BeforeMove상태로 바꾸기.
            //Start 쓴다면 이 함수는 override상태로 비워두기.
        }

        public override void OnWait()
        {
            state = VehicleState.MoveAfter;
            currentTime = 0f;
        }

        public override void OnCollisionUp()
        {
            playerHp.Value -= 1;
            
            OnDeath();
        }

        public override void OnArrival()
        {
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}