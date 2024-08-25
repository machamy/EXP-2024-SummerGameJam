using System.Collections;
using UnityEngine;

namespace Vehicles.Cars
{
    /// <summary>
    /// PriorMoveTime +  priorWaitDelay: 사이렌 시간
    /// </summary>
    public class Ambulance : Car
    {
        [Space, Header("Ambulance")] 
        [SerializeField, Tooltip("사이렌 색 바뀌는 주기")] private float sirenInterval = 0.5f;
        
        [SerializeField] private GameObject RedLight;
        [SerializeField] private GameObject BlueLight;
        
        protected override void Move()
        {
            Vector3 position, startPos,endPos;
            float ratio, totalTime,start,end;
            AnimationCurve pickedCurve;
            switch (state)
            {
                case VehicleState.Idle:
                    // OnStart와 같음
                    currentTime = 0f;
                    state = VehicleState.MoveBefore;
                    StartCoroutine(SirenWaitRoutine(priorMoveDelay+ priorWaitDelay));
                    StartCoroutine(SirenLightRoutine(sirenInterval));
                    currentTime += Time.fixedDeltaTime;
                    return;// 움직이지 않는다.
                case VehicleState.MoveBefore:
                    return;// 사이렌 주기.
                case VehicleState.Wait:
                    return;// 아무것도 하지 않는다.
                case VehicleState.MoveAfter:
                    totalTime = afterMovingTime;
                    startPos = OriginPos.position;
                    endPos = EndPos.position;
                    start = OriginDistance;
                    end = EndDistance;
                    pickedCurve = Curve;
                    if (currentDistance >= EndDistance)
                        OnArrival();
                    break;
                default:
                    return;
            }

            if (totalTime == 0f)
                ratio = 1.0f;
            else
                ratio = currentTime / totalTime;
            position = Vector3.Lerp(startPos,endPos, pickedCurve.Evaluate(ratio));
            currentDistance = Mathf.Lerp(start, end, ratio);
            // print($"{name} : {totalTime} {startPos} {endPos}, {pickedCurve.Evaluate(ratio)}");
            // print($"{currentTime} {ratio} {currentDistance} {position}");
            currentTime += Time.fixedDeltaTime;
            transform.position = position;
        }

        private IEnumerator SirenWaitRoutine(float time)
        {
            yield return new WaitForSeconds(time);
            state = VehicleState.MoveAfter;
            currentTime = 0f;
        }

        private IEnumerator SirenLightRoutine(float time)
        {
            WaitForSeconds wait = new WaitForSeconds(time);
            bool isRed = true;
            while (state == VehicleState.MoveBefore)
            {
                RedLight.SetActive(isRed);
                BlueLight.SetActive(!isRed);
                isRed = !isRed;
                yield return wait;
            }
            RedLight.SetActive(false);
            BlueLight.SetActive(false);
        }

        
        public override float InitBridgeCrossingTime()
        {
            float totalDistance = (EndDistance);
            
            float t0 = BridgeStartDistance /totalDistance;
            float t1 = BridgeEndDistance /totalDistance;
            
            bridgeCrossingTime = (curveSO.EvaluateByValueFirst(t1) - curveSO.EvaluateByValueFirst(t0)) * afterMovingTime;
            return bridgeCrossingTime;
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