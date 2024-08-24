using UnityEngine;

namespace Vehicles.Ships
{
    public class Submarine : global::Vehicles.Ship
    {
        public float submarineCollisionHeight = 0.3f;

        public override bool isCollideHeight(float height)
        {
            return submarineCollisionHeight > height;
        }
        public override void OnCollisionFront()
        {
            Debug.Log("Submarine Front Collided");
            hp.Value -= 1;
            OnDeath();
        }


        public override void OnCollisionUp()
        {
            //������ �Ʒ��� ��εǴ� ���
            OnDeath();
            Debug.Log("Submarine Middle Collided");
        }

        public override void OnArrival()
        {
            score.Value += 1;
            Destroy(gameObject);
        }
    }
}
