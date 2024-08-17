using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
