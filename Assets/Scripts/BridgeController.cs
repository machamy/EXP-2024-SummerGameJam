using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    

    public Rigidbody2D playerRigidbody;
    void Start()
    {
        originalPos = transform.position;
        // Rigidbody2D 컴포넌트를 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        maskOriginalPos = spriteMask.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship = collision.GetComponent<Ship>();
            if (Ship && Ship.CollideCheck(height))
            {
                Ship.OnCollideFront();
            }
        }

        // if(collision.gameObject.CompareTag("Car"))
        // {
        //     Car = collision.GetComponent<Car>();
        //     if(Car && Car.carCollisionHeight < height)
        //     {
        //         Car.OnCollideFront();
        //     }
        // }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship = collision.GetComponent<Ship>();
            if (Ship && Ship.CollideCheck(height) && !Ship.Iscollision)
            {
                Ship.OnCollideUp();
            }
        }

        if (collision.gameObject.CompareTag("Car"))
        {
            Car = collision.GetComponent<Car>();
            var position = Car.transform.position;
            Car.cargfx.transform.position = new Vector3(position.x, position.y + Car.bridgeController.height - 1, position.z);
            if (Car && Car.carCollisionHeight > height && !Car.Isflooding)
            {
                Car.OnCollideDown();
            }
        }

    }

    private bool isInputAcitve = false;

    void Update()
    {
        AnimationCurve sellecteCurve;
        isInputAcitve = Input.GetKey(KeyCode.Space) || EventSystem.current.IsPointerOverGameObject(0);
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
        
        

        height = sellecteCurve.Evaluate(Mathf.Lerp(0, 1, this.progress/ MoveTime)) * heightweight;
        if (height < sinkHeight)
        {
            spriteMask.transform.position = new Vector3(maskOriginalPos.x, maskOriginalPos.y + 3, maskOriginalPos.z);
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
            
        bridgegfx.transform.position = new Vector3(originalPos.x, originalPos.y + height - 1, originalPos.z); 
        
    }
}
