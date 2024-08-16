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
    }

    private void OnBecameInvisible()
    {
        Destroy(this);
    }

    /// <summary>
    /// 앞에서 충돌
    /// </summary>
    public void OnCollideFront()
    {
        Iscollision = true;
        Debug.Log("전면충돌");
    }
    /// <summary>
    /// 밑에서 충돌
    /// </summary>
    public void OnCollideUp()
    {
        Iscollision = true;
        Debug.Log("하부충돌");
    }
}
