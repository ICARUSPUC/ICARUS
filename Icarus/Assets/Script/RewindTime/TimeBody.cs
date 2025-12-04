using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeBody : MonoBehaviour
{
    public static TimeBody Dio;

    [Header("Configurações")]
    public float recordTime = 5f; 

    [Header("Debug / Estado")]
    public bool isRewinding = false;

   
    private List<PointsInTime> pointsInTime;

    private Rigidbody rb;
    private PointsInTime checkpointPoint; 
    private int maxListSize;

    void Start()
    {
        pointsInTime = new List<PointsInTime>();
        rb = GetComponent<Rigidbody>();

       
        maxListSize = Mathf.RoundToInt(recordTime / Time.fixedDeltaTime);
    }

    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.B))
            StartRewind();

        if (Input.GetKeyUp(KeyCode.B))
            StopRewind();

        // Controles de Checkpoint (Salvar e Teleportar)
        if (Input.GetKeyDown(KeyCode.C))
            SaveCheckpoint();

        if (Input.GetKeyDown(KeyCode.T))
            TeleportToCheckpoint();
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }


    void Record()
    {
       
        if (pointsInTime.Count > maxListSize)
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

       
        pointsInTime.Insert(0, new PointsInTime(transform.position, transform.rotation));
    }

    public void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointsInTime pointInTime = pointsInTime[0];

           
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;

            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        if (rb != null)
        {
            rb.isKinematic = true;
        } 
       
    }

    public void StopRewind()
    {
        isRewinding = false;
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero; // Reseta velocidade para não sair voando
        }
    }

    

    public void SaveCheckpoint()
    {
        if (rb != null)
        {
            checkpointPoint = new PointsInTime(transform.position, transform.rotation);
            Debug.Log("Checkpoint Salvo!");
        }
    }

    public void TeleportToCheckpoint()
    {
        if (checkpointPoint != null && rb != null)
        {
            if (isRewinding) StopRewind();

            
            transform.position = checkpointPoint.position;
            transform.rotation = checkpointPoint.rotation;

          
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            pointsInTime.Clear();

            Debug.Log("Teleportado para o Checkpoint!");
        }
    }
}