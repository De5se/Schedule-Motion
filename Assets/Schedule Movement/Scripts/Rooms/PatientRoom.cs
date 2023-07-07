using System.Collections;
using System.Collections.Generic;
using Schedule_Movement.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

public class PatientRoom : MonoBehaviour
{
    [SerializeField] private Transform waitPoint;
    [SerializeField] private ChronosBehaviour patientPrefab;
    [SerializeField] private Transform patientsParent;
    [SerializeField] private InteractionRoom patientsInteractionRoom;
    
    [SerializeField] private TimePeriod[] periods;

    [ShowInInspector, ReadOnly] private List<ChronosBehaviour> _patients = new();
    
    public int EmptyPlaces => Mathf.Max(0, 2 - patientsParent.childCount);

    private string _saveKey;
    
    private void Start()
    {
        _saveKey = $"{gameObject.name}/{transform.parent.name}/patientsCount";
        Load();
        GameTimeManager.Instance.OnTimeChanged += TryUpdatePeriod;
    }

    private void TryUpdatePeriod()
    {
        FinishCurrentPeriods();
        StartCurrentPeriods();
    }

    private void FinishCurrentPeriods()
    {
        foreach (var period in periods)
        {
            if (GameTimeManager.IsTimeEqual(GameTimeManager.Instance.GameTime, period.FinishTime))
            {
                RemovePatientsFromRoom(period.InteractionRoom);
            }
        }
    }
    
    private void StartCurrentPeriods()
    {
        // remove patients from home room
        foreach (var period in periods)
        {
            if (GameTimeManager.IsTimeEqual(GameTimeManager.Instance.GameTime, period.StartTime))
            {
                AddPatientsToRoom(period.InteractionRoom);
            }
        }
    }

    private void AddPatientsToRoom(InteractionRoom interactionRoom)
    {
        foreach (var patient in _patients)
        {
            //interactionRoom.AddPatient(patient);
        }
    }

    private void RemovePatientsFromRoom(InteractionRoom interactionRoom)
    {
        foreach (var patient in _patients)
        {
            //interactionRoom.RemovePatient(patient);
        }
    }


    private void CreateStartPatients(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateStartPatient();
        }
    }

    private void CreateStartPatient()
    {
        var patient = CreatePatient(waitPoint.position);
        _patients.Add(patient);
        SetPatientToInteractionRoom(patient);
    }

    public ChronosBehaviour CreatePatient(Vector3 startPosition)
    {
        var patient = Instantiate(patientPrefab, startPosition, Quaternion.identity, patientsParent);
        patient.SetPatientRoom(this);
        return patient;
    }

    public void SavePatient(ChronosBehaviour chronos)
    {
        _patients.Add(chronos);
        SetPatientToInteractionRoom(chronos);
        Save();
    }

    private void SetPatientToInteractionRoom(ChronosBehaviour chronos)
    {
        patientsInteractionRoom.AddPatient(chronos);
    }
    
    private void Save()
    {
        PlayerPrefs.SetInt(_saveKey, _patients.Count);
    }
    
    private void Load()
    {
        var startPatientsCount = PlayerPrefs.GetInt(_saveKey);
        CreateStartPatients(startPatientsCount);
    }
}
