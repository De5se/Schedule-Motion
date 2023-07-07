using UnityEngine;

public class ChronosBehaviour : NPC
{
    [SerializeField] private ChronosState currentState;
    
    private PatientRoom PatientRoom { get; set; }
    
    public ChronosState CurrentState => currentState;

    public void ChangeState(ChronosState targetState)
    {
        if (targetState == currentState)
        {
            Debug.LogError($"Attempt to set the same state");
        }
        
        currentState = targetState;
    }

    public void SetPatientRoom(PatientRoom patientRoom)
    {
        PatientRoom = patientRoom;
    }

    public void SettleToRoom()
    {
        ChangeState(ChronosState.Settle);
        MoveToTheRoom();
    }
    
    private void MoveToTheRoom()
    {
        SetDestination(PatientRoom.transform.position);
        DestinationReachedAction += CameToRoom;
    }
    
    private void CameToRoom()
    {
        if (currentState == ChronosState.Settle)
        {
            PatientRoom.SavePatient(this);
        }

        ChangeState(ChronosState.Scheduled);
    }
}
