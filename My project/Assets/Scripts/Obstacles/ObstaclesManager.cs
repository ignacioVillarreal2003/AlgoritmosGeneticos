using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstacleMovements = new List<GameObject>();

    public void RebootAll()
    {
        foreach (GameObject o in obstacleMovements)
        {
            ObstacleMovement1 om = o.GetComponent<ObstacleMovement1>();
            om.Reboot();
        }
    }
}
