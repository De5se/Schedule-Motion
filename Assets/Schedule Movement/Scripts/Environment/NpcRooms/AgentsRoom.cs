using System.Collections.Generic;
using Schedule_Movement.Scripts.Npc.Agents;

namespace Schedule_Movement.Scripts.Environment.NpcRooms
{
    public class AgentsRoom : EmployeeRoomWithQueue
    {
        public List<Employee> GetFightingEmployees() => Employees;

    }
}