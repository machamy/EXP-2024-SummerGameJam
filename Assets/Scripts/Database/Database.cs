using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vehicles;

namespace DefaultNamespace.Database
{
    public class Database : MonoBehaviour
    {
        private SpreadSheetReader sheetReader;

        private Coroutine loadRoutine;

        public List<VehicleDataSO> VehicleDataSoList;
        private void Awake()
        {
            sheetReader = new SpreadSheetReader();
            
        }

        public void Start()
        {
            StartCoroutine(UpdateDB());
        }

        private IEnumerator UpdateDB()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                loadRoutine = StartCoroutine(sheetReader.LoadData());
                StartCoroutine(WaitBreak());
                yield return loadRoutine;
                if (sheetReader.IsDataLoaded)
                {
                    string[] lines = sheetReader.rawData.Split("\n");
                    for(int i = 0; i< lines.Length; i ++)
                    {
                        string[] data = lines[i].Split("\t");
                        var vehicle = ParseVehicle(data);
                        if (i < VehicleDataSoList.Count)
                        {
                            VehicleDataSoList[i] = vehicle;
                        }
                        else
                        {
                            VehicleDataSoList.Add(vehicle);
                        }
                    }
                }
            }
            
        }

        public IEnumerator WaitBreak()
        {
            yield return new WaitForSecondsRealtime(3f);
            StopCoroutine(loadRoutine);
        }

        public VehicleDataSO ParseVehicle(string[] data)
        {
            var result = ScriptableObject.CreateInstance<VehicleDataSO>();
            result.name = data[0];
            result.vehicleName = data[0];
            result.type = data[1].ToLower() == "car" ? BaseVehicle.VehicleType.Car : BaseVehicle.VehicleType.Ship;
            result.prefabID = Convert.ToInt32(data[2]);
            result.spriteID = Convert.ToInt32(data[3]);
            result.rawSprite = data[4];
            result.beforeTime = Convert.ToSingle(data[5]);
            if (!float.TryParse(data[6], out var w))
            {
                w = 9999;
            }
            result.waitTime = w;
            result.afterTime = Convert.ToSingle(data[7]);
            result.rawBeforeCurve = data[8];
            result.rawAfterCurve = data[9];
            Rarity rarity;
            switch (data[10].ToLower())
            {
                case "normal":
                    rarity = Rarity.Normal;
                    break;
                case "rare":
                    rarity = Rarity.Rare;
                    break;
                case "special":
                    rarity = Rarity.Special;
                    break;
                default:
                    rarity = Rarity.Normal;
                    break;
            }
            result.Rarity = rarity;
            result.open = Convert.ToInt32(data[11]);
            if (!float.TryParse(data[12], out var t))
            {
                t = 9999;
            }
            result.bridgeCrossVariableT = t;
            result.Init();
            return result;
        }

        [ContextMenu("SaveSO")]
        public void SaveDataSO()
        {
            foreach (var vehicle in VehicleDataSoList)
            {
                AssetDatabase.CreateAsset(vehicle,$"Assets/ScriptableObjects/Vehicles/{vehicle.type}s/{vehicle.name}.asset");
            }
            AssetDatabase.SaveAssets();
        }
    }
}