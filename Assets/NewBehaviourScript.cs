using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public AnimationCurve Curve;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 MoveVector = direction * (speed * Time.fixedDeltaTime);
        transform.position += MoveVector;
        transform.position = transform.position + MoveVector;
        transform.Translate(MoveVector,Space.World);

        Rigidbody2D rigidbody2D = transform.GetComponent<Rigidbody2D>();
        rigidbody2D.MovePosition(transform.position + MoveVector);
        
    }
}
