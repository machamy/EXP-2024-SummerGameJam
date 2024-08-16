using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class VariableSO<T> : ScriptableObject
    {
        private T value;
        public UnityEvent<T> ValueChangeEvent;
    }

    public class IntVariableSO : VariableSO<int>
    {
    }
    public class FloatVariableSO : VariableSO<int>
    {
    }
}