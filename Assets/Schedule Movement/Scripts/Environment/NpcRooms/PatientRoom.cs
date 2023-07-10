using Schedule_Movement.Scripts.Environment.ScheduleManagers;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment.NpcRooms
{
    public class PatientRoom : MonoBehaviour
    {
        [SerializeField] private PatientScheduleManager patientScheduleManager;
        [SerializeField] private Transform waitPoint;
        [SerializeField] private ChronosBehaviour patientPrefab;
        [SerializeField] private Transform patientsParent;
    
        public int EmptyPlaces => Mathf.Max(0, 2 - patientsParent.childCount);

        private string _saveKey;
    
        private void Start()
        {
            _saveKey = $"{gameObject.name}/{transform.parent.name}/patientsCount";
            Load();
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
            var chronos = CreatePatient(waitPoint.position);
            patientScheduleManager.AddChronos(chronos);
            chronos.SendToRoom();
        }

        public ChronosBehaviour CreatePatient(Vector3 startPosition)
        {
            var patient = Instantiate(patientPrefab, startPosition, Quaternion.identity, patientsParent);
            patient.SetPatientRoom(this);
            return patient;
        }

        public void SavePatient(ChronosBehaviour chronos)
        {
            patientScheduleManager.AddChronos(chronos);
            Save();
        }

    
        private void Save()
        {
            PlayerPrefs.SetInt(_saveKey, patientScheduleManager.ChronosesCount);
        }
    
        private void Load()
        {
            var startPatientsCount = PlayerPrefs.GetInt(_saveKey);
            CreateStartPatients(startPatientsCount);
        }
    }
}
