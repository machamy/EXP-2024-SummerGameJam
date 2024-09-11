using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vehicles.Cars
{
    /// <summary>
    /// PriorMoveTime +  priorWaitDelay: 사이렌 시간
    /// </summary>
    public class Ambulance : Car
    {
        [Space, Header("Ambulance")] [SerializeField, Tooltip("사이렌 색 깜빡임 횟수")]
        private int sirenMaxCount = 4;
        
        [SerializeField] private GameObject RedLight;
        [SerializeField] private GameObject BlueLight;
        

        private bool sirenSoundPlayed = false;

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
                    float total = priorMoveDelay + priorWaitDelay;
                    float hardcoding = -0.571f;
                    if(!MoveLine.isFirstLine)
                    {
                        RedLight.transform.position += Vector3.left * (hardcoding);
                        BlueLight.transform.position += Vector3.left * (hardcoding);
                    }
                    StartCoroutine(SirenWaitRoutine(total));
                    StartCoroutine(SirenLightRoutine(total/ sirenMaxCount));

                    if (!sirenSoundPlayed)
                    {
                        SoundManager.Instance.Play("ambul_siren", SoundManager.SoundType.SFX);
                        sirenSoundPlayed = true;
                    }

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
            float curveRatio = pickedCurve.Evaluate(ratio);
            position = Vector3.Lerp(startPos,endPos, curveRatio);
            currentDistance = Mathf.Lerp(start, end, curveRatio);
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
            float t0 = (BridgeStartDistance - frontDeltaPos) /totalDistance;
            // t1 = 대기--다리끝 / 대기--끝
            float t1 = (BridgeEndDistance - backwardDeltaPos) /totalDistance;
            // print($"({vehicle.BridgeEndDistance} - {vehicle.WaitDistance}) / {vehicle.EndDistance} - {vehicle.WaitDistance}");
            // print($"length = {Math.Abs(vehicle.EndDistance - vehicle.WaitDistance)}");
            bridgeStartTime = curveSO.EvaluateByValueFirst(t0) * afterMovingTime;
            bridgeEndTime = curveSO.EvaluateByValueFirst(t1) * afterMovingTime;
            bridgeCrossingTime = bridgeEndTime - bridgeStartTime;
            
            return bridgeCrossingTime;
        }
        
        public override void OnWait()
        {
            state = VehicleState.MoveAfter;
            currentTime = 0f;
        }

        public override void OnCollisionUp()
        {
            if(isDead)
                return;
            isDead = true;
            playerHp.Value -= 1;
            OnDeath();
        }

        public override void OnArrival()
        {
            if(isDead)
                return;
            playerScore.Value += 1;
            Destroy(gameObject);
        }
    }
}