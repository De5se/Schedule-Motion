using Schedule_Movement.Scripts.Npc.Agents;
using Schedule_Movement.Scripts.Rooms;
using UnityEngine;

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
