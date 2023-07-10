using Schedule_Movement.Scripts.Environment.Items;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment.InteractionRooms
{
    public class PatientInteractionRoom : InteractionRoom
    {
        [Space(10)] 
        [SerializeField] private InteractionItem bedItem;
        private bool _isSleeping;

        public void SetSleepPeriod(bool isSleeping)
        {
            _isSleeping = isSleeping;
        }
    
        protected override void TryMovePatients()
        {
            if (_isSleeping)
            {
                SetSleep();
                return;
            }
    
            base.TryMovePatients();
        }

        private void SetSleep()
        {
        
            return;
            while (patientQueue.IsEmpty == false)
            {
                bedItem.SetChronos(patientQueue.GetNpc() as ChronosBehaviour);
            }
        }
    }
}
