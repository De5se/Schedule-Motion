using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts
{
    public class NpcScheduleManager : MonoBehaviour
    {
        [SerializeField] private GameTimeManager gameTimeManager;
        [SerializeField]
        private List<TimePeriod> periods;

        [ShowInInspector, ReadOnly]
        protected TimePeriod CurrentPeriod;


        protected void Awake()
        {
            SetPeriodOnStart();
        }

        protected virtual void Start()
        {
            gameTimeManager.OnTimeChanged += TryUpdatePeriod;
        }

        private void SetPeriodOnStart()
        {
            foreach (var period in periods)
            {
                if (gameTimeManager.IsInsidePeriod(period))
                {
                    CurrentPeriod = period;
                    break;
                }
            }
        }
        
        private void TryUpdatePeriod()
        {
            FinishCurrentPeriods();
            StartCurrentPeriods();
        }
        
        protected virtual void StartPeriod(TimePeriod timePeriod)
        {
            CurrentPeriod = timePeriod;
        }
        
        protected virtual void FinishPeriod(TimePeriod timePeriod)
        {
            if (CurrentPeriod == timePeriod)
            {
                CurrentPeriod = null;
            }
            
            //RemovePatientsFromRoom(period.InteractionRoom);
        }

        private void FinishCurrentPeriods()
        {
            foreach (var period in periods)
            {
                if (GameTimeManager.IsTimeEqual(GameTimeManager.Instance.GameTime, period.FinishTime))
                {
                    FinishPeriod(period);
                }
            }
        }

        private void StartCurrentPeriods()
        {
            foreach (var period in periods)
            {
                if (GameTimeManager.IsTimeEqual(GameTimeManager.Instance.GameTime, period.StartTime))
                {
                    StartPeriod(period);
                }
            }
        }
    }
}