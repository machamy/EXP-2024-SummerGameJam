using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

namespace Vehicles
{
    public abstract class BaseVehicle : MonoBehaviour
    {
        public enum VehicleType
        {
            Car,
            Ship,
            Pedstrian
        }
    
    
        public enum VehicleState
        {
            Idle,
            MoveBefore,
            Wait,
            MoveAfter,
            DeathRoutine // 깜빡임중 움직임 방지
        }

        [Header("Debug")] public bool showPanjeong;
        [Space, Header("BaseVehicle")] public VehicleDataSO VehicleData;
        public GameObject deathEffect;

        public float height = 1f;
        public float timestampCheck;
        public BridgeController bridgeController;
        public VehicleType type;
        public GameObject gfx;

        public virtual bool isBridgeMustSink => false;
        /// <summary>
        /// 다리 이전 이동 시간
        /// </summary>
        [Tooltip("다리 도착전 이동시간")]public float priorMoveDelay = 2;
        /// <summary>
        /// 다리 이전 대기시간(자동차만 해당)
        /// </summary>
        [Tooltip("대기시간(신호등)")] public float priorWaitDelay = 3;
    
        /// <summary>
        /// 다리 오르기 전까지의 시간
        /// </summary>
        public float TotalBeforeTime => type == VehicleType.Ship ? 0 : priorMoveDelay + priorWaitDelay;

        public float bridgeCrossingTime = 1f;
        public float bridgeStartTime = 0.2f;
        public float bridgeEndTime = 1.2f;
        public float afterMovingTime = 3f;

        public float waitTime = 0.4f;
    
        [Header("판정")]
        [Tooltip("다리 충돌")]public float collisionHeight = 0.3f;

        [Tooltip("앞쪽 판정선(양수)")] public float frontDeltaPos = 0f;
        [Tooltip("뒤쪽 판정선(음수)")] public float backwardDeltaPos = 0f;

        [Tooltip("사망여부")] public bool isDead = false;
        [Header("움직임 설정값")]
        public Line MoveLine;
        public bool isReverse;
        public CurveSO curveSO;
        public CurveSO beforeCurveSo;
        [Header("움직임 현재값")] 
        public float currentTime;
        [FormerlySerializedAs("currentPosition")] public float currentDistance = 0f;
        public Line.Point frontPoint;
        public Line.Point backwardPoint;
        public Line.Point midPoint;
        public VehicleState state;
        [field: SerializeField] public bool IsOnBridge { get; private set; }

        #region Properties
        protected AnimationCurve Curve => curveSO.Curve;
        protected AnimationCurve BeforeCurve => beforeCurveSo.Curve;
        public Transform OriginPos => MoveLine.Spawn;
        public Transform WaitPos => MoveLine.Wait01;
        public Transform EndPos => MoveLine.End;
        public float OriginDistance => 0;
        public float WaitDistance => MoveLine.Wait01Distance;
        public float BridgeStartDistance => MoveLine.Bridge01Distance;
        public float BridgeEndDistance => MoveLine.Bridge02Distance;
        public float EndDistance => MoveLine.EndDistance;

        #endregion
        
    
        public Coroutine MoveCoroutine;

        [Header("공용 변수")]
        [FormerlySerializedAs("score")]public IntVariableSO playerScore;
        [FormerlySerializedAs("hp")] public IntVariableSO playerHp;


        private void FixedUpdate()
        {
            frontPoint = MoveLine.CheckPoint(currentDistance + frontDeltaPos);
            midPoint = MoveLine.CheckPoint(currentDistance);
            backwardPoint = MoveLine.CheckPoint(currentDistance + backwardDeltaPos);
            Move();
            CheckBridge();
            if(IsOnBridge)
                // if(!isDead)
                OnBridgeCrossing();
        }

        private void OnEnable()
        {
            currentTime = 0f;
            AutoInit();
        }

        public virtual void AutoInit() => AutoInit(-0.3f, 0.5f);
        
        public void AutoInit(float deltaA, float deltaB)
        {
            StartCoroutine(BridgeAutoInputRoutine(TotalBeforeTime + bridgeStartTime + deltaA, waitTime+deltaB));
        }


        protected virtual void Move()
        {
            Vector3 position, startPos,endPos;
            float ratio, totalTime,start,end;
            AnimationCurve pickedCurve;
            switch (state)
            {
                case VehicleState.Idle:
                    // OnStart와 같음
                    OnIdleStay(); // 초기 시작 함수
                    currentTime += Time.fixedDeltaTime;
                    return;// 움직이지 않는다.
                case VehicleState.MoveBefore:
                    if (currentDistance >= WaitDistance) // 도착하면 움직이지않고 대기 페이즈로
                    {
                        transform.position = WaitPos.position;
                        state = VehicleState.Wait;
                        OnWait();
                        return;
                    }
                    // OnMovingBefore 와 같음
                    totalTime = priorMoveDelay;
                    startPos = OriginPos.position;
                    endPos = WaitPos.position;
                    start = OriginDistance;
                    end = WaitDistance;
                    pickedCurve = BeforeCurve;
                    break;
                case VehicleState.Wait:
                    currentTime += Time.fixedDeltaTime;
                    return;// 아무것도 하지 않는다.
                case VehicleState.MoveAfter:
                    totalTime = afterMovingTime;
                    startPos = WaitPos.position;
                    endPos = EndPos.position;
                    start = WaitDistance;
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

        /// <summary>
        /// 다리위에 있는지확인하는 함수
        /// </summary>
        protected virtual void CheckBridge()
        {
            Line.Point targetPoint = Line.Point.Bridge01;
            if (isDead) // 죽으면 실행 X
                return;
            //backwardPoint: 
            if (frontPoint == targetPoint || backwardPoint == targetPoint) // 현재 다리 위에 있음
            {
                if (!IsOnBridge)
                {
                    OnBridgeStart();
                }
                if (isCollideHeight(bridgeController.height)) // 충돌시
                {
                    // 이전위치와 현재위치 확인
                    if (IsOnBridge) 
                    { 
                        //Stay 이벤트
                        OnCollisionUp();
                    }
                    else
                    { 
                        //Enter 이벤트
                        OnCollisionFront();
                    }
                }
                IsOnBridge = true;
            }
            else
            {
                if (IsOnBridge)
                {
                    OnBridgeEndDebug();
                    OnBridgeEnd();
                }
                IsOnBridge = false;
            }
        }

        public virtual float InitBridgeCrossingTime()
        {
            float totalDistance = (EndDistance - WaitDistance);
            // t0 = 대기--다리시작 / 대기--끝
            float t0 = (BridgeStartDistance - WaitDistance) /totalDistance;
            // t1 = 대기--다리끝 / 대기--끝
            float t1 = (BridgeEndDistance - WaitDistance) /totalDistance;
            // print($"({vehicle.BridgeEndDistance} - {vehicle.WaitDistance}) / {vehicle.EndDistance} - {vehicle.WaitDistance}");
            // print($"length = {Math.Abs(vehicle.EndDistance - vehicle.WaitDistance)}");
            bridgeStartTime = curveSO.EvaluateByValueFirst(t0) * afterMovingTime;
            bridgeEndTime = curveSO.EvaluateByValueFirst(t1) * afterMovingTime;
            bridgeCrossingTime = bridgeEndTime - bridgeStartTime;
            return bridgeCrossingTime;
        }


        public IEnumerator MoveRoutine(Vector3 start, Vector3 end, float time, Action callback)
        {
            float currentTime = 0;
            while (currentTime <= time)
            {
                Vector3 position = Vector3.Lerp(start, end, currentTime / time);
                transform.position = position;
                // print(position);
                yield return new WaitForFixedUpdate();
                currentTime += Time.fixedDeltaTime;
            }
            transform.position = end;
            callback?.Invoke();
        }
        
        private void OnDrawGizmosSelected()
        {
            Vector3 front, back;
            Vector3 position = transform.position;
            Vector3 direction;
            if(MoveLine is null)
            {
                direction = type == VehicleType.Car ? Utilties.carDirection : Utilties.shipDirection;
                if (isReverse)
                    direction *= -1;
            }
            else
            {
                direction = MoveLine.transform.right;
            }
            front = new Vector3(position.x, position.y, position.x) + direction * frontDeltaPos;
            back = new Vector3(position.x, position.y, position.x) + direction * backwardDeltaPos;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(front,back);


            float radius = 0.1f;
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(front,radius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(back,radius);
        }

        public abstract bool isCollideHeight(float height);

        public virtual void OnIdleStay()
        {
            currentTime = 0f;
            state = VehicleState.MoveBefore; // 움직임 시작
        }
    
        /// <summary>
        /// 대기 시작
        /// </summary>
        public abstract void OnWait();

        /// <summary>
        /// 정면 충돌
        /// </summary>
        public abstract void OnCollisionFront();
        /// <summary>
        /// 상향 충돌
        /// </summary>
        public abstract void OnCollisionUp();

        public virtual void OnBridgeStart()
        {
            // float a = TotalBeforeTime + timestampCheck;
            // print($"{name}{GetInstanceID()%10} Start {a+bridgeStartTime} ~ {a+bridgeEndTime} : {a+currentTime}({GameManager.Instance.LM.timestamp})");
            
        }

        private IEnumerator BridgeAutoInputRoutine(float wait, float time)
        {
            yield return new WaitForSeconds(wait);
            bridgeController.autoInput = isBridgeMustSink;
            yield return new WaitForSeconds(time);
            bridgeController.autoInput = false;
        }
 
        public virtual void OnBridgeEndDebug()
        {
            // float a = TotalBeforeTime + timestampCheck;
            // print($"{name}{GetInstanceID()%10} End {a+bridgeStartTime} ~ {a+bridgeEndTime} : {a+currentTime}({GameManager.Instance.LM.timestamp})");
        }
        public abstract void OnBridgeCrossing();
        public abstract void OnBridgeEnd();

        public abstract void OnArrival();
    

        public void OnDeath()
        {
            var effect = Instantiate(deathEffect);
            effect.transform.position = gfx ? gfx.transform.position : transform.position;
            Color c0, c1;
            SpriteRenderer renderer = gfx.GetComponent<SpriteRenderer>();
            c0 = c1 = renderer.color;
            c0.a = 0.5f;
            StartCoroutine(DeatRoutine(1, 0.25f, c0, c1));
        }

        private IEnumerator DeatRoutine(float total, float interval, Color c0, Color c1)
        {
            float remain = total;
            state = VehicleState.DeathRoutine;
            var wait = new WaitForSeconds(interval);
            SpriteRenderer renderer = gfx.GetComponent<SpriteRenderer>();
            bool isFirst = true;
            while (remain > 0)
            {
                remain -= interval;
                renderer.color = isFirst ? c0 : c1;
                yield return wait;
                isFirst = !isFirst;
            }
            Destroy(gameObject);
        }
    }
}
