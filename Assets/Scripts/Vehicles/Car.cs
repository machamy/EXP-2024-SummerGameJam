using System;
using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Vehicles;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class Car : BaseVehicle
{

    public AudioSource audioSource;

    public GameObject cargfx;

    [SerializeField] private TrafficLight trafficLight;
    
    public float carCollisionHeight = 0.3f; // downheight of car
    private float startTime;
    private float distanceLength;
    public bool Isflooding = false; //�浹����
    public bool IsonBridge = false;

    private Collider2D collider;

    // IEnumerator Moving()
    // {
    //     yield return null;
    // }
    

    protected virtual IEnumerator StopRoutine()
    {
        /*if (collision.gameObject.CompareTag("Player"))
        {
          
        }*/



        WaitForSeconds wait = new WaitForSeconds(priorWaitDelay / 3f);

        trafficLight.SetLevel(1);
        // 신호등 띄우고 시간
        yield return wait;
        trafficLight.SetLevel(2);
        yield return wait;
        trafficLight.SetLevel(3);
        yield return wait;
        trafficLight.SetLevel(0);
        state = State.After;
    }

    // void Move()
    // {
    //
    //     // //transform.Translate(Vector3.right * Time.deltaTime);
    //     //
    //     // // transform.position = Vector3.MoveTowards(transform.position, EndPosition, step);
    //     // // transform.position = Vector3.MoveTowards(transform.position, GlobalData.carDirection, step);
    //     // float step = speed * Time.deltaTime;
    //     // transform.position += EndPosition * (step);
    //
    //     if (bridgeController.height <= transform.position.y)
    //     {
    //         IsonBridge=true;
    //     }
    //
    //     if (IsonBridge)
    //     {
    //                 }
    //
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.tag == "Invisible")
        // {
        //     Debug.Log("Invisible과 접촉하였습니다.");
        //     StartCoroutine(Stop());
        //
        // }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bridge"))
        {
            cargfx.transform.position = Vector3.zero;
        }
    }

    public virtual void OnCollideDown()
    {
        if (isDead)
            return;
        Isflooding = true;
        hp.Value -= 1;
        isDead = true;
        // StopAllCoroutines();
        // Destroy(gameObject);
        Debug.Log("Car flooding");
    }
    
    // public void OnCollideFront()
    // {
    //     Isflooding = true;
    //     hp.Value -= 1;
    //     StopAllCoroutines();
    //     Destroy(gameObject);
    //     Debug.Log("Front Flooding");
    // }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {

        SoundManager.Instance.Play("car_crash", SoundManager.SoundType.SFX);

        //renderer = GetComponent<SpriteRenderer>();
        trafficLight.SetLevel(0);

        collider = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    protected virtual void Update()
    {


    }
    

    protected virtual void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (state == State.Stop)
        {
            state = State.Wait;
            StartCoroutine(StopRoutine());
        }
        // else if(state == State.AfterMoving)
        // {
        //     Move();
        // }
    }

    protected virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
        Debug.Log("사라짐");
    }
}
