using System;
using System.Collections.Generic;
using Schedule_Movement.Scripts.Environment.InteractionRooms;
using Schedule_Movement.Scripts.Npc.Agents;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Schedule_Movement.Scripts.Environment.ScheduleManagers
{
    public class CreatorsEmployeeSchedule : NpcScheduleManager
    {
        [Space(10)]
        [SerializeField] private FreeTimePointsQueue freeTimePoints;

        [SerializeField] private CreatorsInteractionRoom creatingRoom;
        [SerializeField] private UsingMaterialsRoom[] usingRooms;

        [ShowInInspector, ReadOnly] private List<Employee> _employees = new();

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
            employee.OnFree += () =>
            {
                AddEmployeeToCurrentRoom(employee);
            };
            AddEmployeeToCurrentRoom(employee);

            employee.OnDestroyed += _ =>
            {
                _employees.Remove(_ as Employee);
            };
        }

        protected override void StartPeriod(TimePeriod timePeriod)
        {
            base.StartPeriod(timePeriod);

            if (timePeriod.PeriodType is PeriodType.Study or PeriodType.HavingFood)
            {
                return;
            }
            
            freeTimePoints.ClearQueue();
            foreach (var employee in _employees)
            {
                if (employee.AgentState == AgentState.FreeTime)
                {
                    AddEmployeeToCurrentRoom(employee);
                }
            }
        }

        protected override void FinishPeriod(TimePeriod timePeriod)
        {
            base.FinishPeriod(timePeriod);
            switch (timePeriod.PeriodType)
            {
                case PeriodType.DeliveryMaterials:
                    creatingRoom.ClearMaterialsCount();
                    break;
                case PeriodType.Study or PeriodType.HavingFood:
                    FinishUsingPeriod();
                    break;
            }
        }

        private void FinishUsingPeriod()
        {
            foreach (var employee in _employees)
            {
                if (employee.AgentState == AgentState.FreeTime)
                {
                    AddEmployeeToCurrentRoom(employee);
                }
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
                    DeliveryToTargetRoom(employee);
                    break;
                case PeriodType.Study:
                case PeriodType.HavingFood:
                    SendEmployeesToTargetRoom(employee);
                    break;
                case PeriodType.ChillRoom:
                case PeriodType.Park:
                case PeriodType.Sleep:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DeliveryToTargetRoom(Employee employee)
        {
            var roomsCount = usingRooms.Length;
            
            //Debug.Log("Send period at" + gameObject.name + " started");
            var creatingMaterialEmployee = employee as CreatingMaterialEmployee;
            var index = GetEmployeeIndex(employee);
            var targetRoom = GetEmployeesTargetRoom(index);

            if (roomsCount > index)
            {
                var materialsToDelivery = creatingRoom.CreatedItems / roomsCount +
                                          Convert.ToInt32(creatingRoom.CreatedItems % roomsCount > index);
                
                if (materialsToDelivery > 0)
                {
                    /*employee.UpdateMaterialCharacteristics(targetRoom, materialEffect, materialsToDelivery);
                    materialsPoint.SetEmployee(employee);*/
                    creatingRoom.TakeMaterials(materialsToDelivery);
                    targetRoom.AddMaterials(materialsToDelivery);
                    targetRoom.AddEmployee(employee);
                }
                else
                {
                    targetRoom.AddEmployee(employee);
                }
            }
            else
            {
                targetRoom.AddEmployee(employee);
            }
        }
        
        private void SendEmployeesToTargetRoom(Employee employee)
        {
            var index = GetEmployeeIndex(employee);
            var targetRoom = GetEmployeesTargetRoom(index);
            
            targetRoom.AddEmployee(employee);
        }


        private UsingMaterialsRoom GetEmployeesTargetRoom(int employeeIndex)
        {
            var roomsCount = usingRooms.Length;
            
            return usingRooms[employeeIndex % roomsCount];
        }

        private int GetEmployeeIndex(Object employee)
        {
            int index;
            for (index = 0; index < _employees.Count; index++)
            {
                if (employee == _employees[index])
                {
                    break;
                }
            }

            return index;
        }
    }
}