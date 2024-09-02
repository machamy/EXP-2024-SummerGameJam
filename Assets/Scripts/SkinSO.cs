using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "SkinSO", fileName = "SkinSO")]
public class SkinSO : ScriptableObject
{
    [FormerlySerializedAs("legacySkin")] public bool isLegacySkin = true;
    public Sprite sprite;

    public Sprite upperSprite;
    public Sprite sideSprite;
    public Color BridgeColor = Color.white;
    public Color SunkenColor = new Color(0.81f, 0.94f, 1f);
}
