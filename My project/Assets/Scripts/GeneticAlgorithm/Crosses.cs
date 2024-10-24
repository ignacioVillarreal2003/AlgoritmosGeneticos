using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosses : MonoBehaviour
{
    public ((int, int, int, int)[], (int, int, int, int)[]) Cross(CrossesOptions crossesOptions, (int, int, int, int)[] parent1, (int, int, int, int)[] parent2)
    {
        if (crossesOptions == CrossesOptions.SinglePointCrossover)
        {
            return SinglePointCrossover(parent1, parent2);
        }
        if (crossesOptions == CrossesOptions.TwoPointCrossover)
        {
            return TwoPointCrossover(parent1, parent2);
        }
        if (crossesOptions == CrossesOptions.MultiPointCrossover)
        {
            return MultiPointCrossover(parent1, parent2);
        }
        if (crossesOptions == CrossesOptions.BestFitnessCrossover)
        {
            return BestFitnessCrossover(parent1, parent2);
        }
        if (crossesOptions == CrossesOptions.UniformCrossover)
        {
            return UniformCrossover(parent1, parent2);
        }
        return (parent1, parent2);
    }

    public enum CrossesOptions
    {
        SinglePointCrossover,
        TwoPointCrossover,
        MultiPointCrossover,
        BestFitnessCrossover,
        UniformCrossover
    }

    /* Se selecciona aleatoriamente un punto de cruce en la cadena de genes de los padres, 
    y se intercambia todo el material genético después de ese punto para formar los hijos. */
    private ((int, int, int, int)[], (int, int, int, int)[]) SinglePointCrossover((int, int, int, int)[] parent1, (int, int, int, int)[] parent2)
    {
        int chromosomeLength = parent1.Length;
        int pivot = Random.Range(0, chromosomeLength);

        (int, int, int, int)[] genesParent1 = parent1;
        (int, int, int, int)[] genesParent2 = parent2;
        (int, int, int, int)[] child1 = new (int, int, int, int)[chromosomeLength];
        (int, int, int, int)[] child2 = new (int, int, int, int)[chromosomeLength];

        for (int i = 0; i < pivot; i++)
        {
            child1[i] = genesParent1[i];
            child2[i] = genesParent2[i];
        }

        for (int i = pivot; i < chromosomeLength; i++)
        {
            child1[i] = genesParent2[i];
            child2[i] = genesParent1[i];
        }

        return (child1, child2);
    }

    /* En este tipo de cruce, se seleccionan dos puntos en la cadena genética, 
    y el intercambio de material genético se realiza entre esos dos puntos. */
    private ((int, int, int, int)[], (int, int, int, int)[]) TwoPointCrossover((int, int, int, int)[] parent1, (int, int, int, int)[] parent2)
    {
        int chromosomeLength = parent1.Length;
        int pivot1 = Random.Range(0, chromosomeLength - 1);
        int pivot2 = Random.Range(pivot1 + 1, chromosomeLength);

        (int, int, int, int)[] genesParent1 = parent1;
        (int, int, int, int)[] genesParent2 = parent2;
        (int, int, int, int)[] child1 = new (int, int, int, int)[chromosomeLength];
        (int, int, int, int)[] child2 = new (int, int, int, int)[chromosomeLength];

        for (int i = 0; i < pivot1; i++)
        {
            child1[i] = genesParent1[i];
            child2[i] = genesParent2[i];
        }

        for (int i = pivot1; i < pivot2; i++)
        {
            child1[i] = genesParent2[i];
            child2[i] = genesParent1[i];
        }

        for (int i = pivot2; i < chromosomeLength; i++)
        {
            child1[i] = genesParent1[i];
            child2[i] = genesParent2[i];
        }

        return (child1, child2);
    }

    /* Este es una generalización del cruce de dos puntos. Se seleccionan varios 
    puntos de cruce en la cadena genética y se alterna la combinación de genes 
    entre esos puntos. */
    private ((int, int, int, int)[], (int, int, int, int)[]) MultiPointCrossover((int, int, int, int)[] parent1, (int, int, int, int)[] parent2)
    {
        int chromosomeLength = parent1.Length;
        int acumulated = 0;
        bool isChanged = true;
        
        (int, int, int, int)[] genesParent1 = parent1;
        (int, int, int, int)[] genesParent2 = parent2;
        (int, int, int, int)[] child1 = new (int, int, int, int)[chromosomeLength];
        (int, int, int, int)[] child2 = new (int, int, int, int)[chromosomeLength];

        while (acumulated < chromosomeLength)
        {
            int pivot = Random.Range(acumulated, chromosomeLength);
            if (isChanged)
            {
                for (int i = acumulated; i < pivot; i++)
                {
                    child1[i] = genesParent1[i];
                    child2[i] = genesParent2[i];
                }
            } 
            else
            {
                for (int i = acumulated; i < pivot; i++)
                {
                    child1[i] = genesParent2[i];
                    child2[i] = genesParent1[i];
                }
            }
            isChanged = !isChanged;
            acumulated = pivot;
        }

        return (child1, child2);
    }

    /* En lugar de seleccionar puntos específicos para intercambiar bloques de genes, 
    en el cruce uniforme se considera cada gen de forma independiente, con 
    una probabilidad predefinida (generalmente 50%) de que el gen de un padre sea 
    tomado o no. */
    private ((int, int, int, int)[], (int, int, int, int)[]) UniformCrossover((int, int, int, int)[] parent1, (int, int, int, int)[] parent2)
    {
        int chromosomeLength = parent1.Length;

        (int, int, int, int)[] genesParent1 = parent1;
        (int, int, int, int)[] genesParent2 = parent2;
        (int, int, int, int)[] child1 = new (int, int, int, int)[chromosomeLength];
        (int, int, int, int)[] child2 = new (int, int, int, int)[chromosomeLength];

        for (int i = 0; i < chromosomeLength; i++)
        {
            float probability = Random.Range(0f, 1f);
            if (probability < 0.5)
            {
                child1[i] = genesParent1[i];
                child2[i] = genesParent2[i];
            } else 
            {
                child1[i] = genesParent2[i];
                child2[i] = genesParent1[i];
            }
        }

        return (child1, child2);
    }

    /* En este tipo de cruce se seleccionan uno a uno cada gen y si en ambos padres 
    coinciden los hijos mantienen el material genetico, en el caso que no coincidan
    se toma de forma aleatoria con una probabilidad de 50% de obtener los genes de
    un padre o del otro. */
    private ((int, int, int, int)[], (int, int, int, int)[]) BestFitnessCrossover((int, int, int, int)[] parent1, (int, int, int, int)[] parent2)
    {
        int chromosomeLength = parent1.Length;

        (int, int, int, int)[] genesParent1 = parent1;
        (int, int, int, int)[] genesParent2 = parent2;
        (int, int, int, int)[] child1 = new (int, int, int, int)[chromosomeLength];
        (int, int, int, int)[] child2 = new (int, int, int, int)[chromosomeLength];

        for (int i = 0; i < chromosomeLength; i++)
        {
            if (genesParent1[i] == genesParent2[i])
            {
                child1[i] = genesParent1[i];
                child2[i] = genesParent1[i];
            }
            else
            {
                child1[i] = (Random.Range(0f, 1f) < 0.5) ? genesParent1[i] : genesParent2[i];
                child2[i] = (Random.Range(0f, 1f) < 0.5) ? genesParent1[i] : genesParent2[i];
            }
        }

        return (child1, child2);
    }
}