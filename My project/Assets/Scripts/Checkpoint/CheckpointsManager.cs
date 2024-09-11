using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    private int currentCheckpoint = 0;

    void Start()
    {
        foreach (Checkpoint c in FindObjectsOfType<Checkpoint>())
        {
            checkpoints.Add(c);
        }
        checkpoints.Sort((a, b) => a.getOrder().CompareTo(b.getOrder()));
    }

    public void InitializePlayers()
    {
        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            p.setCheckpoint(checkpoints[currentCheckpoint]);
        }
    }

    public bool NextCheckpoint()
    {
        currentCheckpoint++;
        if (currentCheckpoint < checkpoints.Count)
        {
            foreach (PlayerController p in FindObjectsOfType<PlayerController>())
            {
                p.setCheckpoint(checkpoints[currentCheckpoint]);
            }
        }
        if (currentCheckpoint == checkpoints.Count)
        {
            return true;
        } 
        else 
        {
            return false;
        }
    }
}
