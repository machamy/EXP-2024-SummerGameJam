using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.DataStructure;
using UnityEditorInternal;
using UnityEngine;

namespace DefaultNamespace
{
    public class Line : MonoBehaviour
    {
        public enum Point : int
        {
            None,
            Spawn,
            Wait01,
            Bridge01,
            Bridge02,
            Wait02,
            End,
            
            Count = 6,
            Debug
        }
        public class PointComparer : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                return x.CompareTo(y);
            }
        }

        
        [Header("Type")]
        [SerializeField] private BaseVehicle.VehicleType vehicleType = BaseVehicle.VehicleType.Car;
        [Header("Property")]
        [SerializeField] private float length = 10;
        [SerializeField] private OrderedSerializableDict<Point, float> Distances = new OrderedSerializableDict<Point, float>(new PointComparer());
        [Header("값 체크용")]
        [SerializeField] private SerializableDict<Point, GameObject> positions;

        [Header("Debug")]
        [SerializeField] private bool debugDraw = false;

        [SerializeField] private bool lineRenderer = false;
        public Transform Spawn => positions[Point.Spawn].transform;
        public Transform Wait01 => positions[Point.Wait01].transform;
        public Transform Bridge01 => positions[Point.Bridge01].transform;
        public Transform Bridge02 => positions[Point.Bridge02].transform;
        public Transform Wait02 => positions[Point.Wait02].transform;
        public Transform End => positions[Point.End].transform;
        
        // public float SpawnDistance => Distances[Position.Spawn];
        public float Wait01Distance => Distances[Point.Wait01];
        public float Bridge01Distance => Distances[Point.Bridge01];
        public float Bridge02Distance => Distances[Point.Bridge02];
        public float Wait02Distance => Distances[Point.Wait02];
        public float EndDistance => Distances[Point.End];
        // public Transform Spawn => positions[Position.Spawn].transform;

        public void Reset()
        {
            
        }

        private void LateUpdate()
        {
            if(!debugDraw)
                return;
            Debug.DrawLine(Spawn.position,Wait01.position,Color.yellow);
            Debug.DrawLine(Wait01.position,Bridge01.position,Color.cyan);
            Debug.DrawLine(Bridge01.position,Bridge02.position,Color.green);
            Debug.DrawLine(Bridge02.position,Wait02.position,Color.cyan);
            Debug.DrawLine(Wait02.position,End.position,Color.yellow);
            
        }

        private void OnValidate()
        {
            Vector3 angle = transform.eulerAngles;
            angle.z = vehicleType == BaseVehicle.VehicleType.Car ? GlobalData.carDegree : GlobalData.shipDegree;
            transform.eulerAngles = angle;
            
            // transform.rotation = new Quaternion().
            if (Distances.Keys.Contains(Point.End))
            {
                Distances[Point.End] = length;
            }
            // 1번값(Spawn) 확인
            // 거리 체크
            if (!Distances.Keys.Contains(Point.Spawn))
            {
                Distances.Add(Point.Spawn,0);
            }
            else
            {
                Distances[Point.Spawn] = 0;
            }

            bool hasGO = positions.TryGetValue(Point.Spawn, out var go);
            // 오브젝트 체크
            if (!(hasGO && go))
            {
                positions[Point.Spawn] = new GameObject("Spawn")
                {
                    transform =
                    {
                        parent = transform,
                        localPosition = Vector3.zero
                    }
                };
            }
            Spawn.localRotation = Quaternion.identity;
            
            // 나머지 값 확인
            for (int i = 2; i <= (int)Point.Count; i++)
            {
                Point point = (Point)i;
                if (!Distances.TryGetValue(point, out var distance))
                {
                    Distances[point] = 0;
                    distance = Distances[point];
                }
                bool hasObject = positions.TryGetValue(point,out var pointObject);
                if (!hasObject || !positions[point])
                {
                    pointObject = new GameObject(point.ToString())
                    {
                        transform =
                        {
                            parent = transform
                        }
                    };
                    positions[point] = pointObject;
                }

                Vector3 newPosition = new Vector3(distance,0,0);
                pointObject.transform.localRotation = Quaternion.identity;
                pointObject.transform.localPosition = newPosition;
            }
            
            // 라인렌더러
            if (lineRenderer)
            {
                LineRenderer renderer;
                if (!TryGetComponent(out renderer))
                    renderer = gameObject.AddComponent<LineRenderer>();

                renderer.loop = false;
                renderer.useWorldSpace = false;
                renderer.sortingLayerName = "Debug";
                
                renderer.positionCount = (int)Point.Count;
                
                renderer.SetPosition(0, transform.InverseTransformPoint(Spawn.position));
                renderer.SetPosition(1,transform.InverseTransformPoint( Wait01.position));
                renderer.SetPosition(2, transform.InverseTransformPoint(Bridge01.position));
                renderer.SetPosition(3, transform.InverseTransformPoint(Bridge02.position));
                renderer.SetPosition(4, transform.InverseTransformPoint(Wait02.position));
                renderer.SetPosition(5, transform.InverseTransformPoint(End.position));
                
                if (renderer.sharedMaterial == null)
                {
                    renderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                }
                
                renderer.startColor = Color.yellow; 
                renderer.endColor = Color.red;      
                
                renderer.startWidth = 0.1f;
                renderer.endWidth = 0.1f;
            }
        }
    }
}