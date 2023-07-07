using System;
using System.Collections;
using System.Collections.Generic;
using Schedule_Movement.Scripts.Rooms;
using UnityEngine;

public class PortalItem : MonoBehaviour
{
    #region Variables
    [SerializeField] private Vector2 timeBetweenTeleportation;
    [SerializeField] private float timeBetweenPatientsCreation;
    [SerializeField] private int maxPatientsInWave;
    [SerializeField] private Transform creationPoint;

    [Space(10)] 
    [SerializeField] private InteractionRoom receptionRoom;
        
    private WaitForSeconds _waitBetweenTeleportation;
    private WaitForSeconds _waitBetweenPatientsCreation;

    [SerializeField] private RoomsStatistics roomsStatistics;
    #endregion
    
    void Start()
    {
        _waitBetweenTeleportation = new WaitForSeconds(timeBetweenTeleportation.x * 60 + timeBetweenTeleportation.y);
        _waitBetweenPatientsCreation = new WaitForSeconds(timeBetweenPatientsCreation);
        
        StartCoroutine(CreationOfPatientsWaves());
    }


    private IEnumerator CreationOfPatientsWaves()
    {
        yield return new WaitForEndOfFrame();
            
        while (true)
        {
            StartCoroutine(CreateNewPatients());
            Debug.Log("Patients created");
            yield return _waitBetweenTeleportation; 
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private IEnumerator CreateNewPatients()
    {
        var createdPatients = 0;
        foreach (var sector in roomsStatistics.PatientsSectors)
        {
            foreach (var currentRoom in sector.PatientRooms)
            {
                var emptyPlaces = currentRoom.EmptyPlaces;
                for (var j = 0; j < emptyPlaces; j++)
                {
                    var patient = currentRoom.CreatePatient(creationPoint.position);
                    patient.ChangeState(ChronosState.Reception);
                    receptionRoom.AddPatient(patient);
                    yield return _waitBetweenPatientsCreation;
                    if (++createdPatients >= maxPatientsInWave){yield break;}
                }
            }
        }
    }
}
