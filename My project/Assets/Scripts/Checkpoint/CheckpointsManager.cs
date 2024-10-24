using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    /* Checkpoints */
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    private int currentCheckpoint = 0;

    /* Referencias a objetos */
    void Awake()
    {
        foreach (Checkpoint c in FindObjectsOfType<Checkpoint>())
        {
            checkpoints.Add(c);
        }
        checkpoints.Sort((a, b) => a.getOrder().CompareTo(b.getOrder()));
    }

    /* Carga el checkpont actual en los individuos */
    public void LoadCheckpoints()
    {
        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            p.SetCheckpoint(checkpoints[currentCheckpoint]);
        }
    }

    /* Cambia al siguiente checkpoint */
    public void NextCheckpoint()
    {
        if (currentCheckpoint < checkpoints.Count - 1)
        {
            currentCheckpoint++;
        }
    }

    /* Verificar si es el final */
    public bool IsTheEnd()
    {
        if (currentCheckpoint == checkpoints.Count - 1)
        {
            return true;
        }
        return false;
    }
}
