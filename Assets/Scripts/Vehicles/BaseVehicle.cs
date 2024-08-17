using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BaseVehicle : MonoBehaviour
{
    public enum VehicleType
    {
        Car,
        Ship
    }
    
    public enum State
    {
        Idle,
        Before,
        Stop,
        Wait,
        After,
        AfterMoving,
        EndReady,
        EndMoving,
        End
    }

    public float timestampCheck;
    public BridgeController bridgeController;
    public VehicleType type;
    public State state;
    public bool isDead = false;
    /// <summary>
    /// 다리 이전 이동 시간
    /// </summary>
    public float priorMoveDelay = 2;
    /// <summary>
    /// 다리 이전 대기시간(자동차만 해당)
    /// </summary>
    public float priorWaitDelay = 3;

    public float TotalBeforeTime => priorMoveDelay + priorWaitDelay;
    /// <summary>
    /// 다리 건너는 시간
    /// </summary>
    public float crossBridgeDelay = 3;
    /// <summary>
    /// 다리 건넌 후 시간
    /// </summary>
     public float afterMoveTime = 1;


    public Transform OriginPos;
    public Transform WaitPos;
    public Transform NextWaitPos;
    public Transform EndPos;

    public Coroutine MoveCoroutine;

    public IntVariableSO score;
    public IntVariableSO hp;

    public void Awake()
    {
        state = State.Idle;
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        if (state == State.Idle)
        {
            state = State.Before;
            MoveCoroutine = StartCoroutine(MoveRoutine(OriginPos.position, WaitPos.position, priorMoveDelay, ()=> state = State.Stop));
        }
        else if (state == State.After)
        {
            state = State.AfterMoving;
            MoveCoroutine = StartCoroutine(MoveRoutine(WaitPos.position, NextWaitPos.position, crossBridgeDelay, ()=> state = State.EndReady));
        }
        else if (state == State.EndReady)
        {
            state = State.EndMoving;
            MoveCoroutine = StartCoroutine(MoveRoutine(NextWaitPos.position, EndPos.position, afterMoveTime, ()=> OnArrival()));
        }
        
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

    public virtual void OnArrival()
    {
        state = State.End;
        score.Value += 1;
        Destroy(gameObject);
    }
}
