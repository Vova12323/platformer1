using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public float arriveTime = 3f;
    void Start()
    {
        
    }   
        // Update is called once per frame
    void Update ()
    {
        transform.position = Vector3.Lerp(start.position, end.position, Mathf.PingPong(Time.time/arriveTime, 1f));
    }
    // Start is called before the first frame update

    // Update is called once per frame
}
