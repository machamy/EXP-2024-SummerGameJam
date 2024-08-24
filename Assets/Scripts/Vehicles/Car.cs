using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Vehicles;
using UnityEngine;

namespace Vehicles
{
    public abstract class Car : BaseVehicle
    {

        [Header("Car")]
        public AudioSource audioSource;

        public GameObject cargfx;

        [SerializeField] private TrafficLight trafficLight;
    
        private float startTime;
        private float distanceLength;

        // IEnumerator Moving()
        // {
        //     yield return null;
        // }
    

        protected virtual IEnumerator StopRoutine()
        {

            /*if (collision.gameObject.CompareTag("Player"))
        {
          
        }*/

            SoundManager.Instance.StopSFX("car_slow");

            WaitForSeconds wait = new WaitForSeconds(priorWaitDelay / 3f);

            trafficLight.SetLevel(1);
            // 신호등 띄우고 시간
            yield return wait;
            trafficLight.SetLevel(2);
            yield return wait;
            trafficLight.SetLevel(3);
            yield return wait;
            trafficLight.SetLevel(0);

            this.state = VehicleState.MoveAfter;
            currentTime = 0f;
            SoundManager.Instance.Play("car_slow", SoundManager.SoundType.SFX);
        }

        protected virtual void Start()
        {
            SoundManager.Instance.Play("car_slow", SoundManager.SoundType.SFX);
  
            //renderer = GetComponent<SpriteRenderer>();
            trafficLight.SetLevel(0);

        }

        public override bool isCollideHeight(float height)
        {
            return collisionHeight > height;
        }


        public override void OnBridgeCrossing()
        {
            var position = transform.position;
            cargfx.transform.position = new Vector3(position.x, position.y + (bridgeController.height - 1.0f), position.z);
        }

        public override void OnWait()
        {
            StartCoroutine(StopRoutine());
        }

        /// <summary>
        /// 차의 전방충돌은 Up과 같다.
        /// </summary>
        public override void OnCollisionFront()
        { 
            OnCollisionUp();
        }

        public override void OnArrival()
        {
            Destroy(gameObject);
        }
    }
}
