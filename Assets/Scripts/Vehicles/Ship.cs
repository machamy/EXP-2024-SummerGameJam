using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject ShipPrefab; // ship ������
    public Vector3 spawnPosition; // Ship ���� ��ġ
    public Vector3 endPosition; // Ship ���� ��ġ
    public float speed = 5.0f; // ���� �ӵ�
    public float shipCollisionHeight = 0.3f;
    float journeyLength;
    float startTime;
    bool Iscollision = false; //�浹����

    void Start()
    {
        journeyLength = Vector3.Distance(spawnPosition, endPosition);
        startTime = Time.time;
    }

   /* private IEnumerator SpawnShip()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ShipPrefab ����
            GameObject newShip = Instantiate(ShipPrefab, spawnPosition, Quaternion.identity);

            // ShipMove �ڷ�ƾ�� �����Ͽ� ���� �̵� �� �Ҹ� ó��
            StartCoroutine(ShipMove(newShip));
        }
    }*/

    private void FixedUpdate()
    {
        // �̵� ����� ���
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        // ������ ���������� endPosition���� �̵�
        transform.position = Vector3.Lerp(spawnPosition, endPosition, fractionOfJourney);

        // ��ǥ ������ �����ϸ� ���� ����
        if (transform.position == endPosition)
        {   
            Destroy(this);
        }
    }

    /// <summary>
    /// �ؿ��� �浹
    /// </summary>
    public void OnCollideUp()
    {
        Iscollision = true;
    }
    /// <summary>
    /// �տ��� �浹
    /// </summary>
    public void OnCollideFront()
    {
        Iscollision = true;
    }
}
