using Schedule_Movement.Scripts.Npc.Agents;

namespace Schedule_Movement.Scripts.Rooms.Items
{
    public class ReceptionInteraction : InteractionPoint
    {
        protected override void FinishInteraction()
        {
            base.FinishInteraction();
            
            MoveNpcToTheRoom();
        }
        
        private void MoveNpcToTheRoom()
        {
            LastPatient.SettleToRoom();
            ((AgentBehaviour) LastEmployee).UpdatePatient(LastPatient);
        }
    }
}