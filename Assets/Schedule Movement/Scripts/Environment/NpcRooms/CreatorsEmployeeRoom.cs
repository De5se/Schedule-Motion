using Schedule_Movement.Scripts.Environment.ScheduleManagers;
using Schedule_Movement.Scripts.Npc.Agents;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment.NpcRooms
{
    public class CreatorsEmployeeRoom : EmployeeRoom
    {
        [Space(10)] 
        [SerializeField] private CreatorsEmployeeSchedule creatorsEmployeeSchedule;

        protected override Employee CreateEmployee(Vector3 startPosition)
        {
            var employee = base.CreateEmployee(startPosition);
            creatorsEmployeeSchedule.AddEmployee(employee);
            return employee;
        }
    }
}
