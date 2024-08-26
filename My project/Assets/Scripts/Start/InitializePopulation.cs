using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePopulation : MonoBehaviour
{
    public GameObject playerPrefab;
    private int numberOfObjects = 20;
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        GeneratePopulation();
    }

    void GeneratePopulation()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            players.Add(player);
        }
    }

    public List<GameObject> GetPlayers()
    {
        return players;
    }
}
