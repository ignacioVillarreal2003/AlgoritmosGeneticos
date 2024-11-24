using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    /* Checkpoints */
    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    /* Referencias a objetos */
    void Awake()
    {
        foreach (Checkpoint c in FindObjectsOfType<Checkpoint>())
        {
            checkpoints.Add(c);
        }
        checkpoints.Sort((a, b) => a.getOrder().CompareTo(b.getOrder()));
    }

    /* Verificar si es el final */
    public bool IsTheEnd(Checkpoint checkpoint)
    {
        if (checkpoint.getOrder() >= checkpoints.Count - 1)
        {
            return true;
        }
        return false;
    }

    /* Ultimo checkpoint */
    public Checkpoint GetLastCheckpoint()
    {
        return checkpoints.Last();
    }
}
