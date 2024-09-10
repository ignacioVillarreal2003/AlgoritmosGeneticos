using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosses : MonoBehaviour
{
    private GeneticController geneticController;
    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
    }

    public ((int, int, int, int)[], (int, int, int, int)[]) NormalCrossOver(PlayerController parent1, PlayerController parent2)
    {
        int adnLength = geneticController.getAdnLength();
        int pivot = Random.Range(0, adnLength);

        (int, int, int, int)[] child1 = new (int, int, int, int)[adnLength];
        (int, int, int, int)[] child2 = new (int, int, int, int)[adnLength];

        for (int i = 0; i < pivot; i++)
        {
            child1[i] = parent1.genes[i];
            child2[i] = parent2.genes[i];
        }

        for (int i = pivot; i < adnLength; i++)
        {
            child1[i] = parent2.genes[i];
            child2[i] = parent1.genes[i];
        }

        return (child1, child2);
    }
}