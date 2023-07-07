using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using _Scripts.SaveSystem;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private WorldTimeAPI _worldTimeAPI;

    // Update is called once per frame
    void Update()
    {
        text.text = (_worldTimeAPI.GetCurrentDateTime() - new DateTime(2023, 1, 1)).TotalSeconds.ToString();
    }
}
