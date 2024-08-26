using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int numberOfObjects = 20;
    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }
    }
}
