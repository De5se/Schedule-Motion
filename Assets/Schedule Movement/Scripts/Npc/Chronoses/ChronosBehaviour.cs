using Schedule_Movement.Scripts;
using Schedule_Movement.Scripts.Environment.Items;
using Schedule_Movement.Scripts.Environment.NpcRooms;
using Schedule_Movement.Scripts.Npc.Fighting;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChronosBehaviour : NPC, IFightingUnit
{
    [ShowInInspector, ReadOnly] 
    public ChronosState CurrentState { get; private set; }
    [ShowInInspector, ReadOnly] 
    public int CurrentRoomInteraction { get; set; }
    
    public Transform Transform => transform;
    
    private PatientRoom PatientRoom { get; set; }

    private void ChangeState(ChronosState targetState)
    {
        if (targetState == CurrentState)
        {
            Debug.LogError($"Attempt to set the same state");
        }
        
        CurrentState = targetState;
    }

    public void SetPatientRoom(PatientRoom patientRoom)
    {
        PatientRoom = patientRoom;
    }

    public override void FinishInteraction()
    {
        var setFreeAfterFinish = CurrentInteraction.SetChronosFree;
        base.FinishInteraction();

        if (setFreeAfterFinish)
        {
            InvokeFreeAction();
        }
    }

    public void SettleToRoom()
    {
        ChangeState(ChronosState.Settle);
        MoveToTheRoom();
    }

    public void SendToRoom()
    {
        ChangeState(ChronosState.GoingHome);
        MoveToTheRoom();
    }

    public override void SetInteraction(InteractionPoint interactionPoint, Vector3 targetPosition)
    {
        base.SetInteraction(interactionPoint, targetPosition);
        ChangeState(ChronosState.Scheduled);
    }

    protected override void InvokeFreeAction()
    {
        ChangeState(ChronosState.FreeTime);
        base.InvokeFreeAction();
    }

    private void MoveToTheRoom()
    {
        SetDestination(PatientRoom.transform.position);
        DestinationReachedAction += CameToRoom;
    }
    
    private void CameToRoom()
    {
        if (CurrentState == ChronosState.Settle)
        {
            PatientRoom.SavePatient(this);
        }

        // add npc to currentRoom
        InvokeFreeAction();
    }
    
    
    public void StartFighting()
    {
        if (CurrentInteraction != null)
        {
            /////CurrentInteraction.ForceStop();
        }
        
        (this as IFightingUnit).FightingNpc.StartFighting();
    }
}
