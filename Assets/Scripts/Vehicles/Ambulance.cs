﻿using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ambulance : BaseVehicle
{
    public BridgeController bridgeController;
    public GameObject cargfx;
    public SpriteRenderer renderer;

    public Vector3 StartPosition;
    public Vector3 EndPosition;

    public float speed = 1f;
    public float carCollisionHeight = 0.3f; // downheight of car
    private float startTime;
    private float distanceLength;
    public bool Isflooding = false; //�浹����
    public bool IsonBridge = false;

    private Collider2D collider;

    public enum State
    {
        start, stop, blinker, go, end
    }

    public State state;

    IEnumerator Moving()
    {
        StartCoroutine(Stop());
        yield return null;
    }

    IEnumerator Stop()
    {
        state = State.stop;


        renderer.color = new Color(1, 1, 1, 1); // 신호등 visible
        // 신호등 띄우고 시간

        yield return new WaitForSeconds(1f); // 대기 시간

        renderer.color = Color.red;
        yield return new WaitForSeconds(1f);

        renderer.color = Color.green;
        yield return new WaitForSeconds(1f);

        StartCoroutine(Go());

    }

    IEnumerator Go()
    {
        state = State.go;

        renderer.enabled = false;

        yield return null;

    }


    void Move()
    {
        if (state == State.stop)
        {
            return;
        }

        //transform.Translate(Vector3.right * Time.deltaTime);

        // transform.position = Vector3.MoveTowards(transform.position, EndPosition, step);
        // transform.position = Vector3.MoveTowards(transform.position, GlobalData.carDirection, step);
        float step = speed * Time.deltaTime;
        transform.position += EndPosition * (step);

        if (bridgeController.height <= transform.position.y)
        {
            IsonBridge = true;
        }

        if (IsonBridge)
        {
            cargfx.transform.position = new Vector3(transform.position.x, transform.position.y + bridgeController.height - 1, transform.position.z);
        }

    }

    /*void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log(other.gameObject.tag);
        StartCoroutine(Stop());

        
        if (other.gameObject.tag == "Invisible")
        {
            Debug.Log("Invisible과 접촉하였습니다.");
        

        }
    }*/

    public void OnCollideDown()
    {
        Isflooding = true;
        Debug.Log("Car flooding");
    }

    public void OnCollideFront()
    {
        Isflooding = true;
        Debug.Log("Front Flooding");
    }

    void Awake()
    {
        state = State.start; // 전투 시작알림
    }

    // Start is called before the first frame update
    void Start()
    {
        //renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, 0);

        collider = GetComponent<Collider2D>();

        StartPosition = transform.position;
        EndPosition = GlobalData.carDirection;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
       
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        Debug.Log("사라짐");
    }
}