using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Vehicles;
using ColorUtility = UnityEngine.ColorUtility;

public class BridgeController : MonoBehaviour
{
    public GameObject bridgegfx;
    public Ship Ship;
    public Car Car;
    [Tooltip("다리가 잠기고 떠오르는데 걸리는 시간")]   public float MoveTime = 1.0f;       
    [Tooltip("잠기고 떠오르는 행위의 진행률")]          public float progress = 0.0f;
    public float height;

    [Tooltip("다리 애니메이션 가중치")]                 public float heightweight = 1.0f;  
    [Tooltip("다리 최초 위치 저장 벡터")]               Vector3 originalPos;

    [Tooltip("다리 잠김 애니메이션 곡선")] public AnimationCurve curveSink;
    [Tooltip("다리 용승 애니메이션 곡선")] public AnimationCurve curveGoup;
    [SerializeField, Tooltip("잠김 색 처리")] private float sinkHeight;
    [SerializeField, Tooltip("잠김 마스크")] private SpriteMask spriteMask;

    [SerializeField, Tooltip("잠김 마스크 기존위치")] private Vector3 maskOriginalPos;

    [SerializeField] private SpriteRenderer[] renderers;
    [Header("Skin")] 
    [SerializeField] private SkinSO skinSo;

    [SerializeField] private Color bridgeColor = Color.white;
    [SerializeField] private Color sunkenColor = new Color(0.81f, 0.94f, 1f);
    private static Color defaultSunkenColor = new Color(0.81f, 0.94f, 1f);
    public SkinSO SkinSo
    {
        get => skinSo;
        set
        {
            skinSo = value;
            bridgeColor = skinSo.BridgeColor;
            sunkenColor = skinSo.SunkenColor;
            if (value.isLegacySkin)
            {
                var bridgeRenderer = bridgegfx.GetComponent<SpriteRenderer>();
                var sunkenRenderer = bridgegfx.transform.GetChild(0).GetComponent<SpriteRenderer>();
                bridgeRenderer.sprite = skinSo.sprite;
                bridgeRenderer.color = bridgeColor;
                sunkenRenderer.sprite = skinSo.sprite;
                sunkenRenderer.color = sunkenColor;
            }
        }
    }
    
    void Start()
    {
        originalPos = transform.position;
        // Rigidbody2D 컴포넌트를 할당
        maskOriginalPos = spriteMask.transform.position;
    }

    
#region legacy
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ship"))
    //     {
    //         Ship = collision.GetComponent<Ship>();
    //         if (Ship && Ship.CollideCheck(height))
    //         {
    //             Ship.OnCollideFront();
    //         }
    //     }
    //
    //     // if(collision.gameObject.CompareTag("Car"))
    //     // {
    //     //     Car = collision.GetComponent<Car>();
    //     //     if(Car && Car.carCollisionHeight < height)
    //     //     {
    //     //         Car.OnCollideFront();
    //     //     }
    //     // }
    // }
    //

    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     // if (collision.gameObject.CompareTag("Ship"))
    //     // {
    //     //     Ship = collision.GetComponent<Ship>();
    //     //     if (Ship && Ship.CollideCheck(height) && !Ship.Iscollision)
    //     //     {
    //     //         Ship.OnCollideUp();
    //     //     }
    //     // }
    //
    //     if (collision.gameObject.CompareTag("Car"))
    //     {
    //         Car = collision.GetComponent<Car>();
    //         var position = Car.transform.position;
    //         Car.cargfx.transform.position = new Vector3(position.x, position.y - Car.bridgeController.height + 0.8f, position.z);
    //         if (Car && Car.carCollisionHeight > height && !Car.Isflooding)
    //         {
    //             Car.OnCollideDown();
    //         }
    //     }
    // }
    

    #endregion
    

    private bool isInputAcitve = false;

    void Update()
    {
        isInputAcitve = Input.GetKey(KeyCode.Space) || Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
    
    

    private void FixedUpdate()
    {
        AnimationCurve sellecteCurve;
        if (isInputAcitve) 
        {
            this.progress = Mathf.Min(MoveTime, this.progress+ Time.deltaTime);
            sellecteCurve = curveSink;
            //Debug.Log("Bridge Sink");
        }
        else
        {
            this.progress = Mathf.Max(0, this.progress- Time.deltaTime);
            sellecteCurve = curveGoup;
            //Debug.Log("Bridge Go Up");
        }
        
        

        height = sellecteCurve.Evaluate(Mathf.Lerp(0, 1, this.progress/ MoveTime));
        if (height < sinkHeight)
        {
            spriteMask.transform.position = new Vector3(maskOriginalPos.x, maskOriginalPos.y, maskOriginalPos.z);
            foreach (var renderer in renderers)
            {
                renderer.sortingLayerName = "SunkenBridge";
            }
        }
        else
        {
            spriteMask.transform.position = new Vector3(maskOriginalPos.x, maskOriginalPos.y, maskOriginalPos.z);
            foreach (var renderer in renderers)
            {
                renderer.sortingLayerName = "Bridge";
            }
        }
            
        bridgegfx.transform.position = new Vector3(originalPos.x, originalPos.y + height* heightweight - 1* heightweight, originalPos.z); 
    }

    public void OnValidate()
    {
        SkinSo = skinSo;
    }
}
