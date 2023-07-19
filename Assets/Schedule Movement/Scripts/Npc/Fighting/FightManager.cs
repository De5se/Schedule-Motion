using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Schedule_Movement.Scripts.Environment.InteractionRooms;
using Schedule_Movement.Scripts.Environment.NpcRooms;
using Schedule_Movement.Scripts.Npc.Agents;
using Schedule_Movement.Scripts.Npc.Fighting;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private AgentsRoom _agentRoom;
    [SerializeField] private InteractionRoom _prison;

    private IFightingUnit[] _agents;
    private IFightingUnit[] _patients;

    private bool _isFighting;
    #endregion
    
    private void InitAgents()
    {
        var agents = _agentRoom.GetFightingEmployees().ToArray();
        _agents = new IFightingUnit[agents.Length];
        //_agents = (AgentEmployee[]) agentRoom.Employees.ToArray();
        for (var i = 0; i < _agents.Length; i++)
        {
            _agents[i] = (AgentBehaviour) agents[i];
        }
    }
    
    public void StartFight(IEnumerable<ChronosBehaviour> patients)
    {
        if (_isFighting)
        {
            return;
        }

        // Init patients
        _patients = patients as IFightingUnit[];
        InitAgents();
        
        foreach (var patient in _patients)
            patient.StartFighting();
        foreach (var agent in _agents)
            agent.StartFighting();

        _isFighting = true;
    }

    public FightingNpc GetNearestEnemy(FightingNpc currentNpc)
    {
        var units = currentNpc.GetComponent<Employee>() ? _patients : _agents;
        var minDistance = 1e9f;
        FightingNpc targetAgent = null;
        
        foreach (var enemy in units)
        {
            if (!enemy.IsAlive)
            {
                continue;
            }
            var currentDistance = Vector3.Distance(currentNpc.transform.position, enemy.Transform.position);
            if (currentDistance >= minDistance) continue;
            minDistance = currentDistance;
            targetAgent = enemy.FightingNpc;
        }
        return targetAgent;
    }

    public void TryFinishFight()
    {
        if (!_isFighting)
        {
            return;
        }
        if (!IsAnyPatientAlive())
        {
            FinishFight(true);
            return;
        }
        if (!IsAnyAgentAlive())
        {
            FinishFight(false);
        }
    }

    private void FinishFight(bool isWin)
    {
        var firedAgents = FireDeadAgents();
        RemovePatients();
    }

    private int FireDeadAgents()
    {
        var firedAgents = 0;
        foreach (var agent in _agents)
        {
            if (!agent.IsAlive)
            {
                // ToDo fire
                firedAgents++;
            }
            else
            {
                agent.FightingNpc.FinishFighting();
            }
        }

        return firedAgents;
    }

    private int RemovePatients()
    {
        var removedPatients = 0;
        foreach (var patient in _patients)
        {
            if (!patient.IsAlive)
            {
                _prison.AddPatient(patient as ChronosBehaviour);
                patient.FightingNpc.FinishFighting();
            }
            else
            {
                removedPatients++;
                // ToDo remove
            }
        }

        return removedPatients;
    }
    
    private bool IsAnyPatientAlive() => _patients.Any(t => t.IsAlive);
    private bool IsAnyAgentAlive() => _agents.Any(t => t.IsAlive);
}
