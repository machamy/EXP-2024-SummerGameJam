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

   /* private IEnumerator SpawnShip()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ShipPrefab 생성
            GameObject newShip = Instantiate(ShipPrefab, spawnPosition, Quaternion.identity);

            // ShipMove 코루틴을 실행하여 선박 이동 및 소멸 처리
            StartCoroutine(ShipMove(newShip));
        }
    }*/

    private void FixedUpdate()
    {
        // 이동 진행률 계산
        float distCovered = (Time.time - startTime) * speed;
        //float fractionOfJourney = distCovered / journeyLength;

        // 선박을 점진적으로 endPosition으로 이동
        //transform.position = Vector3.Lerp(spawnPosition, endPosition, fractionOfJourney);

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
