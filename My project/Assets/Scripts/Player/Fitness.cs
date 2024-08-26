using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fitness : MonoBehaviour
{
    private GameObject end;
    private GameObject player;

    void Start()
    {
        player = gameObject;
        end = GameObject.FindGameObjectWithTag("End");
    }

    public float CalcularFitness()
    {
        float distancia = Vector3.Distance(player.transform.position, end.transform.position);
        return distancia;
    }
}
