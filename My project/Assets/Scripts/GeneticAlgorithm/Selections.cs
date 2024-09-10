using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selections : MonoBehaviour
{
    private GeneticController geneticController;
    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
    }

    public PlayerController NormalSelection()
    {
        List<PlayerController> population = geneticController.getPopulation();
        float totalFitness = population.Sum(p => p.getFitness());
        float randomValue = Random.Range(totalFitness / 1.4f, totalFitness);
        float cumulativeFitness = 0f;

        foreach (PlayerController player in population)
        {
            cumulativeFitness += player.getFitness();
            if (randomValue <= cumulativeFitness)
            {
                return player;
            }
        }
        return population[population.Count - 1];
    }
}
