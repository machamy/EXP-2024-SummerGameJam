using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "CurveSO")]
    public class CurveSO : ScriptableObject
    {
        public AnimationCurve Curve;
    }
}