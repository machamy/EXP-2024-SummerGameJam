using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.DataStructure;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float weight = 1.00f;
    [SerializeField] private float weightCoefficient = 0.95f;
    [SerializeField] private IntVariableSO score;
    [SerializeField] private float deltaTime = 10f;

    [SerializeField] private BridgeController bridge;

    [Header("Lines")] 
    [SerializeField] private Line carUpper;
    [SerializeField] private Line carLower;
    [SerializeField] private Line carUpperRev;
    [SerializeField] private Line carLowerRev;
    [SerializeField] private Line shipLeft;
    [SerializeField] private Line shipRight;
    [Header("Curves")]
    [SerializeField] private List<Pair<string,CurveSO>> curveDict;

    public CurveSO GetCurve(string name)
    {
        foreach (var p in curveDict)
        {
            if (p.first.ToLower() == name.ToLower())
                return p.second;
        }

        return curveDict[0].second;
    }
    [Header("Prefabs")]
    [SerializeField] private GameObject[] cars;
    [SerializeField] private GameObject[] ships;
    [Header("Particles")]
    [SerializeField] private GameObject explodeEffect;
    [SerializeField] private GameObject bubbleEffect;
    
    [Header("Summon Interval")]
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
        foreach (var go in schedule.Values)
        {
            Destroy(go);
        }
        schedule.Clear();
        canSpawn = true;
        foreach (var bv in FindObjectsByType<BaseVehicle>(FindObjectsInactive.Include,FindObjectsSortMode.None))
        {
            Destroy(bv.gameObject);
        }
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
                StartCoroutine(WaitSpawnRoutine(vehicle.bridgeCrossingTime));
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
        if (n == 0)
        {
            weight = 1.00f;
        }
        else if (n % 10 == 0)
        {
            weight *= weightCoefficient;
        }
    }

    public GameObject SummonRandom()
    {
        GameObject go;
        Transform spawnPoint, waitPoint,nextwaitPoint, endPoint;
        bool isLeft = Random.Range(0, 2) == 0;
        bool isCar = Random.Range(0, 100) < 50;
        Line PickedLine;
        bool isReverse = false;
        if (isCar)
        {
            isReverse = Random.Range(0, 2) == 1;
            isReverse = false;
            PickedLine = isLeft ? carUpper : carLower;
            go = Instantiate(cars[Random.Range(0, cars.Length)]);
        }
        else
        {
            PickedLine = isLeft ? shipLeft : shipRight;
            go = Instantiate(ships[Random.Range(0, ships.Length)]);
        }
        go.transform.position = isReverse ? PickedLine.End.position : PickedLine.Spawn.position;
        BaseVehicle vehicle = go.GetComponent<BaseVehicle>();
        InitVehicle(vehicle,PickedLine,"Linear",isReverse);
        return go;
    }

    private void InitVehicle(BaseVehicle vehicle, Line line, string curveSo, bool isReverse = false)
    {
        InitVehicle(vehicle, line, GetCurve(curveSo), isReverse);
    }
    

    private void InitVehicle(BaseVehicle vehicle,Line line,CurveSO curveSo,bool isReverse = false)
    {
        vehicle.MoveLine = line;
        vehicle.isReverse = isReverse;
        vehicle.beforeCurveSo = GetCurve("Linear");
        vehicle.curveSO = curveSo;
        vehicle.currentDistance = isReverse ? line.EndDistance : 0;
        vehicle.bridgeController = bridge;
        vehicle.playerHp = GameManager.Instance.hp;
        vehicle.playerScore = GameManager.Instance.score;
        vehicle.deathEffect = vehicle.type == BaseVehicle.VehicleType.Car ? bubbleEffect : explodeEffect;
        
        float t = (vehicle.BridgeEndDistance - vehicle.WaitDistance) / (vehicle.EndDistance - vehicle.WaitDistance);
        // print($"({vehicle.BridgeEndDistance} - {vehicle.WaitDistance}) / {vehicle.EndDistance} - {vehicle.WaitDistance}");
        // print($"length = {Math.Abs(vehicle.EndDistance - vehicle.WaitDistance)}");
        vehicle.bridgeCrossingTime = curveSo.EvaluateByValueFirst(t);
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
