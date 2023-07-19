using Schedule_Movement.Scripts.Npc.Fighting;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Npc.Agents
{
    public class AgentBehaviour : Employee, IFightingUnit
    {
        [ShowInInspector, ReadOnly]
        private ChronosBehaviour _targetChronos;
        
        public Transform Transform => transform;
        
        public void UpdatePatient(ChronosBehaviour patient)
        {
            _targetChronos = patient;
            SetDestination(patient.NavMeshAgent.destination);
            patient.DestinationReachedAction += StopAccompany;
        }
        
        private void StopAccompany()
        {
            _targetChronos = null;
            InvokeFreeAction();
        }
        
        public void StartFighting()
        {
            if (AgentState == AgentState.Accompany)
            {
                StopAccompany();
            }
            if (AgentState == AgentState.Interaction)
            {
                //currentInteractionPoint.ForceStop();
            }
            
            ChangeState(AgentState.Fighting);
            (this as IFightingUnit).FightingNpc.StartFighting();
        }
    }
}