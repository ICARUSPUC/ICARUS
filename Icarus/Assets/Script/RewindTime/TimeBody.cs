using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Essencial para a lista

public class TimeBody : MonoBehaviour
{
    public bool isrewinding = false;

    List<PointsInTime> pointsintime;
    Rigidbody rb;
    bool DestroyAfterpointsintime = false;

    // Variável para armazenar o ponto de segurança (Checkpoint)
    private PointsInTime checkpointPoint;

    void Start()
    {
        pointsintime = new List<PointsInTime>();
        rb = GetComponent<Rigidbody>();
    }

    void DestroyOnthisTimeLine()
    {
        if (DestroyAfterpointsintime)
            return;
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
        if (DestroyAfterpointsintime)
            return;

        if (isrewinding)
            Rewind();
        else
            Record();
    }

    void Record()
    {
        if (pointsintime.Count > Mathf.Round(4f / Time.fixedDeltaTime))
        {
            pointsintime.RemoveAt(pointsintime.Count - 1);
        }

        pointsintime.Insert(0, new PointsInTime(transform.position, transform.rotation));
    }

    void Rewind()
    {
        if (pointsintime.Count > 0 && rb != null)
        {
            PointsInTime pointInTime = pointsintime[0];
            rb.MovePosition(pointInTime.position);
            rb.MoveRotation(pointInTime.rotation);
            pointsintime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        isrewinding = true;
        if (rb != null)
        { rb.isKinematic = true; }

    }

    public void StopRewind()
    {
        isrewinding = false;
        if (rb != null)
        { rb.isKinematic = false; }
    }

    // MÉTODO NOVO 1: Salva o ponto atual como um checkpoint
    public void SaveCheckpoint()
    {
        if (rb != null) // Checagem de segurança
        {
            checkpointPoint = new PointsInTime(rb.position, rb.rotation);
        }

    }

    // MÉTODO NOVO 2: Teleporta o objeto para o ponto salvo
    public void TeleportToCheckpoint()
    {
        // Garante que temos um Rigidbody para mover e um ponto salvo
        if (checkpointPoint != null && rb != null)
        {
            if (isrewinding)
            {
                StopRewind();
            }

            // Teleporta o Rigidbody para a posição salva
            rb.position = checkpointPoint.position;
            rb.rotation = checkpointPoint.rotation;

            // Limpa a lista para que o rewind não volte para antes do teleporte
            pointsintime.Clear();

            Debug.Log("Teleporte de emergência! Retorno ao ponto seguro ativado.");
        }
    }
}