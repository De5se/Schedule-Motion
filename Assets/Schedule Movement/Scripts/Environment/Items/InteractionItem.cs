using System;
using System.Linq;
using Schedule_Movement.Scripts.Environment.InteractionRooms;
using Schedule_Movement.Scripts.Npc.Agents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment.Items
{
    public class InteractionItem : MonoBehaviour
    {
        [SerializeField] private InteractionRoom interactionRoom;
        [SerializeField] private InteractionType interactionType;
        [SerializeField] private bool isBought;
        [ShowInInspector, ReadOnly]
        protected InteractionPoint[] InteractionPoint;
    
        public bool HasPlace(InteractionType targetType) => isBought && InteractionPoint.Any(ip => ip.HasNpcPlace(targetType));

        public InteractionType InteractionType => interactionType;

        public event Action OnFree;
        public event Action OnInteractionStarted;
        public event Action OnInteractionFinished;


        protected virtual  void Awake()
        {
            InteractionPoint = GetComponentsInChildren<InteractionPoint>(true);
        }

        protected virtual void Start()
        {
            UpdateItemState();

            foreach (var ip in InteractionPoint)
            {
                ip.OnFree += () =>
                {
                    OnFree?.Invoke();
                };
                ip.OnInteractionStarted += () =>
                {
                    OnInteractionStarted?.Invoke();
                };
                ip.OnInteractionFinished += () =>
                {
                    OnInteractionFinished?.Invoke();
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
            InteractionPoint.FirstOrDefault(ip => ip.HasNpcPlace(targetType));


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
}
