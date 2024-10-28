using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDropdown : MonoBehaviour
{
    public TMP_Dropdown myDropdown;
    public string nameDD = "";

    void Start()
    {
        LevelController levelController = FindObjectOfType<LevelController>();

        if (myDropdown != null && levelController != null)
        {
            myDropdown.onValueChanged.RemoveAllListeners();

            if (nameDD == "Selection")
            {
                myDropdown.onValueChanged.AddListener((int value) => {
                    levelController.SetSelection(value);
                });
            }
            else if (nameDD == "Mutation")
            {
                myDropdown.onValueChanged.AddListener((int value) => {
                    levelController.SetMutation(value);
                });
            }
            else if (nameDD == "Cross")
            {
                myDropdown.onValueChanged.AddListener((int value) => {
                levelController.SetCross(value);
                });
            }
            else if (nameDD == "ElitCount")
            {
                myDropdown.onValueChanged.AddListener((int value) => {
                    levelController.SetEliteCount(value);
                });
            }
            else if (nameDD == "ChromosomeLength")
            {
                myDropdown.onValueChanged.AddListener((int value) => {
                levelController.SetChromosomeLength(value);
                });
            }
            else if (nameDD == "PopulationSize")
            {
                myDropdown.onValueChanged.AddListener((int value) => {
                    levelController.SetPopulationSize(value);
                });
            }
        }
    }
}
