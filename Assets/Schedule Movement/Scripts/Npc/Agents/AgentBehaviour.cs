using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Npc.Agents
{
    public class AgentBehaviour : Employee
    {
        [ShowInInspector, ReadOnly]
        private ChronosBehaviour _targetChronos;
        
        
        public void UpdatePatient(ChronosBehaviour patient)
        {
            _targetChronos = patient;
            ChangeState(AgentState.Accompany);
            SetDestination(patient.NavMeshAgent.destination);
            patient.DestinationReachedAction += StopAccompany;
        }
        
        private void StopAccompany()
        {
            _targetChronos = null;
            ChangeState(AgentState.FreeTime);
        }
    }
}