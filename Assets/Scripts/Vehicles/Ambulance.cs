using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ambulance : Car
{

    public float ambulanceCollisionHeight = 0.3f; // downheight of car


    IEnumerator Moving()
    {
        StartCoroutine(Stop());
        yield return null;
    }

    /*protected override IEnumerator Stop()
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


    }*/

    IEnumerator Go()
    {
        state = State.go;

        GetComponent<Renderer>().enabled = false;

        yield return null;

    }



    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log(other.gameObject.tag);
        StartCoroutine(Stop());

        
        if (other.gameObject.tag == "Invisible")
        {
            Debug.Log("Invisible과 접촉하였습니다.");
        

        }
    }

    
    public override void OnCollideDown()
    {
        Isflooding = true;
        Debug.Log("Car flooding");
    }

    public override void OnCollideFront()
    {
        Isflooding = true;
        Debug.Log("Front Flooding");
    }

    
}
    