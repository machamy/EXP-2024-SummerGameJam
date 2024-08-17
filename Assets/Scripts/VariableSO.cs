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
                ValueChangeEvent.Invoke(value);
                this.value = value;
            }
        }

        /// <summary>
        /// 이후값이 인자로 들어간다
        /// </summary>
        public UnityEvent<T> ValueChangeEvent;
        
        
    }


    [CreateAssetMenu(menuName = "VariableSO/float")]
    public class FloatVariableSO : VariableSO<int>
    {
    }
}