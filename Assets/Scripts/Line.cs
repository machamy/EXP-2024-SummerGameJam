using System;
using System.Collections.Generic;
using DefaultNamespace.DataStructure;
using UnityEditorInternal;
using UnityEngine;

namespace DefaultNamespace
{
    public class Line : MonoBehaviour
    {
        public enum Position
        {
            None,
            Spawn,
            Wait01,
            Bridge01,
            Bridge02,
            Wait02,
            End
        }


        [SerializeField] public SerializableDict<Position, GameObject> positions;

        public Transform Spawn => positions[Position.Spawn].transform;
        public Transform Wait01 => positions[Position.Spawn].transform;
        public Transform Bridge01 => positions[Position.Spawn].transform;
        public Transform Bridge02 => positions[Position.Spawn].transform;
        public Transform Wait02 => positions[Position.Spawn].transform;
        public Transform End => positions[Position.Spawn].transform;
        // public Transform Spawn => positions[Position.Spawn].transform;
    }
}