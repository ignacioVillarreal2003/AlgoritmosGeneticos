using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    [Header("Player options")]
    [SerializeField]
    private GameObject playerPrefab;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    private int numberOfPlayers = 20;
    
    [Header("Player movements")]
    [SerializeField]
    private int numberOfMovements = 100;
    [SerializeField]
    private float playerMoveCooldown = 0.2f;
    private List<List<(int, int, int, int)>> movementsToAsign = new List<List<(int, int, int, int)>>();

    [Header("Checkpoints")]
    [SerializeField]
    private GameObject checkpoint;

    [Header("Generations")]
    [SerializeField]
    private int numGenerations = 20; 
    private List<(List<(int, int, int, int)>, float)> generacion = new List<(List<(int, int, int, int)>, float)>();
    private int actualGeneration = 1;
    
    [Header("Other variables")]
    private float timeToFinishGeneration = 0;
    private bool canUpdate = true;

    void Start()
    {
        timeToFinishGeneration = playerMoveCooldown * numberOfMovements;
        GenerateMovements();
        GeneratePopulation();
    }

    void Update()
    {
        if (numGenerations >= actualGeneration) {
            timeToFinishGeneration -= Time.deltaTime;
            if (timeToFinishGeneration < 0 && canUpdate) {
                UpdateData();
                canUpdate = false;
                NextGeneration();
            }
        }
    }

    public void UpdateData()
    {
        foreach (GameObject player in players)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement)
            {
                generacion.Add((playerMovement.GetMovements(), CalculateFitness(player, checkpoint)));
            }
        }  
    }

    (List<(int, int, int, int)>, List<(int, int, int, int)>) Cross(List<(int, int, int, int)> parent1, List<(int, int, int, int)> parent2)
    {
        int pibote = Random.Range(parent1.Count / 4, parent1.Count / 2);

        List<(int, int, int, int)> children1 = new List<(int, int, int, int)>();
        List<(int, int, int, int)> children2 = new List<(int, int, int, int)>();
        children1.AddRange(parent1.GetRange(0, pibote));
        children1.AddRange(parent2.GetRange(pibote, parent2.Count - pibote));
        children2.AddRange(parent2.GetRange(0, pibote));
        children2.AddRange(parent1.GetRange(pibote, parent1.Count - pibote));
        
        return (children1, children2);
    }

    void Mutate(List<(int, int, int, int)> children)
    {
        int pibote = Random.Range(0, children.Count);
        int probability = Random.Range(0, children.Count);
        float randomNumber = UnityEngine.Random.Range(0, 4);
        (int, int, int, int) movementsToAsign;
        if (randomNumber == 0) {
            movementsToAsign = (1, 0, 0, 0);
        } else if (randomNumber == 1) {
           movementsToAsign = (0, 1, 0, 0);
        } else if (randomNumber == 2) {
            movementsToAsign = (0, 0, 1, 0);
        } else {
            movementsToAsign = (0, 0, 0, 1);
        }
        if (probability <= children.Count / 4) {
            children[pibote] = movementsToAsign;
        }
    }

    public List<(int, int, int, int)> Selection ()
    {
        generacion.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        float totalWeight = 0f;
        foreach ((List<(int, int, int, int)>, float) g in generacion) {
            totalWeight += g.Item2;
        }
        float randomValue = Random.Range(totalWeight/1.5f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < generacion.Count; i++)
        {
            cumulativeWeight += generacion[i].Item2;
            if (randomValue <= cumulativeWeight)
            {
                return generacion[i].Item1;
            }
        }
        return generacion[generacion.Count - 1].Item1;
    }

    public void NextGeneration()
    {
        DeletePopulation();
        actualGeneration++;
        GenerateMovements();
        GeneratePopulation();
        timeToFinishGeneration = playerMoveCooldown * numberOfMovements;
        canUpdate = true;
    }

    float CalculateFitness(GameObject player, GameObject checkpoint)
    {
        float distancia = Vector3.Distance(player.transform.position, checkpoint.transform.position);
        return 1f / distancia;
    }

    public void GenerateMovements()
    {
        if (actualGeneration == 1)
        {
            for (int n = 0; n < numberOfPlayers; n++) {
                List<(int, int, int, int)> movements = new List<(int, int, int, int)>();
                for (int i = 0; i < numberOfMovements; i++){
                    float randomNumber = UnityEngine.Random.Range(0, 4);
                    if (randomNumber == 0) {
                        movements.Add((1, 0, 0, 0));
                    } else if (randomNumber == 1) {
                        movements.Add((0, 1, 0, 0));
                    } else if (randomNumber == 2) {
                        movements.Add((0, 0, 1, 0));
                    } else {
                        movements.Add((0, 0, 0, 1));
                    }
                }
                movementsToAsign.Add(movements);
            }
        } else {
            for (int n = 0; n < numberOfPlayers / 2; n++) {
                List<(int, int, int, int)> parent1 = Selection();
                List<(int, int, int, int)> parent2 = Selection();
                (List<(int, int, int, int)>, List<(int, int, int, int)>) childrens = Cross(parent1, parent2);
                Mutate(childrens.Item1);
                Mutate(childrens.Item2);
                movementsToAsign.Add(childrens.Item1);
                movementsToAsign.Add(childrens.Item2);
            }
        }
    }

    public List<(int, int, int, int)> GetMovements()
    {
        if (movementsToAsign.Count > 0) {
            List<(int, int, int, int)> lastMovement = movementsToAsign[movementsToAsign.Count - 1];
            movementsToAsign.RemoveAt(movementsToAsign.Count - 1);
            return lastMovement;
        }
        return null;
    }
    
    void DeletePopulation()
    {
        foreach (GameObject p in players)
        {
            Destroy(p);
        }
        players.Clear();
    }

    void GeneratePopulation()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            players.Add(player);
        }
    }

    public float GetPlayerMoveCooldown()
    {
        return playerMoveCooldown;
    }

    public float GetActualGeneration()
    {
        return actualGeneration;
    }
}
