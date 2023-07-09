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
            chronosBehaviour.OnFree += () =>
            {
                SetPatientToCurrentInteractionRoom(chronosBehaviour);
            };
        }

        protected override void StartPeriod(TimePeriod timePeriod)
        {
            base.StartPeriod(timePeriod);

            if (CurrentPeriod.PeriodType == PeriodType.Sleep)
            {
                patientsInteractionRoom.SetSleepPeriod(true);
            }
            
            foreach (var chronos in _chronoses)
            {
                chronos.CurrentRoomInteraction = 0;
                if (chronos.CurrentState == ChronosState.FreeTime)
                {
                    SetPatientToCurrentInteractionRoom(chronos);
                }
            }
        }

        protected override void FinishPeriod(TimePeriod timePeriod)
        {
            base.FinishPeriod(timePeriod);
            
            patientsInteractionRoom.ClearPatientQueue();
            
            switch (timePeriod.PeriodType)
            {
                case PeriodType.ChillRoom:
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
                    patientsInteractionRoom.SetSleepPeriod(false);
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
                case PeriodType.Sleep:
                    patientsInteractionRoom.AddPatient(chronos);
                    break;
                case PeriodType.Study:
                    SendChronosToStudyRoom(chronos);
                    break;
                case PeriodType.HavingFood:
                    SendChronosToTavern(chronos);
                    break;
                case PeriodType.Park:
                    park.AddPatient(chronos);
                    break;
                case PeriodType.CreatingMaterials:
                case PeriodType.DeliveryMaterials:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            chronos.CurrentRoomInteraction++;
        }


        private void SendChronosToStudyRoom(ChronosBehaviour chronos)
        {
            switch (chronos.CurrentRoomInteraction)
            {
                case < 1:
                    studyRoom.AddPatient(chronos);
                    break;
                case 1:
                    chronos.SendToRoom();
                    break;
                default:
                    patientsInteractionRoom.AddPatient(chronos);
                    break;
            }
        }
        
        private void SendChronosToTavern(ChronosBehaviour chronos)
        {
            switch (chronos.CurrentRoomInteraction)
            {
                case < 2:
                    tavern.AddPatient(chronos);
                    break;
                case 2:
                    chronos.SendToRoom();
                    break;
                default:
                    patientsInteractionRoom.AddPatient(chronos);
                    break;
            }
        }
    }
}