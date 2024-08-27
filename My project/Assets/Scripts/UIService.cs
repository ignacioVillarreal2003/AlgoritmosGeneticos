using UnityEngine;
using TMPro;

public class UIService : MonoBehaviour
{
    private GeneticAlgorithm geneticAlgorithm;
    public TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        geneticAlgorithm = FindAnyObjectByType<GeneticAlgorithm>();
        textMesh.text = "Generation: " + geneticAlgorithm.GetActualGeneration().ToString();
    }

    void Update()
    {
        textMesh.text = "Generation: " + geneticAlgorithm.GetActualGeneration().ToString();
    }
}
