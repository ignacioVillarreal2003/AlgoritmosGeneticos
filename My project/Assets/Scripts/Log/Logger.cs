using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class Logger : MonoBehaviour
{
    private GeneticController geneticController;
    private TimeUI timeUI;
    private string logFilePath;

    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
        timeUI = FindAnyObjectByType<TimeUI>();
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        logFilePath = Path.Combine(desktopPath, "GeneticAlgorithmLog.txt");
    }

    public void InitialData()
    {
        WriteLog("--------------------------------------------------------------------------------");
        WriteLog("---- INICIO DE LA PRUEBA ----");
        WriteLog("Prueba de algoritmos genéticos con el juego más difícil del mundo.");
        WriteLog($"Fecha y hora de inicio: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        WriteLog("");
        WriteLog("---- DATOS INICIALES ----");
        WriteLog($"Tamaño de la población: {geneticController.GetPopulationSize()} individuos.");
        WriteLog($"Cantidad de cromosomas por individuo: {geneticController.GetChromosomeLength()}.");
        WriteLog($"Velocidad: {geneticController.GetVelocity()} unidades.");
        WriteLog($"Intervalo de movimiento: {geneticController.GetMoveCooldown()} segundos.");
        WriteLog($"Penalización aplicada al fitness: {geneticController.GetPenality()}.");
        WriteLog("");
        WriteLog("---- CONFIGURACIÓN DE ALGORITMOS ----");
        WriteLog($"  - Selección: {geneticController.GetSelectionOptions()}.");
        WriteLog($"  - Cruce: {geneticController.GetCrossesOptions()}.");
        WriteLog($"  - Mutación: {geneticController.GetMutationsOptions()}.");
        WriteLog("");
    }

    public void FinalData()
    {
        LogGenerationStats();
        WriteLog("---- FIN DE LA PRUEBA ----");
        WriteLog($"Fecha y hora de finalización: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        WriteLog("");
        WriteLog("---- RESULTADOS FINALES ----");
        WriteLog($"Tiempo total transcurrido: {timeUI.GetTotalTime()} segundos.");
        WriteLog($"Cantidad total de generaciones: {geneticController.GetCurrentGeneration()}.");
        WriteLog("--------------------------------------------------------------------------------");
        WriteLog("");
    }

    public void CandelData()
    {
        WriteLog("---- FIN DE LA PRUEBA ----");
        WriteLog($"Fecha y hora de finalización: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        WriteLog("Se ha cancelado la prueba.");
        WriteLog("--------------------------------------------------------------------------------");
        WriteLog("");
    }

    public void LogGenerationStats()
    {
        float minDistance = geneticController.GetPopulation().Min(p => p.GetDistanceToTarget());
        float maxDistance = geneticController.GetPopulation().Max(p => p.GetDistanceToTarget());
        float avgDistance = geneticController.GetPopulation().Average(p => p.GetDistanceToTarget());

        WriteLog($"Generación: {geneticController.GetCurrentGeneration()}");
        WriteLog($"Distancia mínima: {minDistance:F2}");
        WriteLog($"Distancia máxima: {maxDistance:F2}");
        WriteLog($"Distancia promedio: {avgDistance:F2}");
        WriteLog("");
    }

    private void WriteLog(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(message);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al escribir en el log: {ex.Message}");
        }
    }
}
