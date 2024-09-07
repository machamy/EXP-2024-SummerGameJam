using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Database;
using DefaultNamespace.DataStructure;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool dbgSummonCar;
    [SerializeField] private int dbgCarId = 0;
    [SerializeField] private bool dbgSummonShip;
    [SerializeField] private int dbgShipId = 0;

    [Header("Level")]
    [Range(0,3),SerializeField] public int difficulty = 0;
    [SerializeField] private float[] intervalMinArr = { 3f, 1f, 0.2f };
    [SerializeField] private float[] intervalMaxArr = { 7f, 4f, 3f };
    [field:SerializeField] public int[] openScoreArr { get; private set; } = { 0, 70, 70, -1 };
    [SerializeField] private bool[] reverseValueArr = { false, false, true };
    [SerializeField] private bool[] useTArr = { false, true, true };

    [Header("Spawn")]    
    [SerializeField] private bool useReverse = false;
    [SerializeField] private bool useT = false;
    [Space] 
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
    [SerializeField] private Line pedestrianLine;
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
    [Space, Header("Prefabs")]
    [SerializeField] private GameObject[] cars;
    [SerializeField] private GameObject[] ships;
    [SerializeField] private GameObject[] pedestrianunit;
    [Header("CarData"), SerializeField] private List<VehicleDataSO> carDataList;
    [Header("ShipData"), SerializeField] private List<VehicleDataSO> shipDataList;
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

    [SerializeField] private bool canSpawn = false;
    [SerializeField] private bool PedcanSpawn = false;

    public float timestamp = 0.0f;
    public void Initialize()
    {
        weight = 1.00f;
        timestamp = 0.0f;
        intervalRemain = Random.Range(intervalMin, intervalMax);
        foreach (var go in schedule.Values)
        {
            Destroy(go);
        }
        schedule.Clear();
        canSpawn = true;
        PedcanSpawn = true;
        carDataList = Database.Instance.CarDataSoList;
        shipDataList = Database.Instance.ShipDataSoList;
        difficulty = GameManager.Instance.currentDifficulty;
        intervalMin = intervalMinArr[difficulty];
        intervalMax = intervalMaxArr[difficulty];
        useReverse = reverseValueArr[difficulty];
        useT = useTArr[difficulty];
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
                GameObject go = SummonRandomByData();
                go.SetActive(false);
                
                BaseVehicle vehicle = go.GetComponent<BaseVehicle>();
                float vehicleActivateTime = timestamp + deltaTime - (vehicle.TotalBeforeTime + vehicle.bridgeStartTime);
                schedule.Add(vehicleActivateTime, go);
                intervalRemain = Random.Range(intervalMin, intervalMax);
                vehicle.timestampCheck = vehicleActivateTime;
                float t;
                
                if (useT)
                    t = Mathf.Max(vehicle.VehicleData.bridgeCrossVariableT * weight,bridge.curveSinkSO.EvaluateByValueFirst(vehicle.collisionHeight));
                else
                    t = 10000f;
                // float startDelay = Random.Range(vehicle.bridgeStartTime, vehicle.bridgeEndTime - t - 0.05f);
                // 난이도 쉬움 : t 없음
                
                canSpawn = false;
                float waitTime = Random.Range(Mathf.Min(t + 0.1f, vehicle.bridgeCrossingTime),
                    vehicle.bridgeCrossingTime);
                // print($"{vehicle.GetInstanceID()%10} wait start {timestamp + deltaTime} ~ end {timestamp + deltaTime + waitTime}({waitTime})");
                // print($"{vehicleActivateTime}, {vehicleActivateTime + vehicle.TotalBeforeTime+ vehicle.bridgeStartTime} {vehicleActivateTime +vehicle.TotalBeforeTime+ vehicle.bridgeEndTime}");
                StartCoroutine(WaitSpawnRoutine(waitTime));
            }
        }

        return;
        // 일단 비활성화
        if (PedcanSpawn)
        {
            GameObject Ped = SummonPedestrian();
            Ped.SetActive(false);
            PedcanSpawn = false;
            BaseVehicle vehicle = Ped.GetComponent<BaseVehicle>();
            schedule.Add(timestamp + deltaTime - vehicle.TotalBeforeTime, Ped);
            intervalRemain = Random.Range(intervalMin, intervalMax);
            vehicle.timestampCheck = timestamp + deltaTime - vehicle.TotalBeforeTime;
            StartCoroutine(WaitSpawnPed(Random.Range(3, 5))); // 생성 인터벌 3~5초로 - machamy
        }


    }

    private IEnumerator WaitSpawnRoutine (float time)
    {
        // yield return new WaitForSeconds(startWait);
        // canSpawn = false;
        yield return new WaitForSeconds(time);
        canSpawn = true;
    }

    private IEnumerator WaitSpawnPed(float time)
    {
        yield return new WaitForSeconds(time);
        PedcanSpawn = true;
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
            weight = Mathf.Max(0.05f, weight); // 최솟값(임의) 설정
        }
    }
    [ContextMenu("SummonRandomByData")]
    public GameObject SummonRandomByData()
    {
        GameObject prefab;
        bool isCar = Random.Range(0, 2) == 1;
        Rarity rarity = (Rarity)Utilties.WeightedRandom(50,30,20);
        GameObject[] prefabArr = cars;
        List<VehicleDataSO> vehicleDataList = carDataList;
        if (!isCar)
        {
            prefabArr = ships;
            vehicleDataList = shipDataList;
        }
        var candidates = GetSummonables(rarity, vehicleDataList);
        
        while (candidates.Count == 0)
        {
            rarity = (Rarity)Utilties.WeightedRandom(50,30,20);
            candidates = GetSummonables(rarity, vehicleDataList);
        }
        VehicleDataSO data = candidates[Random.Range(0, candidates.Count)];
        var line = isCar ? GetCarLine(data) : GetShipLine(data);


        // 거꾸로 이동 부여
        GameObject result = Instantiate(prefabArr[data.prefabID]);
        InitVehicle(result.GetComponent<BaseVehicle>(),line,data.rawAfterCurve,data:data,isReverse:line.isReverse);
        
        return result;
    }
    [ContextMenu("SummonDebug")]
    public GameObject SummonDebug()
    {
        GameObject prefab;
        bool isCar = Random.Range(0, 2) == 1;
        Rarity rarity = (Rarity)Utilties.WeightedRandom(50,30,20);
        GameObject[] prefabArr = cars;
        List<VehicleDataSO> vehicleDataList = carDataList;
        if (!isCar)
        {
            prefabArr = ships;
            vehicleDataList = shipDataList;
        }
        var candidates = GetSummonables(rarity, vehicleDataList);
        
        while (candidates.Count == 0)
        {
            rarity = (Rarity)Utilties.WeightedRandom(50,30,20);
            candidates = GetSummonables(rarity, vehicleDataList);
        }
        VehicleDataSO data = candidates[Random.Range(0, candidates.Count)];
        var line = isCar ? GetCarLine(data) : GetShipLine(data);


        // 거꾸로 이동 부여
        GameObject result = Instantiate(prefabArr[data.prefabID]);
        InitVehicle(result.GetComponent<BaseVehicle>(),line,data.rawAfterCurve,data:data,isReverse:line.isReverse);
        
        return result;
    }
    private Line GetCarLine(VehicleDataSO data)
    {
        Line line;
        bool isFirstLine = Random.Range(0, 2) == 1;
        if (useReverse && Random.Range(0, 100) < data.reverseValue)
            line = isFirstLine ? carUpperRev : carLowerRev;
        else
            line = isFirstLine ? carUpper : carLower;
        return line;
    }
    
    private Line GetShipLine(VehicleDataSO data)
    {
        Line line;
        bool isFirstLine = Random.Range(0, 2) == 1;
        line = isFirstLine ? shipLeft : shipRight;
        return line;
    }

    private List<VehicleDataSO> GetSummonables(Rarity rarity, List<VehicleDataSO> vehicleDataSoList)
    {
        var resultList = from data in vehicleDataSoList where data.open <= score.Value && rarity == data.Rarity select data;
        // foreach (var data in vehicleDataSoList)
        // {
        //     print($"{data.name} : {data.open <= score.Value} {data.Rarity == rarity} ({rarity})");
        // }
        return resultList.ToList();
    }

    [ContextMenu("SummonRandom"), Obsolete("Use SummonRandomByData()")]
    public GameObject SummonRandom(bool applyWeight = true)
    {
        GameObject go;
        Transform spawnPoint, waitPoint, nextwaitPoint, endPoint;
        bool isLeft = Random.Range(0, 2) == 0;
        bool isCar = Random.Range(0, 100) < 50;
        #if UNITY_EDITOR
        if(dbgSummonCar)
            isCar = true;
        else if (dbgSummonShip)
            isCar = false;
        #endif
        Line PickedLine;
        bool isReverse = false;
        int pickedId = Random.Range(0, isCar ? cars.Length : ships.Length);
        #if UNITY_EDITOR
            if(dbgSummonCar && dbgCarId >= 0)
                pickedId = dbgCarId;
            else if (dbgSummonShip&& dbgShipId >= 0)
                pickedId = dbgShipId;
        #endif
        if (isCar)
        {
            // isReverse = Random.Range(0, 2) == 1;
            PickedLine = isLeft ? carUpper : carLower;
            go = Instantiate(cars[pickedId]);
        }
        else
        {
            PickedLine = isLeft ? shipLeft : shipRight;
            go = Instantiate(ships[pickedId]);
        }
        go.transform.position = isReverse ? PickedLine.End.position : PickedLine.Spawn.position;
        BaseVehicle vehicle = go.GetComponent<BaseVehicle>();
        InitVehicle(vehicle,PickedLine,"Linear",isReverse:isReverse);
        return go;
    }

    [ContextMenu("SummonPedestrian")]
    public GameObject SummonPedestrian()
    {
        GameObject Ped;
        Transform spawnPoint, waitPoint, nextwaitPoint, endPoint;
        bool isPed = Random.Range(0, 3) == 0;
        Line PickedLine;
        bool isReverse = false;
        PickedLine = pedestrianLine;
        Ped = Instantiate(pedestrianunit[0]);
        Ped.transform.position = PickedLine.Spawn.position;
        BaseVehicle vehicle = Ped.GetComponent<BaseVehicle>();
        InitVehicle(vehicle, PickedLine, "Linear", isReverse:isReverse);
        return Ped;
    }
    private void InitVehicle(BaseVehicle vehicle, Line line, string curveSo,VehicleDataSO data = null, bool isReverse = false, bool applyWeight = true)
    {
        InitVehicle(vehicle, line, GetCurve(curveSo),data, isReverse:isReverse);
    }
    

    private void InitVehicle(BaseVehicle vehicle,Line line,CurveSO curveSo,VehicleDataSO data = null,bool isReverse = false, bool applyWeight = true)
    {
        if(data)
            vehicle.name = data.vehicleName;
        vehicle.MoveLine = line;
        vehicle.isReverse = isReverse;
        vehicle.beforeCurveSo = GetCurve("Linear");
        vehicle.curveSO = curveSo;
        vehicle.currentDistance = 0;
        vehicle.bridgeController = bridge;
        vehicle.playerHp = GameManager.Instance.hp;
        vehicle.playerScore = GameManager.Instance.score;
        vehicle.deathEffect = vehicle.type == BaseVehicle.VehicleType.Car ? bubbleEffect : explodeEffect;
        vehicle.transform.position = line.Spawn.position;

        data?.ApplyData(vehicle);
        // print(data);
        if (applyWeight)
        {
            vehicle.priorMoveDelay *= weight;
            vehicle.priorWaitDelay *= weight;
            vehicle.afterMovingTime *= weight;
        }
        // 다리 건너는 시간 계산
        
        vehicle.InitBridgeCrossingTime();
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
