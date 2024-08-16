using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public GameObject bridgegfx;
    public Ship Ship;
    public GameObject Car;
    [Tooltip("다리가 잠기고 떠오르는데 걸리는 시간")]   public float MoveTime = 1.0f;       
    [Tooltip("잠기고 떠오르는 행위의 진행률")]          public float progress = 0.0f;

    [Tooltip("다리 애니메이션 가중치")]                 public float heightweight = 1.0f;  
    [Tooltip("다리 최초 위치 저장 벡터")]               Vector3 originalPos;

    [Tooltip("다리 잠김 애니메이션 곡선")] public AnimationCurve curveSink;
    [Tooltip("다리 용승 애니메이션 곡선")] public AnimationCurve curveGoup;

    

    public Rigidbody2D playerRigidbody;
    void Start()
    {
        originalPos = transform.position;
        // Rigidbody2D 컴포넌트를 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship = collision.GetComponent<Ship>();
            if (Ship && Ship.shipCollisionHeight <= progress)
            {
                Ship.GetComponent<Ship>().OnCollideFront();
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship = collision.GetComponent<Ship>();
            if (Ship && Ship.shipCollisionHeight <= progress && !Ship.Iscollision)
            {
                Ship.GetComponent<Ship>().OnCollideUp();
            }
        }
    }

    void Update()
    {
        AnimationCurve sellecteCurve;
        if (Input.GetKey(KeyCode.Space)) 
        {
            this.progress = Mathf.Min(MoveTime, this.progress+ Time.deltaTime);
            sellecteCurve = curveSink;
            Debug.Log("Bridge Sink");
        }
        else
        {
            this.progress = Mathf.Max(0, this.progress- Time.deltaTime);
            sellecteCurve = curveGoup;
            Debug.Log("Bridge Go Up");
        }

        float height = sellecteCurve.Evaluate(Mathf.Lerp(0, 1, this.progress/ MoveTime)) * heightweight;
        bridgegfx.transform.position = new Vector3(originalPos.x, originalPos.y + height - 1, originalPos.z); 
        
    }
}
