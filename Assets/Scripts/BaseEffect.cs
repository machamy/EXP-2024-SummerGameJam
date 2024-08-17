
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
