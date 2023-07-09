using System;
using System.Collections.Generic;
using Schedule_Movement.Scripts.Npc.Agents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class CreatorsEmployeeSchedule : NpcScheduleManager
    {
        [Space(10)]
        [SerializeField] private FreeTimePointsQueue freeTimePoints;

        [SerializeField] private CreatorsInteractionRoom creatingRoom;
        [SerializeField] private InteractionRoom[] usingRooms;

        [ShowInInspector, ReadOnly] private List<Employee> _employees = new();

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
            employee.OnFree += () =>
            {
                AddEmployeeToCurrentRoom(employee);
            };
            AddEmployeeToCurrentRoom(employee);
        }

        protected override void StartPeriod(TimePeriod timePeriod)
        {
            base.StartPeriod(timePeriod);

            freeTimePoints.ClearQueue();
            foreach (var employee in _employees)
            {
                AddEmployeeToCurrentRoom(employee);
            }
        }

        private void AddEmployeeToCurrentRoom(Employee employee)
        {
            if (CurrentPeriod == null)
            {
                // if no period - free time
                freeTimePoints.AddNpc(employee);
                return;
            }

            switch (CurrentPeriod.PeriodType)
            {
                case PeriodType.CreatingMaterials:
                    creatingRoom.AddEmployee(employee);
                    break;
                case PeriodType.DeliveryMaterials:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}