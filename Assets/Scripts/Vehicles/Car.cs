using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Vehicles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Vehicles
{
    public abstract class Car : BaseVehicle
    {

        [Header("Car")]
        public AudioSource audioSource;
        
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
            // print($"car {TotalBeforeTime} {priorWaitDelay} , {currentTime}");
            currentTime = 0f;
            SoundManager.Instance.Play("car_slow", SoundManager.SoundType.SFX);
        }

        protected virtual void Start()
        {
            trafficLight.SetLevel(0);
            gfx.GetComponent<SpriteRenderer>().sprite = isReverse ? VehicleData.ReverseSprite : VehicleData.Sprite;
        }

        public override bool isCollideHeight(float height)
        {
            return collisionHeight >= this.height;
        }

        public void UpdateGfxPosition() => UpdateGfxPosition(height);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"> 0 ~ 1 : 최저점 ~ 최고점</param>
        public void UpdateGfxPosition(float height)
        {
            var position = transform.position;
            float targetPositionY = transform.position.y + (height - 1.0f) * bridgeController.heightweight;
            gfx.transform.position = new Vector3(position.x,targetPositionY , position.z);
        }

        public override void OnBridgeCrossing()
        {
            // 다리 높이에 맞춰서
            float G = isDead ? GameManager.WaterGravity : GameManager.Gravity;

            if (isDead)
            // 죽으면 쭉 떨어지기
            {
                height -= 1.0f * G * Time.fixedDeltaTime;
            }
            else if (height > bridgeController.height)
            // 떨어지는경우
            {
                height = Mathf.Max(height - 1.0f * G * Time.fixedDeltaTime, bridgeController.height);
            }
            else
            // 올라가는경우
            {
                height = bridgeController.height; 
            }
            
            UpdateGfxPosition();
            if(height <= 0.02f)
                Destroy(gameObject);
        }

        public override void OnBridgeEnd()
        {
            // 최고점으로
            height = 1;
            UpdateGfxPosition();
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
