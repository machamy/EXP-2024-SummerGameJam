using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExplodeEffect : MonoBehaviour
{
    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
