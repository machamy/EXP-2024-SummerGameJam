using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class VariableSO<T> : ScriptableObject
    {
        [SerializeField]private T value;

        public T Value{
            get => value;
            set {
                ValueChangeEvent.Invoke(this.value);
                this.value = value;
            }
        }

        /// <summary>
        /// 이전 값이 인자로 들어간다.
        /// </summary>
        public UnityEvent<T> ValueChangeEvent;
        
        
    }

    [CreateAssetMenu(menuName = "VariableSO/int")]
    public class IntVariableSO : VariableSO<int>
    {
    }
    [CreateAssetMenu(menuName = "VariableSO/float")]
    public class FloatVariableSO : VariableSO<int>
    {
    }
}