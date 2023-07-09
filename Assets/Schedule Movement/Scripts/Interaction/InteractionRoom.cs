using System.Linq;
using Schedule_Movement.Scripts;
using Schedule_Movement.Scripts.Npc.Agents;
using UnityEngine;

public class InteractionRoom : MonoBehaviour
{
    [SerializeField] private Transform itemsParent;

    [SerializeField] protected QueueBase patientQueue;
    [SerializeField] protected FreeTimePointsQueue employeeQueue;

    private InteractionItem[] _items;

    protected virtual void Awake()
    {
        _items = itemsParent.GetComponentsInChildren<InteractionItem>(true);
    }

    protected virtual void Start()
    {
        foreach (var item in _items)
        {
            item.OnFree += TryMoveNpc;
        }

        if (employeeQueue)
        {
            employeeQueue.OnNewNpc += AskForEmployee;
        }
    }

    public void AskForEmployee()
    {
        if (employeeQueue.IsEmpty)
        {
            Debug.Log($"There's no free employee for {gameObject.name} room");
            return;
        }
        
        foreach (var item in _items.Where(item => item.HasPlace(InteractionType.Agent)))
        {
            var targetEmployee = employeeQueue.GetNpc() as Employee;
            item.SetAgent(targetEmployee);
            return;
        }
        
        Debug.Log($"There's no place for agent in {gameObject.name}");
    }

    public void AddEmployee(Employee employee)
    {
        if (employeeQueue.ContainsNpc(employee))
        {
            Debug.LogError($"{employee} already in employee list");
        }
        
        employeeQueue.AddNpc(employee);
        AskForEmployee();
    }
    
    public virtual void AddPatient(ChronosBehaviour patient)
    {
        if (patientQueue.ContainsNpc(patient))
        {
            Debug.LogError($"patient {patient.name} is already in {gameObject.name} room");
        }
        
        patientQueue.AddNpc(patient);
        TryMovePatients();
    }

    public virtual void ClearPatientQueue()
    {
        patientQueue.ClearQueue();
    }
    
    private void TryMoveNpc()
    {
        if (patientQueue != null)
            TryMovePatients();
        if (employeeQueue != null)
            AskForEmployee();
    }
    
    protected virtual void TryMovePatients()
    {
        if (patientQueue.IsEmpty)
        {
            return;
        }
        
        foreach (var item in _items)
        {
            if (!item.HasPlace(InteractionType.Chronos))
            {
                continue;
            }
            
            item.SetChronos(patientQueue.GetNpc() as ChronosBehaviour);
            return;
        }
    }
}
