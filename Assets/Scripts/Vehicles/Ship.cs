using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : BaseVehicle
{
    public GameObject ShipPrefab; // ship ������
    public Vector3 spawnPosition; // Ship ���� ��ġ
    public Vector3 Direction; // ship �������
    public float speed = 5.0f; // ���� �ӵ�
    public float shipCollisionHeight = 0.3f;
    float startTime;
    public bool Iscollision = false; //�浹����

    void Start()
    {
        startTime = Time.time;
        Direction = GlobalData.shipDirection;
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
    /// �տ��� �浹
    /// </summary>
    public void OnCollideFront()
    {
        Iscollision = true;
        Debug.Log("�����浹");
    }
    /// <summary>
    /// �ؿ��� �浹
    /// </summary>
    public void OnCollideUp()
    {
        Iscollision = true;
        Debug.Log("�Ϻ��浹");
    }
}
