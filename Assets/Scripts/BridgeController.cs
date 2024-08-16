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
    [Tooltip("�ٸ��� ���� �������µ� �ɸ��� �ð�")]   public float MoveTime = 1.0f;       
    [Tooltip("���� �������� ������ �����")]          public float progress = 0.0f;

    [Tooltip("�ٸ� �ִϸ��̼� ����ġ")]                 public float heightweight = 1.0f;  
    [Tooltip("�ٸ� ���� ��ġ ���� ����")]               Vector3 originalPos;

    [Tooltip("�ٸ� ��� �ִϸ��̼� �")] public AnimationCurve curveSink;
    [Tooltip("�ٸ� ��� �ִϸ��̼� �")] public AnimationCurve curveGoup;

    

    public Rigidbody2D playerRigidbody;
    void Start()
    {
        originalPos = transform.position;
        // Rigidbody2D ������Ʈ�� �Ҵ�
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
