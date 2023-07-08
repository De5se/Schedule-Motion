using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class PatientScheduleManager : NpcScheduleManager
    {
        [Space(10)]
        [SerializeField] private PatientInteractionRoom patientsInteractionRoom;
        [SerializeField] private InteractionRoom studyRoom;
        [SerializeField] private InteractionRoom tavern;
        [SerializeField] private InteractionRoom park;
        
        [ShowInInspector, ReadOnly]
        private List<ChronosBehaviour> _chronoses = new();

        public int ChronosesCount => _chronoses.Count;
        
        public void AddChronos(ChronosBehaviour chronosBehaviour)
        {
            _chronoses.Add(chronosBehaviour);
            SetPatientToCurrentInteractionRoom(chronosBehaviour);
        }

        protected override void StartPeriod(TimePeriod timePeriod)
        {
            base.StartPeriod(timePeriod);

            foreach (var chronos in _chronoses)
            {
                SetPatientToCurrentInteractionRoom(chronos);
            }
        }

        protected override void FinishPeriod(TimePeriod timePeriod)
        {
            base.FinishPeriod(timePeriod);
            
            switch (timePeriod.PeriodType)
            {
                case PeriodType.ChillRoom:
                    patientsInteractionRoom.ClearPatientQueue();
                    break;
                case PeriodType.Study:
                    studyRoom.ClearPatientQueue();
                    break;
                case PeriodType.HavingFood:
                    tavern.ClearPatientQueue();
                    break;
                case PeriodType.Park:
                    park.ClearPatientQueue();
                    break;
                case PeriodType.Sleep:
                    patientsInteractionRoom.ClearPatientQueue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void SetPatientToCurrentInteractionRoom(ChronosBehaviour chronos)
        {
            switch (CurrentPeriod.PeriodType)
            {
                case PeriodType.ChillRoom:
                    patientsInteractionRoom.AddPatient(chronos);
                    break;
                case PeriodType.Study:
                    studyRoom.AddPatient(chronos);
                    break;
                case PeriodType.HavingFood:
                    tavern.AddPatient(chronos);
                    break;
                case PeriodType.Park:
                    park.AddPatient(chronos);
                    break;
                case PeriodType.Sleep:
                    patientsInteractionRoom.AddPatient(chronos);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}