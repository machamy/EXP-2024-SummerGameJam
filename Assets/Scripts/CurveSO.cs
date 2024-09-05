using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "CurveSO")]
    public class CurveSO : ScriptableObject
    {
        public AnimationCurve Curve;

        public float EvaluateByValueFirst(float value, int acc = 100)
        {
            for (int i = 0; i <= acc; i++)
            {
                float time = (float)i / acc;
                if (Curve.Evaluate(time) >= value)
                {
                    // Debug.Log($"[EVAL]found {Curve.Evaluate(time)} <= {value}");
                    return time;
                }
            }

            return 1.0f;
        }
        public float EvaluateByValueLast(float value, int acc = 100)
        {
            for (int i = acc; i >= 0; i--)
            {
                float time = (float)i / acc;
                if (Curve.Evaluate(time) <= value)
                {
                    return time;
                }
            }

            return 1.0f;
        }
    }
}