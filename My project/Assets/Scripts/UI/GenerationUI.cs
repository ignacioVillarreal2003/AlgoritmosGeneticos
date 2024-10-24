using UnityEngine;
using TMPro;

public class GenerationUI : MonoBehaviour
{
    private GeneticController geneticController;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        geneticController = FindAnyObjectByType<GeneticController>();
    }

    void Update()
    {
        textMesh.text = "Generation: " + geneticController.GetCurrentGeneration().ToString();
    }
}
