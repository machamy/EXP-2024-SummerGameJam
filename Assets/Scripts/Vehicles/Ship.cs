using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : BaseVehicle
{
    public GameObject ShipPrefab; // ship 프리팹
    public Vector3 spawnPosition; // Ship 생성 위치
    public Vector3 Direction; // ship 진행방향
    public float speed = 5.0f; // 선박 속도
    public float shipCollisionHeight = 0.3f;
    float startTime;
    public bool Iscollision = false; //충돌여부

    void Start()
    {
        startTime = Time.time;
        Direction = Global.shipDirection;
    }

    private void FixedUpdate()
    {
        transform.position += Direction * (speed * Time.deltaTime);
        Debug.Log(Direction);
    }

    private void OnBecameInvisible()
    {
        Destroy(this);
    }

    /// <summary>
    /// 밑에서 충돌
    /// </summary>
    public void OnCollideUp()
    {
        Iscollision = true;
    }
    /// <summary>
    /// 앞에서 충돌
    /// </summary>
    public void OnCollideFront()
    {
        Iscollision = true;
    }
}
