using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    public Color red = Color.red;
    public Color endColor = Color.green;
    public float Colorspeed = 1;

    Image imgComp;

    public Vector3 StartPosition;
    public Vector3 EndPosition;

    public float speed = 1f;
    private float startTime;
    private float distanceLength;

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

    IEnumerator Stop()
    {
        state = State.stop;

        // 신호등 띄우고 시간


        yield return new WaitForSeconds(1f); // 대기 시간

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
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, EndPosition, step);

        if (transform.position == EndPosition)
        {
            Destroy(gameObject);
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

    void Awake()
    {
        state = State.start; // 전투 시작알림
        imgComp = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();

        StartPosition = transform.position;
        EndPosition = new Vector3(4, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
    }
}
