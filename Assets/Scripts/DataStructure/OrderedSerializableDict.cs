using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.DataStructure
{
    [Serializable]
    public class OrderedSerializableDict<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
    {
        public static V Null = default;
        public List<SerializableData<K, V>> dataList = new List<SerializableData<K, V>>();
        private IComparer<K> _comparer;

        public OrderedSerializableDict(IComparer<K> comparer = null)
        {
            _comparer = comparer ?? Comparer<K>.Default;
        }
        
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
            dataList.Sort((x, y) => _comparer.Compare(x.key, y.key));
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
    
}