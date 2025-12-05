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

    private Coroutine rewindCoroutine; // Referência para controlar a Coroutine
    private float rewindStartTime;      // Tempo em que o rebobinamento começou

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
        
        if (!isRewinding)
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

    private IEnumerator Rewind()
    {
      
        while (pointsInTime.Count > 0)
        {
            // Pega o ponto mais antigo (posição 0)
            PointsInTime pointInTime = pointsInTime[0];

            // Aplica posição e rotação
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;

            // Remove o ponto da lista
            pointsInTime.RemoveAt(0);

            // Espera pelo próximo FixedUpdate (para coincidir com a taxa de gravação)
            yield return new WaitForFixedUpdate();

            
            if (!isRewinding)
            {
                yield break;
            }
        }

     

        // Calcula o tempo que o rebobinamento levou até a lista se esgotar
        float timeSpentRewinding = Time.time - rewindStartTime;

        // Calcula quanto tempo ainda falta para completar o recordTime
        float timeRemaining = recordTime - timeSpentRewinding;

        
        if (timeRemaining > 0f)
        {
            
            
            yield return new WaitForSeconds(timeRemaining);
        }

     
        StopRewind();
    }

    public void StartRewind()
    {

        isRewinding = true;
        rewindStartTime = Time.time; // Registra o tempo atual

        // Interrompe qualquer coroutine anterior e inicia a nova
        if (rewindCoroutine != null)
        {
            StopCoroutine(rewindCoroutine);
        }
        rewindCoroutine = StartCoroutine("Rewind");

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void StopRewind()
    {

        isRewinding = false;

        
        if (rewindCoroutine != null)
        {
            StopCoroutine(rewindCoroutine);
            rewindCoroutine = null;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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