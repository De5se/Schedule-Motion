using System;
using TMPro;
using UnityEngine;

namespace Schedule_Movement.Scripts
{
    public class TimeDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private GameTimeManager gameTimeManager;

        private void Start()
        {
            gameTimeManager.OnTimeChanged += UpdateText;
        }


        private void UpdateText()
        {
            timer.text = $"{(int)gameTimeManager.GameTime.x:D2}:{(int)gameTimeManager.GameTime.y:D2}";
        }
    }
}