﻿using UnityEngine;

namespace Schedule_Movement.Scripts
{
    [System.Serializable]
    public class TimePeriod
    {
        [SerializeField] private Vector2 startTime;
        [SerializeField] private Vector2 finishTime;
        [SerializeField] private InteractionRoom interactionRoom;

        public Vector2 StartTime => startTime;
        public Vector2 FinishTime => finishTime;
        public InteractionRoom InteractionRoom => interactionRoom;
    }
}