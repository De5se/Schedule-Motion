using System.Collections.Generic;
using Schedule_Movement.Scripts.Npc.Agents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment.NpcRooms
{
    public class EmployeeRoom : MonoBehaviour
    {
        [SerializeField] private Employee employeePrefab;
        [SerializeField] private int startCount = 2;
        [SerializeField] private Transform employeesParent;
        
        private string _employeeCountKey;

        private readonly List<Employee> _employees = new();
        
        protected virtual void Start()
        {
            _employeeCountKey = $"{gameObject.name}/{transform.parent.name}/employeesCount";

            Load();
        }

        [Button("Hire Employee")]
        protected virtual void HireEmployee()
        {
            CreateEmployee(employeesParent.position);
            Save();
        }
        

        private void CreateStartEmployees(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateEmployee(employeesParent.position);
            }
        }

        protected virtual Employee CreateEmployee(Vector3 startPosition)
        {
            var employee = Instantiate(employeePrefab, startPosition, Quaternion.identity, employeesParent);
            _employees.Add(employee);
            return employee;
        }

        protected virtual void Save()
        {
            PlayerPrefs.SetInt(_employeeCountKey, employeesParent.childCount);
        }

        protected virtual void Load()
        {
            var startEmployeeCount = PlayerPrefs.GetInt(_employeeCountKey, startCount);
            CreateStartEmployees(startEmployeeCount);
        }
    }
}