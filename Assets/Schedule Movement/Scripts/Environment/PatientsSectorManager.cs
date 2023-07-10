using Schedule_Movement.Scripts.Environment.NpcRooms;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment
{
    public class PatientsSectorManager : MonoBehaviour
    {
        public PatientRoom[] PatientRooms { get; private set; }
        
        private void Awake()
        {
            PatientRooms = GetComponentsInChildren<PatientRoom>();
        }
    }
}