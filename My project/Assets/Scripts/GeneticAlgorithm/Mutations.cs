using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutations : MonoBehaviour
{
    public (int, int, int, int)[] Mutate(MutationsOptions mutationsOptions, (int, int, int, int)[] individual)
    {
        if (mutationsOptions == MutationsOptions.UniformMutation)
        {
            return UniformMutation(individual);
        }
        if (mutationsOptions == MutationsOptions.AdaptativeMutation)
        {
            return AdaptativeMutation(individual);
        }
        if (mutationsOptions == MutationsOptions.Swap1Mutation)
        {
            return Swap1Mutation(individual);
        }
        if (mutationsOptions == MutationsOptions.Swap2Mutation)
        {
            return Swap2Mutation(individual);
        }
        if (mutationsOptions == MutationsOptions.ShiftMutation)
        {
            return ShiftMutation(individual);
        }
        if (mutationsOptions == MutationsOptions.InversionMutation)
        {
            return InversionMutation(individual);
        }
        if (mutationsOptions == MutationsOptions.InsertionMutation)
        {
            return InsertionMutation(individual);
        }
        return individual;
    }

    public enum MutationsOptions
    {
        UniformMutation,
        AdaptativeMutation,
        Swap1Mutation,
        Swap2Mutation,
        ShiftMutation,
        InversionMutation,
        InsertionMutation
    }

    /* En esta mutación, cada gen del individuo tiene una probabilidad p 
    de ser reemplazado por un valor aleatorio dentro de su dominio de 
    posibles valores. */
    public (int, int, int, int)[] UniformMutation((int, int, int, int)[] chromosome)
    {
        for (int i = 0; i < chromosome.Length; i++)
        {
            if (Random.Range(0f, 1f) < 0.05)
            {
                int randomNumber = Random.Range(0, 4);
                if (randomNumber == 0) {
                    chromosome[i] = (1, 0, 0, 0);
                } else if (randomNumber == 1) {
                    chromosome[i] = (0, 1, 0, 0);
                } else if (randomNumber == 2) {
                    chromosome[i] = (0, 0, 1, 0);
                } else {
                    chromosome[i] = (0, 0, 0, 1);
                }
            }
        }

        return chromosome;
    }

    /* Modificacion de mutacion uniforme, si la población converge 
    rápidamente y las soluciones no mejoran, se aumenta la tasa 
    de mutación para intentar escapar de óptimos locales. Por el 
    contrario, si la población aún está explorando el espacio de 
    soluciones, se mantiene una tasa de mutación baja. */

    [SerializeField] private float mutationRateAdaptative = 0.02f;

    public (int, int, int, int)[] AdaptativeMutation((int, int, int, int)[] chromosome)
    {
        for (int i = 0; i < chromosome.Length; i++)
        {
            if (Random.Range(0f, 1f) < mutationRateAdaptative)
            {
                int randomNumber = Random.Range(0, 4);
                if (randomNumber == 0) {
                    chromosome[i] = (1, 0, 0, 0);
                } else if (randomNumber == 1) {
                    chromosome[i] = (0, 1, 0, 0);
                } else if (randomNumber == 2) {
                    chromosome[i] = (0, 0, 1, 0);
                } else {
                    chromosome[i] = (0, 0, 0, 1);
                }
            }
        }

        return chromosome;
    }

    public void ChangeMutationRateAdaptative(bool isBetter)
    {
        if (isBetter)
        {
            mutationRateAdaptative = System.Math.Clamp(mutationRateAdaptative * 0.9f, 0.02f, 0.1f);
        }
        else 
        {
            mutationRateAdaptative = System.Math.Clamp(mutationRateAdaptative * 1.1f, 0.02f, 0.1f);
        }
    }

    /* Se seleccionan dos posiciones aleatorias en el individuo y se 
    intercambian los valores de esas posiciones. */
    public (int, int, int, int)[] Swap1Mutation((int, int, int, int)[] chromosome)
    {
        int pivot1 = Random.Range(0, chromosome.Length);
        int pivot2 = Random.Range(0, chromosome.Length);

        (int, int, int, int) aux = chromosome[pivot1];
        chromosome[pivot1] = chromosome[pivot2];
        chromosome[pivot2] = aux;

        return chromosome;
    }

    public (int, int, int, int)[] Swap2Mutation((int, int, int, int)[] chromosome)
    {
        int position = 0;

        while (position < chromosome.Length)
        {
            int pivot1 = Random.Range(position, chromosome.Length);
            int pivot2 = Random.Range(position, chromosome.Length);

            (int, int, int, int) aux = chromosome[pivot1];
            chromosome[pivot1] = chromosome[pivot2];
            chromosome[pivot2] = aux;

            position = System.Math.Min(pivot1, pivot2);
        }

        return chromosome;
    }
    
    /* En la mutación de inserción, un gen es seleccionado, se elimina 
    de su posición actual y se inserta en otra posición. El resto de 
    los genes se mueven en consecuencia. */
    public (int, int, int, int)[] InsertionMutation((int, int, int, int)[] chromosome)
    {        
        int geneToMove = Random.Range(0, chromosome.Length);
        int newPosition = Random.Range(0, chromosome.Length);
        
        (int, int, int, int) gene = chromosome[geneToMove];
        
        if (newPosition < geneToMove)
        {
            for (int i = geneToMove; i > newPosition; i--)
            {
                chromosome[i] = chromosome[i - 1];
            }
        }
        else if (newPosition > geneToMove)
        {
            for (int i = geneToMove; i < newPosition; i++)
            {
                chromosome[i] = chromosome[i + 1];
            }
        }
        
        chromosome[newPosition] = gene;

        return chromosome;
    }

    /* La mutación de inversión selecciona dos puntos en el 
    cromosoma y revierte el orden de los genes entre esos dos puntos. */
    public (int, int, int, int)[] InversionMutation((int, int, int, int)[] chromosome)
    {        
        int pivot1 = Random.Range(0, chromosome.Length);
        int pivot2 = Random.Range(0, chromosome.Length);
        
        if (pivot1 > pivot2)
        {
            int temp = pivot1;
            pivot1 = pivot2;
            pivot2 = temp;
        }

        while (pivot1 < pivot2)
        {
            (int, int, int, int) aux = chromosome[pivot1];
            chromosome[pivot1] = chromosome[pivot2];
            chromosome[pivot2] = aux;
            pivot1++;
            pivot2--;
        }

        return chromosome;
    }

    /* En la mutación de desplazamiento, se selecciona un segmento del 
    cromosoma y se mueve de manera aleatoria dentro del mismo. */
    public (int, int, int, int)[] ShiftMutation((int, int, int, int)[] chromosome)
    {
        int start = Random.Range(0, chromosome.Length);
        int end = Random.Range(0, chromosome.Length);

        if (start > end)
        {
            int temp = start;
            start = end;
            end = temp;
        }

        (int, int, int, int)[] segment = new (int, int, int, int)[end - start + 1];
        for (int i = start; i <= end; i++)
        {
            segment[i - start] = chromosome[i];
        }

        int newPosition = Random.Range(0, chromosome.Length - segment.Length + 1);

        if (newPosition < start)
        {
            for (int i = start; i < chromosome.Length - segment.Length; i++)
            {
                chromosome[end - (i - start)] = chromosome[i];
            }
        }
        else if (newPosition > end)
        {
            for (int i = end; i < newPosition; i++)
            {
                chromosome[i] = chromosome[i + 1];
            }
        }

        for (int i = 0; i < segment.Length; i++)
        {
            chromosome[newPosition + i] = segment[i];
        }

        return chromosome;
    }
}
