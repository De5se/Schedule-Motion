using System.Collections.Generic;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment
{
    public class RoomsStatistics : MonoBehaviour
    {
        [SerializeField] public PatientsSectorManager[] patientsSectors;

        public IEnumerable<PatientsSectorManager> PatientsSectors => patientsSectors;
    }
}