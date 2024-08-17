using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float weight = 1.00f;
    [SerializeField] private float weightCoefficient = 0.95f;
    [SerializeField] private IntVariableSO score;
    [SerializeField] private float deltaTime = 10f;

    [SerializeField] private BridgeController bridge;

    [SerializeField] private Transform carSpawnLeft;
    [SerializeField] private Transform carSpawnRight;
    [SerializeField] private Transform carWaitLeft;
    [SerializeField] private Transform carWaitRight;
    
    [SerializeField] private Transform shipSpawnLeft;
    [SerializeField] private Transform shipSpawnRight;
    [SerializeField] private Transform shipWaitLeft;
    [SerializeField] private Transform shipWaitRight;
    [SerializeField] private Transform shipNextWaitLeft;
    [SerializeField] private Transform shipNextWaitRight;
    [SerializeField] private Transform shipEndLeft;
    [SerializeField] private Transform shipEndRight;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private GameObject[] ships;


    [SerializeField] private float intervalMax = 7;
    [SerializeField] private float intervalMin = 3;
    [SerializeField] private float intervalRemain;

    // private class Comparer : IComparer<Tuple<float, GameObject>>
    // {
    //     public int Compare(Tuple<float, GameObject> x, Tuple<float, GameObject> y)
    //     {
    //         return (x.Item1 < y.Item1) ? -1 : 1;
    //     }
    // }
    private SortedList<float, GameObject> schedule = new SortedList<float, GameObject>();

    [SerializeField]private bool canSpawn = false;
    
    public float timestamp = 0.0f;
    public void Initialize()
    {
        weight = 100f;
        timestamp = 0.0f;
        intervalRemain = Random.Range(intervalMin, intervalMax);
    }
    

    public void Update()
    {
        if(GameManager.Instance.State != GameManager.GameState.Running)
            return;

        timestamp += Time.deltaTime;

        if(schedule.Count > 0)
        {
            var key = schedule.Keys[0];
            while (timestamp > key)
            {
                GameObject go = schedule.Values[0];
                schedule.RemoveAt(0);
                go.SetActive(true);
                if (schedule.Count <= 0)
                    return;
                key = schedule.Keys[0];
            }
        }

        if (canSpawn)
        {
            intervalRemain -= Time.deltaTime;
            if (intervalRemain <= 0)
            {
                // 소환 후 대기
                GameObject go = SummonRandom();
                go.SetActive(false);
                canSpawn = false;
                BaseVehicle vehicle = go.GetComponent<BaseVehicle>();
                schedule.Add( timestamp + deltaTime - vehicle.TotalBeforeTime, go);
                intervalRemain = Random.Range(intervalMin, intervalMax);
                vehicle.timestampCheck = timestamp + deltaTime - vehicle.TotalBeforeTime;
                StartCoroutine(WaitSpawnRoutine(vehicle.crossBridgeDelay));
            }
        }
    }

    private IEnumerator WaitSpawnRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        canSpawn = true;
    }
    
    private void CheckScore(int n)
    {
        if (n % 10 == 0)
        {
            weight *= weightCoefficient;
        }
    }

    public GameObject SummonRandom()
    {
        GameObject go;
        Transform spawnPoint, waitPoint,nextwaitPoint, endPoint;
        bool isLeft = Random.Range(0, 1) == 1;
        if (Random.Range(0, 100f) <= 50) // 차
        {
            isLeft = true;
            spawnPoint = carSpawnLeft;
            waitPoint = carWaitLeft;
            nextwaitPoint = carWaitRight;
            endPoint = carSpawnRight;
            go = Instantiate(cars[0]);
        }
        else //배
        {
            spawnPoint = isLeft ? shipSpawnLeft : shipSpawnRight;
            waitPoint = isLeft ? shipWaitLeft : shipWaitRight;
            nextwaitPoint = isLeft ? shipNextWaitLeft : shipNextWaitRight;
            endPoint = isLeft ? shipEndLeft : shipEndRight;
            go = Instantiate(ships[Random.Range(0,2)]);
        }
        go.transform.position = spawnPoint.position;
        BaseVehicle vehicle = go.GetComponent<BaseVehicle>();
        InitVehicle(vehicle,spawnPoint,waitPoint,nextwaitPoint,endPoint);
        return go;
    }

    private void InitVehicle(BaseVehicle vehicle,Transform spawnPoint, Transform waitPoint,Transform nextwaitPoint, Transform endPoint)
    {
        vehicle.OriginPos = spawnPoint;
        vehicle.WaitPos = waitPoint;
        vehicle.NextWaitPos = nextwaitPoint;
        vehicle.EndPos = endPoint;
        vehicle.bridgeController = bridge;
        vehicle.hp = GameManager.Instance.hp;
        vehicle.score = GameManager.Instance.score;
    }

    private void OnEnable()
    {
        score.ValueChangeEvent.AddListener(CheckScore);
    }

    private void OnDisable()
    {
        score.ValueChangeEvent.RemoveListener(CheckScore);
    }
}
