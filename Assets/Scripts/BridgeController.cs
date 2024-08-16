using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public GameObject bridge;
    public float MoveTime = 1.0f;
    public float progress = 0.0f;
    public float heightweight = 1.0f;
    Vector3 originalPos;


    bool IsSunken = false;
    
    public AnimationCurve curveSink;
    public AnimationCurve curveGoup;
    public Rigidbody2D playerRigidbody;
    void Start()
    {
        originalPos = transform.position;
        // Rigidbody2D 컴포넌트를 할당
        playerRigidbody = GetComponent<Rigidbody2D>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsSunken)
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                Debug.Log("잠수중 차량 이동");
                Debug.Log("Life 1--");
            } 
        }
        else if (!IsSunken)
        {
            if (collision.gameObject.CompareTag("Ship"))
            {
                Debug.Log("다리,선박 충돌");
                Debug.Log("Life 1--");
            }
        }
    }

    void Update()
    {
        AnimationCurve sellecteCurve;
        if (Input.GetKey(KeyCode.Space)) 
        {
            progress = Mathf.Min(MoveTime, progress+Time.deltaTime);
            sellecteCurve = curveSink;
/*            if (time > 0.1f) 
            { 
                IsSunken = true;
                Debug.Log("Bridge Sink");
               //transform.position = 
            }*/
        }
        else
        {
            progress = Mathf.Max(0, progress-Time.deltaTime);
            sellecteCurve = curveGoup;
        }

      /*  if(Input.GetKeyUp(KeyCode.Space))
        {
            time += Time.deltaTime;
            if (time > 0.1f) 
            { 
                IsSunken = false;
                //Debug.Log("Bridge Not sink");
            }
        }*/
        float height = sellecteCurve.Evaluate(Mathf.Lerp(0, 1, progress/MoveTime)) * heightweight;
        transform.position = new Vector3(originalPos.x, height, originalPos.z); 
        
    }


}
