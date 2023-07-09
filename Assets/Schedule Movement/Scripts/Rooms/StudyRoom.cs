using System;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class StudyRoom : UsingMaterialsRoom
    {
        [Space(10)] 
        [SerializeField] private NpcQueue juiceQueue;
        [SerializeField] private NpcQueue workQueue;
        [Space(10)]
        [SerializeField] private InteractionItem[] tables;
        [SerializeField] private InteractionItem juice;
        [SerializeField] private InteractionItem[] workTherapyItems;
        
        
        public override void AddPatient(ChronosBehaviour patient)
        {
            Debug.Log($"Patient interaction cycle {patient.CurrentRoomInteraction}");
            switch (patient.CurrentRoomInteraction)
            {
                case 0:
                    // set to queue
                    AddChronosToLearn(patient);
                    break;
                case 1:
                    AddChronosToJuiceQueue(patient);
                    break;
                case 2:
                    AddChronosToWork(patient);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected override void TryMovePatients()
        {
            TryMoveLearning();
            TryMoveJuice();
            TryMoveWork();
        }

        private void TryMoveLearning()
        {
            if (patientQueue.IsEmpty)
            {
                return;
            }
        
            foreach (var table in tables)
            {
                if (table.HasPlace(InteractionType.Chronos))
                {
                    table.SetChronos(patientQueue.GetNpc() as ChronosBehaviour);
                    return;
                }
            }
        }

        private void TryMoveJuice()
        {
            if (juiceQueue.IsEmpty)
            {
                return;
            }
            
            if (juice.HasPlace(InteractionType.Chronos))
            {
                juice.SetChronos(juiceQueue.GetNpc() as ChronosBehaviour);
            }
        }

        private void TryMoveWork()
        {
            if (workQueue.IsEmpty)
            {
                return;
            }
        
            foreach (var workItem in workTherapyItems)
            {
                if (workItem.HasPlace(InteractionType.Chronos))
                {
                    workItem.SetChronos(workQueue.GetNpc() as ChronosBehaviour);
                    return;
                }
            }
        }
        
        private void AddChronosToLearn(ChronosBehaviour chronos)
        {
            if (MaterialsLeft <= 0)
            {
                chronos.CurrentRoomInteraction++;
                AddChronosToJuiceQueue(chronos);
                return;
            }
        
            patientQueue.AddNpc(chronos);
            TryMoveLearning();
        }
        
        private void AddChronosToJuiceQueue(ChronosBehaviour chronos)
        {
            juiceQueue.AddNpc(chronos);
            TryMoveJuice();
        }
        
        private void AddChronosToWork(ChronosBehaviour chronos)
        {
            workQueue.AddNpc(chronos);
            TryMoveWork();
        }
    }
}