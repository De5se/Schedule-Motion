using System;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class TavernRoom : UsingMaterialsRoom
    {
        [Space(10)]
        [SerializeField] private InteractionItem cooler;
        [SerializeField] private InteractionItem line;
        [SerializeField] private InteractionItem[] tables;
        
        public override void AddPatient(ChronosBehaviour patient)
        {
            Debug.Log($"Patient interaction cycle {patient.CurrentRoomInteraction}");
            switch (patient.CurrentRoomInteraction)
            {
                case 0:
                    // set to queue
                    AddChronosToQueue(patient);
                    break;
                case 1:
                    AddChronosToTable(patient);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        protected override void TryMovePatients()
        {
            if (patientQueue.IsEmpty)
            {
                return;
            }

            var interactionItem = (MaterialsLeft > 0) ? line : cooler;
            if (interactionItem.HasPlace(InteractionType.Chronos))
            {
                interactionItem.SetChronos(patientQueue.GetNpc() as ChronosBehaviour);
            }
        }
        
        private void AddChronosToQueue(ChronosBehaviour chronos)
        {
            patientQueue.AddNpc(chronos);
            TryMovePatients();
        }
        
        private void AddChronosToTable(ChronosBehaviour chronos)
        {
            foreach (var table in tables)
            {
                if (table.HasPlace(InteractionType.Chronos))
                {
                    table.SetChronos(chronos);
                    return;
                }
            }

            throw new ArgumentOutOfRangeException($"There's no place for chronos");
        }
    }
}