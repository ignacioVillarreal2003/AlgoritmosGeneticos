using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    private GeneticController geneticController;
    private TextMeshProUGUI textMesh;
    private float totalTime = 0f;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        geneticController = FindAnyObjectByType<GeneticController>();
    }

    void Update()
    {
        totalTime += Time.deltaTime;
        float generationTime = geneticController.getGenerationTime();
        
        string generationTimeFormatted = generationTime.ToString("F2");
        string totalTimeFormatted = totalTime.ToString("F2");

        textMesh.text = "Tiempo generación: " + generationTimeFormatted + "s. Tiempo total: " + totalTimeFormatted + "s";
    }
}
