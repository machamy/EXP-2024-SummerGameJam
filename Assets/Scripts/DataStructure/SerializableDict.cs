using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.DataStructure
{
    [Serializable]
    public class SerializableDict<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
    {
        public static V Null = default;
        public List<SerializableData<K, V>> dataList = new List<SerializableData<K, V>>();
        
        /// <summary>
        /// 리스트 -> 딕셔너리
        /// </summary>
        public void OnBeforeSerialize()
        {
            dataList.Clear();
            foreach (KeyValuePair<K, V> pair in this)
            {
                dataList.Add(new SerializableData<K, V>(pair.Key,pair.Value));
            }
        }

        /// <summary>
        /// 딕셔너리 -> 리스트
        /// </summary>
        public void OnAfterDeserialize()
        {
            this.Clear();
            foreach (var data in dataList)
            {
                if (this.Keys.Contains(data.key))
                    data.key = default(K);
                if (!TryAdd(data.key,data.value))
                {
                    Debug.LogWarning($"같은 키 값은 들어갈 수 없습니다 (key: {data.key})");
                };
            }
        }
    }
    
    [Serializable]
    public class SerializableData<K,V>
    {
        public K key;
        public V value;
        
        public SerializableData(K key, V val)
        {
            this.key = key;
            this.value = val;
        }
    }
}