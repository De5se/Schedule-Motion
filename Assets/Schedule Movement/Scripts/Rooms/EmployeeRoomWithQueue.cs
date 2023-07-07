using Schedule_Movement.Scripts.Npc.Agents;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class EmployeeRoomWithQueue : EmployeeRoom
    {
        [SerializeField] private FreeTimePointsQueue agentsQueue;

        protected override Employee CreateEmployee(Vector3 startPosition)
        {
            var employee = base.CreateEmployee(startPosition);
            employee.OnFree += () =>
            {
                AddEmployeeToQueue(employee);
            };
            AddEmployeeToQueue(employee);

            return employee;
        }

        private void AddEmployeeToQueue(NPC employee)
        {
            agentsQueue.AddNpc(employee);
        }
    }
}