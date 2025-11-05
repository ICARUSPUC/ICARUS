using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TimeBody : MonoBehaviour
{
     public bool isrewinding = false;

    List<PointsInTime> pointsintime;
    Rigidbody rb;
    void Start()
    {
        pointsintime = new List<PointsInTime>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            StopRewind();
        }
    }

    private void FixedUpdate()
    {
        if (isrewinding)
            Rewind();
        else
            Record();
    }

    void Record ()
    {
        if (pointsintime.Count > Mathf.Round(4f / Time.fixedDeltaTime))
        {
            pointsintime.RemoveAt(pointsintime.Count - 1);
        }

        pointsintime.Insert(0, new PointsInTime(transform.position , transform.rotation));
    }

    void Rewind()

    {
        if (pointsintime.Count > 0)
        {
            PointsInTime pointInTime = pointsintime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsintime.RemoveAt(0);
        }
        else
            {
            StopRewind();

        }
    }

    public void StartRewind () 
        { 

         isrewinding = true;
        if (rb != null)
        { rb.isKinematic = true; }
        
        }

    public void StopRewind ()
    {
        isrewinding = false;
        if (rb != null)
        { rb.isKinematic = false; }
    }
}
