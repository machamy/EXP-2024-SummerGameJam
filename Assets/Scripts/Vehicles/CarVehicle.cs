using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarVehicle : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;

    public float speed = 1f;
    private float startTime;
    private float distanceLength;

    private bool state;

    // Start is called before the first frame update
    void Start()
    {

        StartPosition = transform.position;
        EndPosition = new Vector3(4, 0, 0);

        state = true;


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


}
