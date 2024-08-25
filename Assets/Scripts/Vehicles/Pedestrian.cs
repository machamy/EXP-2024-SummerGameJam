using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Vehicles;
using UnityEngine;

namespace Vehicles
{
    public class Pedestrian : BaseVehicle
    {

        [Header("Pedestrian")]
        public AudioSource audioSource;

        //public GameObject Pedgfx;

        private float startTime;
        private float distanceLength;

        // IEnumerator Moving()
        // {
        //     yield return null;
        // }


        protected virtual IEnumerator StopRoutine()
        {

            //SoundManager.Instance.StopSFX("ped_walk");

            WaitForSeconds wait = new WaitForSeconds(priorWaitDelay / 3f);

            yield return wait;
            this.state = VehicleState.MoveAfter;
            currentTime = 0f;
            //SoundManager.Instance.Play("ped_walk", SoundManager.SoundType.SFX);
        }

        protected virtual void Start()
        {

        }

        public override bool isCollideHeight(float height)
        {
            return false;
        }


        public override void OnBridgeCrossing()
        {
            
        }

        public override void OnWait()
        {
            StartCoroutine(StopRoutine());
        }

        public override void OnCollisionFront()
        {
        }

        public override void OnCollisionUp()
        {
        }

        public override void OnArrival()
        {
            Destroy(gameObject);
        }

        
    }
}
