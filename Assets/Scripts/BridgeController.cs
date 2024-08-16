using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public GameObject bridge;
    [Tooltip("�ٸ��� ���� �������µ� �ɸ��� �ð�")]   public float MoveTime = 1.0f;       
    [Tooltip("���� �������� ������ �����")]          public float progress = 0.0f;

    [Tooltip("�ٸ� �ִϸ��̼� ����ġ")]                 public float heightweight = 1.0f;  
    [Tooltip("�ٸ� ���� ��ġ ���� ����")]               Vector3 originalPos;


    [Tooltip("�ٸ� ������ Ȯ�� ����")]                bool IsSunken = false;

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
        if (IsSunken)
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                Debug.Log("����� ���� �̵�");
                Debug.Log("Life 1--");
            } 
        }
        else if (!IsSunken)
        {
            if (collision.gameObject.CompareTag("Ship"))
            {
                Debug.Log("�ٸ�,���� �浹");
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
            IsSunken = true;
            Debug.Log("Bridge Sink");
        }
        else
        {
            progress = Mathf.Max(0, progress-Time.deltaTime);
            sellecteCurve = curveGoup;
            IsSunken = false;
            Debug.Log("Bridge Go Up");
        }

        float height = sellecteCurve.Evaluate(Mathf.Lerp(0, 1, progress/MoveTime)) * heightweight;
        transform.position = new Vector3(originalPos.x, originalPos.y + height - 1, originalPos.z); 
        
    }
}
