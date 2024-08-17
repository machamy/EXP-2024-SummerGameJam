using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Vehicles;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class Car : BaseVehicle
{
    public BridgeController bridgeController;
    public GameObject cargfx;

    [SerializeField] private TrafficLight trafficLight;

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
        start,stop, blinker,go, end
    }

    public State state;

    IEnumerator Moving()
    {
        yield return null;
    }

    protected virtual IEnumerator Stop()
    {
        state = State.stop;
        WaitForSeconds wait = new WaitForSeconds(priorWaitDelay / 3f);

        trafficLight.SetLevel(1);
        // 신호등 띄우고 시간
        yield return wait;
        trafficLight.SetLevel(2);
        yield return wait;
        trafficLight.SetLevel(3);
        yield return wait;
        trafficLight.SetLevel(0);
        StartCoroutine(Go());

    }

    IEnumerator Go()
    {
        state = State.go;
        yield return null;

    }


    void Move()
    {
        if(state == State.stop)
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
            IsonBridge=true;
        }

        if (IsonBridge)
        {
            cargfx.transform.position = new Vector3(transform.position.x, transform.position.y + bridgeController.height - 1, transform.position.z);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log(other.gameObject.tag);


        if (other.gameObject.tag == "Invisible")
        {
            Debug.Log("Invisible과 접촉하였습니다.");
            StartCoroutine(Stop());
 
        }
    }

    public virtual void OnCollideDown()
    {
        Isflooding = true;
        Debug.Log("Car flooding");
    }

    public virtual void OnCollideFront()
    {
        Isflooding = true;
        Debug.Log("Front Flooding");
    }

    protected virtual void Awake()
    {
        state = State.start; // 전투 시작알림
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //renderer = GetComponent<SpriteRenderer>();
        trafficLight.SetLevel(0);

        collider = GetComponent<Collider2D>();

        StartPosition = transform.position;
        EndPosition = GlobalData.carDirection;

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
        Debug.Log("사라짐");
    }
}
