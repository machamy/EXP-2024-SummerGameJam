using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{


    public Vector3 StartPosition;
    public Vector3 EndPosition;

    public float speed = 1f;
    private float startTime;
    private float distanceLength;

    private bool state;

    private BoxCollider testcollider;

    // Start is called before the first frame update
    void Start()
    {
        testcollider = GetComponent<BoxCollider>();

        StartPosition = transform.position;
        EndPosition = new Vector3(4, 0, 0);

        state = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("접촉하였습니다.");
    }

    // Update is called once per frame
    void Update()
    {
     

        //transform.Translate(Vector3.right * Time.deltaTime);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, EndPosition, step);

        if (transform.position == EndPosition)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {

    }

}
