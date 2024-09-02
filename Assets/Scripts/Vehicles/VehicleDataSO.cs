using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

namespace Vehicles
{
    [CreateAssetMenu, Serializable]
    public class VehicleDataSO : ScriptableObject
    {
        [FormerlySerializedAs("name")] public string vehicleName;
        public BaseVehicle.VehicleType type;
        public int prefabID;
        public int spriteID;
        public string rawSprite;
        [SerializeField]private Sprite sprite;
        [SerializeField]private Sprite reverseSprite;
        public Sprite Sprite
        {
            get
            {
                if (sprite is null)
                {
                    string path = $"Sprites/Vehicle/{(type == BaseVehicle.VehicleType.Car ? "car" : "ship")}{spriteID}";
                    if (rawSprite != String.Empty && !rawSprite.Contains("-"))
                    {
                        path += $"_{rawSprite}";
                    }
                    sprite = Resources.Load<Sprite>(path);
                    if(sprite == null)
                        Debug.LogWarning($"not found: {path}");
                }

                return sprite;
            }
            set => sprite = value;
        }

        public Sprite ReverseSprite
        {
            get
            {
                if (sprite is null)
                {
                    string path = $"Sprites/Vehicle/{(type == BaseVehicle.VehicleType.Car ? "car" : "ship")}{spriteID}B";
                    if (rawSprite == String.Empty || rawSprite.Contains("-"))
                    {
                        path += $"_{rawSprite}";
                    }
                    reverseSprite = Resources.Load<Sprite>(path) ?? Sprite;
                    if(reverseSprite == null)
                        Debug.LogWarning($"not found: {path}");
                }

                return sprite;
            }
            set => sprite = value;
        }
        public float beforeTime;
        public float waitTime;
        public float afterTime;
        public string rawBeforeCurve;
        public string rawAfterCurve;
        public CurveSO beforeCurve;
        public CurveSO afterCurve;
        public Rarity Rarity;
        public int open;
        public float bridgeCrossVariableT;

        public void Init()
        {
            var s = Sprite;
            var r = reverseSprite;
        }
    }
}