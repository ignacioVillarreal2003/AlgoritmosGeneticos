using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutations : MonoBehaviour
{
    private GeneticController geneticController;
    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
    }

    public void NormalMutation(PlayerController adn)
    {
        float mutationRate = geneticController.getMutationRate();
        for (int i = 0; i < adn.genes.Length; i++)
        {
            if (Random.Range(0f, 1f) < mutationRate)
            {
                int randomNumber = Random.Range(0, 4);
                if (randomNumber == 0) {
                    adn.genes[i] = (1, 0, 0, 0);
                } else if (randomNumber == 1) {
                    adn.genes[i] = (0, 1, 0, 0);
                } else if (randomNumber == 2) {
                    adn.genes[i] = (0, 0, 1, 0);
                } else {
                    adn.genes[i] = (0, 0, 0, 1);
                }
            }
        }
    }
}
