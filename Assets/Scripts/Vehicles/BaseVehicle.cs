using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseVehicle : MonoBehaviour
{
    public enum VehicleType
    {
        Car,
        Ship
    }
    
    
    public enum VehicleState
    {
        Idle,
        MoveBefore,
        Wait,
        MoveAfter
    }

    public GameObject deathEffect;

    public float timestampCheck;
    public BridgeController bridgeController;
    public VehicleType type;
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
    public float TotalBeforeTime => priorMoveDelay + priorWaitDelay;

    public float bridgeCrossingTime = 1f;
    public float afterMovingTime = 3f;
    
    [Header("충돌")]
    public float collisionHeight = 0.3f;
    [Header("움직임 설정값")]
    public Line MoveLine;
    public bool isReverse;
    public CurveSO curveSO;
    public CurveSO beforeCurveSo;
    [Header("움직임 현재값")] 
    public float currentTime;
    [FormerlySerializedAs("currentPosition")] public float currentDistance = 0f;
    public Line.Point currentPoint;
    public VehicleState state;
    [field: SerializeField] public bool IsOnBridge { get; private set; }
    private AnimationCurve Curve => curveSO.Curve;
    private AnimationCurve BeforeCurve => beforeCurveSo.Curve;
    public Transform OriginPos => isReverse ? MoveLine.End : MoveLine.Spawn;
    public Transform WaitPos => isReverse ? MoveLine.Wait02 : MoveLine.Wait01;
    public Transform EndPos => isReverse ? MoveLine.Spawn : MoveLine.End;
    public float OriginDistance => isReverse ? MoveLine.EndDistance :0;
    public float WaitDistance => isReverse ? MoveLine.Wait02Distance : MoveLine.Wait01Distance;
    
    public float BridgeEndDistance => isReverse ? MoveLine.Bridge01Distance : MoveLine.Bridge02Distance;
    public float EndDistance => isReverse ?  0 :MoveLine.EndDistance;
    
    public Coroutine MoveCoroutine;

    [Header("공용 변수")]
    public IntVariableSO score;
    public IntVariableSO hp;


    private void FixedUpdate()
    {
        currentPoint = CheckPoint();
        Move();
        CheckBridge();
        if(IsOnBridge)
            OnBridgeCrossing();
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
                currentTime = 0f;
                state = VehicleState.MoveBefore; // 움직임 시작
                currentTime += Time.fixedDeltaTime;
                return;// 움직이지 않는다.
            case VehicleState.MoveBefore:
                if (currentDistance >= WaitDistance) // 도착하면 움직이지않고 대기 페이즈로
                {
                    transform.position = WaitPos.position;
                    state = VehicleState.Wait;
                    currentTime = 0f;
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
        position = Vector3.Lerp(startPos,endPos, pickedCurve.Evaluate(ratio));
        currentDistance = Mathf.Lerp(start, end, ratio);
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
        Line.Point targetPoint = isReverse ? Line.Point.Bridge02 : Line.Point.Bridge01;

        if (currentPoint == targetPoint) // 현재 다리 위에 있음
        {
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
            IsOnBridge = false;
        }
    } 
    public Line.Point CheckPoint()
    {
        Line.Point res = Line.Point.Spawn;
        if (isReverse)
        {
            for (Line.Point i = Line.Point.Spawn; i <= Line.Point.End; i++)
            {
                if (MoveLine.distances[i] >= currentDistance)
                {
                    res = i;
                    break;
                }
            }
        }
        else
        {
            for (Line.Point i = Line.Point.End; i >= Line.Point.Spawn; i--)
            {
                if (MoveLine.distances[i] <= currentDistance)
                {
                    res = i;
                    break;
                }
            }
        }

        return res;
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

    public abstract bool isCollideHeight(float height);
    
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

    public abstract void OnBridgeCrossing();

    public abstract void OnArrival();
    

    public void OnDeath()
    {
        var effect = Instantiate(deathEffect);
        effect.transform.position = transform.position;
        Destroy(gameObject);
    }
}
