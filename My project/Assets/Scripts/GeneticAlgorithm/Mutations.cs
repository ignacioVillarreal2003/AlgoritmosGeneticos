using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutations : MonoBehaviour
{
    /* En esta mutación, cada gen del individuo tiene una probabilidad p 
    de ser reemplazado por un valor aleatorio dentro de su dominio de 
    posibles valores. */
    public void UniformMutation(PlayerController adn)
    {
        for (int i = 0; i < adn.chromosome.Length; i++)
        {
            if (Random.Range(0f, 1f) < 0.05)
            {
                int randomNumber = Random.Range(0, 4);
                if (randomNumber == 0) {
                    adn.chromosome[i] = (1, 0, 0, 0);
                } else if (randomNumber == 1) {
                    adn.chromosome[i] = (0, 1, 0, 0);
                } else if (randomNumber == 2) {
                    adn.chromosome[i] = (0, 0, 1, 0);
                } else {
                    adn.chromosome[i] = (0, 0, 0, 1);
                }
            }
        }
    }

    /* Modificacion de mutacion uniforme, si la población converge 
    rápidamente y las soluciones no mejoran, se aumenta la tasa 
    de mutación para intentar escapar de óptimos locales. Por el 
    contrario, si la población aún está explorando el espacio de 
    soluciones, se mantiene una tasa de mutación baja. */

    [SerializeField] private float mutationRateAdaptative = 0.02f;

    public void AdaptativeMutation(PlayerController adn)
    {
        for (int i = 0; i < adn.chromosome.Length; i++)
        {
            if (Random.Range(0f, 1f) < mutationRateAdaptative)
            {
                int randomNumber = Random.Range(0, 4);
                if (randomNumber == 0) {
                    adn.chromosome[i] = (1, 0, 0, 0);
                } else if (randomNumber == 1) {
                    adn.chromosome[i] = (0, 1, 0, 0);
                } else if (randomNumber == 2) {
                    adn.chromosome[i] = (0, 0, 1, 0);
                } else {
                    adn.chromosome[i] = (0, 0, 0, 1);
                }
            }
        }
    }

    public void ChangeMutationRateAdaptative(bool isBetter)
    {
        if (isBetter)
        {
            mutationRateAdaptative = System.Math.Clamp(mutationRateAdaptative * 0.9f, 0.01f, 0.1f);
        }
        else 
        {
            mutationRateAdaptative = System.Math.Clamp(mutationRateAdaptative * 1.1f, 0.01f, 0.1f);
        }
    }

    /* Se seleccionan dos posiciones aleatorias en el individuo y se 
    intercambian los valores de esas posiciones. */
    public void Swap1Mutation(PlayerController individual)
    {
        (int, int, int, int)[] genes = individual.chromosome;

        int pivot1 = Random.Range(0, genes.Length);
        int pivot2 = Random.Range(0, genes.Length);

        (int, int, int, int) aux = genes[pivot1];
        genes[pivot1] = genes[pivot2];
        genes[pivot2] = aux;
    }

    public void Swap2Mutation(PlayerController individual)
    {
        int position = 0;
        (int, int, int, int)[] genes = individual.chromosome;

        while (position < genes.Length)
        {
            int pivot1 = Random.Range(position, genes.Length);
            int pivot2 = Random.Range(position, genes.Length);

            (int, int, int, int) aux = individual.chromosome[pivot1];
            individual.chromosome[pivot1] = individual.chromosome[pivot2];
            individual.chromosome[pivot2] = aux;

            position = System.Math.Min(pivot1, pivot2);
        }
    }
    
    /* En la mutación de inserción, un gen es seleccionado, se elimina 
    de su posición actual y se inserta en otra posición. El resto de 
    los genes se mueven en consecuencia. */
    public void InsertionMutation(PlayerController individual)
    {
        (int, int, int, int)[] genes = individual.chromosome;
        
        int geneToMove = Random.Range(0, genes.Length);
        int newPosition = Random.Range(0, genes.Length);
        
        (int, int, int, int) gene = genes[geneToMove];
        
        if (newPosition < geneToMove)
        {
            for (int i = geneToMove; i > newPosition; i--)
            {
                genes[i] = genes[i - 1];
            }
        }
        else if (newPosition > geneToMove)
        {
            for (int i = geneToMove; i < newPosition; i++)
            {
                genes[i] = genes[i + 1];
            }
        }
        
        genes[newPosition] = gene;
    }

    /* La mutación de inversión selecciona dos puntos en el 
    cromosoma y revierte el orden de los genes entre esos dos puntos. */
    public void InversionMutation(PlayerController individual)
    {
        (int, int, int, int)[] genes = individual.chromosome;
        
        int pivot1 = Random.Range(0, genes.Length);
        int pivot2 = Random.Range(0, genes.Length);
        
        if (pivot1 > pivot2)
        {
            int temp = pivot1;
            pivot1 = pivot2;
            pivot2 = temp;
        }

        while (pivot1 < pivot2)
        {
            (int, int, int, int) aux = genes[pivot1];
            genes[pivot1] = genes[pivot2];
            genes[pivot2] = aux;
            pivot1++;
            pivot2--;
        }
    }

    /* En la mutación de desplazamiento, se selecciona un segmento del 
    cromosoma y se mueve de manera aleatoria dentro del mismo. */
    public void ShiftMutation(PlayerController individual)
    {
        (int, int, int, int)[] genes = individual.chromosome;

        int start = Random.Range(0, genes.Length);
        int end = Random.Range(0, genes.Length);

        if (start > end)
        {
            int temp = start;
            start = end;
            end = temp;
        }

        (int, int, int, int)[] segment = new (int, int, int, int)[end - start + 1];
        for (int i = start; i <= end; i++)
        {
            segment[i - start] = genes[i];
        }

        int newPosition = Random.Range(0, genes.Length - segment.Length + 1);

        if (newPosition < start)
        {
            for (int i = start; i < genes.Length - segment.Length; i++)
            {
                genes[end - (i - start)] = genes[i];
            }
        }
        else if (newPosition > end)
        {
            for (int i = end; i < newPosition; i++)
            {
                genes[i] = genes[i + 1];
            }
        }

        for (int i = 0; i < segment.Length; i++)
        {
            genes[newPosition + i] = segment[i];
        }
    }
}
