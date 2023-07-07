using System;
using System.Linq;
using Schedule_Movement.Scripts;
using Schedule_Movement.Scripts.Npc.Agents;
using Sirenix.OdinInspector;
using UnityEngine;

public class InteractionItem : MonoBehaviour
{
    [SerializeField] private InteractionRoom interactionRoom;
    [SerializeField] private InteractionType interactionType;
    [SerializeField] private bool isBought;
    [SerializeField] private InteractionPoint[] interactionPoint;
    
    public bool HasPlace(InteractionType targetType) => isBought && interactionPoint.Any(ip => ip.HasNpcPlace(targetType));

    public InteractionType InteractionType => interactionType;

    public event Action OnFree;
    
    private void Start()
    {
        UpdateItemState();

        foreach (var ip in interactionPoint)
        {
            ip.OnFree += () =>
            {
                OnFree?.Invoke();
            };
        }
    }


    [Button("Buy Item")]
    private void BuyItem()
    {
        if (isBought)
            return;
        
        isBought = true;
        UpdateItemState();
        OnFree?.Invoke();
    }

    public void SetAgent(Employee targetEmployee)
    {
        var targetPoint = GetPointByType(InteractionType.Agent);
        if (targetPoint == null)
        {
            Debug.LogError($"There's no free points");
            return;
        }
        
        targetPoint.SetAgent(targetEmployee);
    }
    
    public void SetChronos(ChronosBehaviour targetChronos)
    {
        var targetPoint = GetPointByType(InteractionType.Chronos);
        if (targetPoint == null)
        {
            Debug.LogError($"There's no free points");
            return;
        }
        
        targetPoint.SetChronos(targetChronos);
    }

    private InteractionPoint GetPointByType(InteractionType targetType) =>
        interactionPoint.FirstOrDefault(ip => ip.HasNpcPlace(targetType));


    public void AddItemWithoutEmployee()
    {
        interactionRoom.AskForEmployee();
        Debug.Log($"{gameObject.name} Asked for employee");
    }
    
    private void UpdateItemState()
    {
        gameObject.SetActive(isBought);
    }
}
