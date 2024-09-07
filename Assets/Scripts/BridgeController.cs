using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DefaultNamespace;
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
    public float height = 1;

    [Tooltip("다리 애니메이션 가중치")]                 public float heightweight = 1.0f;  
    [Tooltip("다리 최초 위치 저장 벡터")]               Vector3 originalPos;

    [Tooltip("다리 잠김 애니메이션 곡선")]public CurveSO curveSinkSO;
    public AnimationCurve curveSink => curveSinkSO.Curve;
    
    [Tooltip("다리 용승 애니메이션 곡선")]  public CurveSO curveGoupSO;
    public AnimationCurve curveGoup => curveGoupSO.Curve;
    [SerializeField, Tooltip("잠김 색 처리")] private float sinkHeight;
    [SerializeField, Tooltip("잠김 마스크")] private SpriteMask spriteMask;

    [SerializeField, Tooltip("잠김 마스크 기존위치")] private Vector3 maskOriginalPos;

    [SerializeField] private SpriteRenderer[] renderers;
    [Header("Skin")] 
    [SerializeField] private SkinSO skinSo;

    
    [SerializeField] private Color bridgeColor = Color.white;
    [SerializeField] private Color sunkenColor = new Color(0.81f, 0.94f, 1f);
    private static Color defaultSunkenColor = new Color(0.81f, 0.94f, 1f);
    private AnimationCurve selectedCurve;  // 클래스 레벨에서 selectedCurve 선언
    private AnimationCurve previousCurve;  // 이전 곡선을 저장할 변수
    public SkinSO SkinSo
    {
        get => skinSo;
        set
        {
            skinSo = value;
            bridgeColor = skinSo.BridgeColor;
            sunkenColor = skinSo.SunkenColor;
            
            var bridgeRenderer = bridgegfx.GetComponent<SpriteRenderer>();
            var sunkenRenderer = bridgegfx.transform.GetChild(0).GetComponent<SpriteRenderer>();
            Transform upper = bridgegfx.transform.GetChild(1);
            Transform side = bridgegfx.transform.GetChild(2);
            var upperRenderer = upper.GetComponent<SpriteRenderer>();
            var sideRenderer = side.GetComponent<SpriteRenderer>();
            if (value.isLegacySkin)
            {
                bridgeRenderer.enabled = true;
                sunkenRenderer.enabled = true;
                upperRenderer.enabled = false;
                sideRenderer.enabled = false;
                bridgeRenderer.sprite = skinSo.sprite;
                bridgeRenderer.color = bridgeColor;
                sunkenRenderer.sprite = skinSo.sprite;
                sunkenRenderer.color = sunkenColor;
            }
            else
            {
                bridgeRenderer.enabled = false;
                sunkenRenderer.enabled = false;
                
                
                upperRenderer.enabled = true;
                sideRenderer.enabled = true;
                upperRenderer.sprite = skinSo.upperSprite;
                sideRenderer.sprite = skinSo.sideSprite;
            }
        }
    }
    
    void Start()
    {
        originalPos = transform.position;
        // Rigidbody2D 컴포넌트를 할당
        maskOriginalPos = spriteMask.transform.position;
        selectedCurve = curveSink;
        previousCurve = selectedCurve;
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
    

    private bool isInputActive = false;

    void Update()
    {
        isInputActive = Input.GetKey(KeyCode.Space) || Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }



    private void FixedUpdate()
    {
        previousCurve = selectedCurve;
        float previousHeight = height;  // 이전 height 값 저장
        
        if (isInputActive)
        {
            // progress가 MoveTime을 넘지 않도록 증가
            this.progress = Mathf.Clamp(this.progress + Time.deltaTime, 0, MoveTime);
            selectedCurve = curveSink;
            // Debug.Log("Bridge Sink");
        }
        else
        {
            // progress가 0보다 작지 않도록 감소
            this.progress = Mathf.Clamp(this.progress - Time.deltaTime, 0, MoveTime);
            selectedCurve = curveGoup;
            // Debug.Log("Bridge Go Up");
        }

        // 곡선이 바뀌었다면, 이전 height에 맞는 새로운 progress 값을 찾음
        if (previousCurve != selectedCurve)
        {
            this.progress = FindProgressForHeight(selectedCurve, previousHeight);
        }

        // 현재 곡선에 따른 height 계산
        height = selectedCurve.Evaluate(Mathf.Lerp(0, 1, this.progress / MoveTime));

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

        bridgegfx.transform.position = new Vector3(originalPos.x, originalPos.y + height * heightweight - heightweight, originalPos.z);
    }

    // 주어진 height에 맞는 progress 값을 찾는 함수
    private float FindProgressForHeight(AnimationCurve curve, float targetHeight)
    {
        // 0에서 MoveTime까지의 progress에서 해당 height에 가까운 값을 찾음
        float bestProgress = 0f;
        float bestDifference = Mathf.Abs(curve.Evaluate(0) - targetHeight);

        // 일정 간격으로 progress 값을 탐색해 가장 가까운 값을 찾음
        for (float p = 0f; p <= MoveTime; p += 0.01f)
        {
            float heightAtP = curve.Evaluate(Mathf.Lerp(0, 1, p / MoveTime));
            float difference = Mathf.Abs(heightAtP - targetHeight);

            if (difference < bestDifference)
            {
                bestDifference = difference;
                bestProgress = p;
            }
        }

        return bestProgress;
    }


    public void OnValidate()
    {
        if(skinSo)
            SkinSo = skinSo;
    }
}
