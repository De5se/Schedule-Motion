using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Npc.Agents
{
    public class Employee : NPC
    {
        [ShowInInspector, ReadOnly]
        public AgentState AgentState { get; private set; }
        
        protected void ChangeState(AgentState targetState)
        {
            if (targetState == AgentState)
            {
                Debug.LogError($"Attempt to put the same state to {gameObject.name}");
            }

            AgentState = targetState;
        }

        public override void SetInteraction(InteractionPoint interactionPoint, Vector3 targetPosition)
        {
            base.SetInteraction(interactionPoint, targetPosition);
            ChangeState(AgentState.Interaction);
        }

        public override void FinishInteraction()
        {
            var setFreeAfterFinish = currentInteraction.SetEmployeeFree;
            base.FinishInteraction();

            if (setFreeAfterFinish)
            {
                InvokeFreeAction();
            }
        }

        protected override void InvokeFreeAction()
        {
            ChangeState(AgentState.FreeTime);
            base.InvokeFreeAction();
        }
    }
}