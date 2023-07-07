using System;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
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