using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selections : MonoBehaviour
{
    private GeneticController geneticController;
    private List<PlayerController> population = null;

    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
    }

    public PlayerController NormalSelection()
    {
        population = geneticController.getPopulation();
        float totalWeight = 0f;
        foreach (PlayerController p in population) {
            totalWeight += p.getFitness();
        }
        float randomValue = Random.Range(totalWeight/2f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < population.Count; i++)
        {
            cumulativeWeight += population[i].getFitness();
            if (randomValue <= cumulativeWeight)
            {
                return population[i];
            }
        }
        return population[population.Count - 1];
    }
}
